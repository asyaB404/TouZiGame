// ********************************************************************************************
//     /\_/\                           @file       NodeQueue.cs
//    ( o.o )                          @brief     Game07
//     > ^ <                           @author     Basya
//    /     \
//   (       )                         @Modified   2024111619
//   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// ********************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GamePlay.Core;
using NetWork.Client;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace GamePlay.Node
{
    /// <summary>
    /// Node队列，表示一列
    /// </summary>
    public class NodeQueue : MonoBehaviour
    {
        public int playerId = -1; //表示这个
        public int id = -1; //表示第几列
        private static int MaxQueueNode => NodeQueueManager.MAX_QUEUE_NODE; // 最大节点数（从NodeQueueManager获取）

        [SerializeField] private int sumScore; // 当前节点队列的总点数

        // 获取或设置总点数
        public int SumScore
        {
            get => sumScore;
            private set => sumScore = value;
        }

        [FormerlySerializedAs("nodePos")] [SerializeField]
        private Transform[] touziPos; // 节点的位置数组

        private Node[] Nodes; //节点数组

        private void Awake()
        {
            Nodes = new Node[MaxQueueNode];
            for (int i = 0; i < MaxQueueNode; i++)
            {
                Nodes[i] = touziPos[i].GetComponent<Node>();
                Nodes[i].Init(this);
            }
        }

        [SerializeField] private List<int> scores; //存储每个节点的点数

        [FormerlySerializedAs("nodeObjs")] [SerializeField]
        private List<GameObject> touziObjs; // 存储与节点关联的游戏对象（例如显示的骰子）

        public IReadOnlyList<int> Scores => scores; //提供只读的点数列表

        private readonly Dictionary<int, int> _scoreCounts = new(); //用于跟踪每个点数的出现次数

        //当鼠标进入节点时，缩放节点并播放动画
        public void OnPointerEnter(PointerEventData data)
        {
            if (StageManager.CurGameStage != GameStage.Place) return;
            switch (GameManager.GameMode)
            {
                case GameMode.Native:
                    if (playerId != GameManager.CurPlayerId) return; // 只允许当前玩家操作
                    break;
                case GameMode.Online:
                    // if(playerId == 1) return;
                    break;
                case GameMode.SoloWithAi:
                    if (playerId == 1) return;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (Node node in Nodes)
            {
                node.transform.DOKill(); // 停止所有当前的动画
                node.SetScale(MyGlobal.INITIAL_SCALE * MyGlobal.HOVER_SCALE_FACTOR, 0.3f); // 放大节点
            }

            HintManager.Instance.SetEventHint("NodeQueue");
        }

        //当鼠标离开节点时，恢复节点的原始大小
        public void OnPointerExit(PointerEventData data)
        {
            if (StageManager.CurGameStage != GameStage.Place) return;
            foreach (Node node in Nodes)
            {
                node.transform.DOKill(); // 停止所有当前的动画
                node.SetScale(MyGlobal.INITIAL_SCALE, 0.3f); // 恢复节点大小
            }

            HintManager.Instance.SetEventHint("");
        }

        //当鼠标按下时
        public void OnPointerDown(PointerEventData data)
        {
        }

        // 当鼠标松开时，根据游戏模式执行相应操作
        public void OnPointerUp(PointerEventData data)
        {
            if (StageManager.CurGameStage != GameStage.Place) return;
            if (data == null) return;
            if (data.pointerCurrentRaycast.gameObject == null) return;
            if (!touziPos.Contains(data.pointerCurrentRaycast.gameObject.transform)) return;
            if (playerId != GameManager.CurPlayerId) return; // 确保点击的是当前节点且是当前玩家操作
            // 根据不同的游戏模式执行不同的逻辑
            switch (GameManager.GameMode)
            {
                case GameMode.Native:
                    GameManager.Instance.AddTouzi(id); // 在本地模式下，向游戏中添加骰子
                    break;
                case GameMode.Online:
                    if (playerId == 1) return;
                    MyClient.Instance.AddTouziRequest(GameManager.CurPlayerId, id,
                        GameManager.Instance.CurScore);
                    break;
                case GameMode.SoloWithAi:
                    if (playerId == 1) return;
                    GameManager.Instance.AddTouzi(id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(); // 其他模式抛出异常
            }

            foreach (Node node in Nodes)
            {
                node.transform.DOKill(); // 停止所有当前的动画
                node.SetScale(MyGlobal.INITIAL_SCALE, 0.3f); // 恢复节点大小
            }
        }

        //添加一个节点
        public bool AddNode(int score)
        {
            if (scores.Count == MaxQueueNode) return false;
            GameObject node = NodeFactory.CreateTouzi(score, touziPos[scores.Count]);
            touziObjs.Add(node);
            scores.Add(score);
            AudioMgr.Instance.PlaySFX("SFX/place");
            // 更新点数出现次数
            if (!_scoreCounts.TryAdd(score, 1))
            {
                _scoreCounts[score]++;
            }

            HintManager.Instance.SetUpHint(playerId, "Hit" + _scoreCounts[score]);
            // 更新总分
            UpdateSumScore();
            return true;
        }

        /// <summary>
        /// 移除骰子
        /// </summary>
        /// <param name="removedScore"></param>
        /// <returns></returns>
        public bool RemoveNode(int removedScore)
        {
            if (scores.Count == 0) return false; // 如果队列为空，返回失败

            bool isRemoved = false;
            for (int i = scores.Count - 1; i >= 0; i--) // 从后往前遍历点数
            {
                var curScore = scores[i];
                if (curScore != removedScore) continue; // 如果当前点数不匹配，跳过

                // 删除节点和对应的游戏对象
                touziObjs[i].GetComponent<TouZi>().clear();
                // Destroy(touziObjs[i]);
                touziObjs.RemoveAt(i);
                scores.RemoveAt(i);
                HintManager.Instance.SetUpHint(playerId, "clear");
                // 更新点数计数
                if (_scoreCounts.TryGetValue(curScore, out int count))
                {
                    if (count == 1)
                        _scoreCounts.Remove(curScore); // 如果点数只出现一次，移除
                    else
                        _scoreCounts[curScore]--; // 否则减少计数
                }

                isRemoved = true;
            }

            if (!isRemoved) return false; // 如果没有成功删除，返回失败

            //更新总分和节点位置
            UpdateSumScore();
            UpdateTouziPos();

            return true;
        }

        //重置
        public void Reset()
        {
            sumScore = 0;
            scores.Clear();
            // Debug.Log("reset");
            // 清除所有节点对象
            for (int i = touziObjs.Count - 1; i >= 0; i--)
            {
                touziObjs[i].transform.DOKill();
                Destroy(touziObjs[i]);
            }

            _scoreCounts.Clear();
            touziObjs.Clear();
        }

        //更新总点数，根据骰子点数出现次数加权计算
        private void UpdateSumScore()
        {
            SumScore = 0;
            foreach (var kv in _scoreCounts)
            {
                int score = kv.Key;
                int count = kv.Value;
                if (count >= 2)
                {
                    SumScore += score * count * count; // 如果点数出现超过一次，按平方加权
                }
                else
                {
                    SumScore += score; // 否则直接加上点数
                }
            }
        }

        //更新节点的位置，根据当前的节点列表重新排列它们的位置
        private void UpdateTouziPos()
        {
            for (int i = 0; i < touziObjs.Count; i++)
            {
                // touziObjs[i].transform.DOMove(touziPos[i].position, 0.5f);
                MyTool.PlayParabola(touziObjs[i].transform, touziPos[i].position, 1, 0.8f);
                AudioMgr.Instance.PlaySFX("SFX/hit");
                touziObjs[i].transform.SetParent(Nodes[i].transform);
            }
        }
    }
}
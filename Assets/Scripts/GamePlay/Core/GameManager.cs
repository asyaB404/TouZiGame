// // ********************************************************************************************
// //     /\_/\                           @file       GameManager.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111820
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using System.Collections.Generic;
using GamePlay.Node;
using NetWork;
using UI.Panel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamePlay.Core
{
    public enum GameMode
    {
        Native = 0,
        Online = 1
    }

    public class GameManager : MonoBehaviour
    {
        public static GameMode GameMode { get; private set; } = GameMode.Native;
        public static GameManager Instance { get; private set; }
        [SerializeField] private int curPlayerId = 0;
        private int NextPlayerId => MyTool.GetNextPlayerId(curPlayerId);
        [SerializeField] private Sprite[] touzi;
        [SerializeField] private NodeQueueManager[] nodeQueueManagers;
        public IReadOnlyList<Sprite> Touzi => touzi;
        public IReadOnlyList<NodeQueueManager> NodeQueueManagers => nodeQueueManagers;

        /// <summary>
        /// 当前玩家Id,    0-1
        /// </summary>
        public int CurPlayerId => curPlayerId;

        /// <summary>
        /// 当前骰子的大小-1，  取值范围为0-5
        /// </summary>
        public int curScore = -1;

        private void Awake()
        {
            Debug.Log(NetWorkMgr.CloseServer()); //test
            Instance = this;
            for (int i = 0; i < nodeQueueManagers.Length; i++)
            {
                nodeQueueManagers[i].Init(i);
            }
        }

        public void NextToPlayerId()
        {
            curPlayerId++;
            curPlayerId %= MyGlobal.MAX_PLAYER_COUNT;
        }

        public void StartGame(int seed)
        {
            Random.InitState(seed);
            curScore = Random.Range(0, 6);
            GameUIPanel.Instance.RollDiceAnimation(curScore);
            Application.targetFrameRate = 9999;
        }

        /// <summary>
        /// 下一回合，更新玩家id，得到这次的骰子点数，播放动画
        /// </summary>
        public void NextTurn()
        {
            NextToPlayerId();
            curScore = Random.Range(0, 6);
            GameUIPanel.Instance.RollDiceAnimation(curScore);
        }

        /// <summary>
        /// 添加骰子
        /// </summary>
        /// <param name="playerId">玩家id</param>
        /// <param name="id">第几行</param>
        /// <param name="score">骰子点数大小</param>
        public void AddTouzi(int playerId, int id, int score)
        {
            NodeQueueManager playerNodeQueueManager = nodeQueueManagers[playerId];
            if (!playerNodeQueueManager.AddTouzi(id, score)) return;
            GameUIPanel.Instance.UpdateScoreUI(curPlayerId, playerNodeQueueManager);
            if (RemoveTouzi(NextPlayerId, id, score))
                GameUIPanel.Instance.UpdateScoreUI(NextPlayerId, nodeQueueManagers[NextPlayerId]);
            if (playerNodeQueueManager.CheckIsGameOver())
            {
                GameOver();
                return;
            }

            NextTurn();
        }

        public void AddTouzi(int id)
        {
            AddTouzi(curPlayerId, id, curScore);
        }


        public bool RemoveTouzi(int playerId, int id, int score)
        {
            NodeQueueManager playerNodeQueueManager = nodeQueueManagers[playerId];
            return playerNodeQueueManager.RemoveTouzi(id, score);
        }

        private void GameOver()
        {
            Reset();
        }

        private void Reset()
        {
            curScore = Random.Range(0, 6);
            curPlayerId = 0;
            foreach (var nodeQueueManager in nodeQueueManagers)
            {
                nodeQueueManager.Clear();
            }

            GameUIPanel.Instance.RollDiceAnimation(curScore);
            GameUIPanel.Instance.UpdateScoreUI(0, nodeQueueManagers[0]);
            GameUIPanel.Instance.UpdateScoreUI(1, nodeQueueManagers[1]);
        }

        #region Debug

        [Space(10)] [SerializeField] private int t1 = 0;
        [SerializeField] private int t2 = 0;
        [SerializeField] private int t3 = 0;

        [ContextMenu("add")]
        private void Test()
        {
            AddTouzi(t1, t2, t3);
        }

        [ContextMenu("add1")]
        private void Test1()
        {
            AddTouzi(curPlayerId, t2, t3);
        }

        [ContextMenu("clear")]
        private void Test2()
        {
            Reset();
        }

        #endregion
    }
}
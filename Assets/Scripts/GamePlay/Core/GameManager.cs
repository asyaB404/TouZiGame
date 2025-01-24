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
using UI.Panel;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GamePlay.Core
{
    public class GameManager : MonoBehaviour
    {
        #region 字段以及属性

        public static GameState GameState { get; private set; } = GameState.Idle;

        public static GameMode GameMode { get; set; } = GameMode.Native;

        public static GameManager Instance { get; private set; }

        /// <summary>
        /// 当前玩家Id
        /// </summary>
        public static int CurPlayerId => Instance.curPlayerId;

        /// <summary>
        /// 当前骰子点数大小
        /// </summary>
        public static int CurScore => Instance.curScore;

        [SerializeField] private int curPlayerId = 0;

        private int NextPlayerId => MyTool.GetNextPlayerId(curPlayerId);

        [FormerlySerializedAs("touzi")] [SerializeField]
        private Sprite[] touziSprites;

        [SerializeField] private NodeQueueManager[] nodeQueueManagers;

        public IReadOnlyList<Sprite> TouziSprites => touziSprites;

        public IReadOnlyList<NodeQueueManager> NodeQueueManagers => nodeQueueManagers;

        [SerializeField] private int curScore;

        public HoleCardManager[] holeCardManagers;

        /// <summary>
        /// 当前选中的底牌下标
        /// </summary>
        [FormerlySerializedAs("holeCardNumber")]
        public int holeCardIndex;

        #endregion

        private void Awake()
        {
            Instance = this;
            for (int i = 0; i < nodeQueueManagers.Length; i++)
            {
                nodeQueueManagers[i].Init(i);
            }

            for (int i = 0; i < holeCardManagers.Length; i++)
            {
                holeCardManagers[i].Init(i);
            }
        }


        //重置

        private void Reset()
        {
            GameState = GameState.Idle;
            // curScore = Random.Range(1, 7);
            curPlayerId = 0;
            foreach (var nodeQueueManager in nodeQueueManagers)
            {
                nodeQueueManager.Reset();
            }

            GameUIPanel.Instance.UpdateScoreUI(0);
            GameUIPanel.Instance.UpdateScoreUI(1);
        }

        //开始游戏

        public void StartGame(int seed)
        {
            GameState = GameState.Gaming;
            Random.InitState(seed);
            // curScore = Random.Range(1, 7);
            // GameUIPanel.Instance.RollDiceAnimation(curScore);
            Debug.Log(GameUIPanel.Instance);
            curPlayerId = 0;
            StageManager.Instance.NewHand();
            JackpotManager.Instance.NewGame();
            JackpotManager.Instance.NewHand();
            holeCardManagers[0].SetFirstHoleCard();
            holeCardManagers[1].SetFirstHoleCard();

            // HoleCardManager.Instance.HoleCardsInit();
            SetCurScore(0);
        }

        #region 一个回合内发生的事

        public void NextToPlayerId()
        {
            SetNewHoleCard(CurPlayerId, holeCardIndex); //更新骰子，要在更新玩家id前调用
            holeCardIndex = 0;
            curScore = holeCardManagers[curPlayerId].GetPocket(holeCardIndex).TouZiScore;
            curPlayerId++;
            curPlayerId %= MyGlobal.MAX_PLAYER_COUNT;
        }

        /// <summary>
        /// 下一回合，更新玩家id，得到这次的骰子点数，播放动画
        /// </summary>
        public void NextTurn()
        {
            NextToPlayerId();
            SetCurScore(0);
            StageManager.Instance.NewRound(curPlayerId);
            // curScore = Random.Range(1, 7);
            // GameUIPanel.Instance.RollDiceAnimation(curScore);
        }

        /// <summary>
        /// 添加骰子,将骰子添加到场上
        /// </summary>
        /// <param name="playerId">玩家id</param>
        /// <param name="id">第几行</param>
        /// <param name="score">骰子点数大小</param>
        public void AddTouzi(int playerId, int id, int score)
        {
            if (GameState == GameState.Idle) return;
            NodeQueueManager playerNodeQueueManager = nodeQueueManagers[playerId];
            if (!playerNodeQueueManager.AddTouzi(id, score)) return;
            GameUIPanel.Instance.UpdateScoreUI(curPlayerId);
            if (RemoveTouzi(NextPlayerId, id, score))
                GameUIPanel.Instance.UpdateScoreUI(NextPlayerId);
            if (playerNodeQueueManager.CheckIsGameOver())
            {
                OverOneHand();
                return;
            }

            NextTurn();
        }

        public void AddTouzi(int id)
        {
            AddTouzi(curPlayerId, id, curScore);
        }

        /// <summary>
        /// 移除骰子，一般用于对面骰子与自己骰子点数相同时
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="id"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public bool RemoveTouzi(int playerId, int id, int score)
        {
            NodeQueueManager playerNodeQueueManager = nodeQueueManagers[playerId];
            return playerNodeQueueManager.RemoveTouzi(id, score);
        }

        /// <summary>
        /// 获得新的底牌骰子
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="holeCardNumber"></param>
        public void SetNewHoleCard(int playerId, int holeCardNumber)
        {
            int nub = Random.Range(1, 7);
            holeCardManagers[playerId].SetHoleCard(holeCardNumber, nub);
        }

        /// <summary>
        /// 设置当前骰子点数大小
        /// </summary>
        /// <param name="amount"></param>

        public void SetCurScore(int amount = 0)
        {
            curScore = amount;
        }

        #endregion


        #region 一hand的起始和结束

        /// <summary>
        /// 结束一轮Hand，结算奖池
        /// </summary>
        public void OverOneHand()
        {
            int sumScore0 = GameManager.Instance.NodeQueueManagers[0].SumScore;
            int sumScore1 = GameManager.Instance.NodeQueueManagers[1].SumScore;
            JackpotManager.Instance.JackpotCalculation(sumScore0 > sumScore1 ? 0 : 1);
            if (sumScore0 == 0 || sumScore1 == 0)
            {
                //TODO:彻底结束
            }

            JackpotManager.Instance.NewHand();
            GameUIPanel.Instance.ShowOverPanel(sumScore0, sumScore1);
        }

        //重新开始第二hand，清空棋盘，分数，奖池
        public void ResetChessboard()
        {
            curPlayerId = 0;
            foreach (var nodeQueueManager in nodeQueueManagers) //清空棋盘
            {
                nodeQueueManager.Reset();
            }

            GameUIPanel.Instance.UpdateScoreUI(0);
            GameUIPanel.Instance.UpdateScoreUI(1); //重新计算分数（清空分数
            JackpotManager.Instance.NewHand(); //奖池清零（奖池结算在
            StageManager.Instance.NewHand(); //TODO先手权的转移
        }

        #endregion


        #region Debug

        [Space(10)] [SerializeField] private int t1 = 0;
        [SerializeField] private int t2 = 0;
        [SerializeField] private int t3 = 0;

        [ContextMenu("startGame")]
        private void TestStartGame()
        {
            StartGame(123);
        }

        [ContextMenu("ReSetGame")]
        private void TestResetGame()
        {
            Reset();
        }

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

    public enum GameMode
    {
        Native = 0,
        Online = 1
    }

    public enum GameState
    {
        Idle = 0, //游戏未开始
        Gaming = 1,
    }
}
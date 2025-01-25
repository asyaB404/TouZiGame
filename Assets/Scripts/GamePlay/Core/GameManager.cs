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
        public int CurScore => HoleCardManagers[curPlayerId].CurHoleCardScore;

        [SerializeField] private int curPlayerId = 0;
        private int NextPlayerId => MyTool.GetNextPlayerId(curPlayerId);

        [FormerlySerializedAs("touzi")]
        [SerializeField]
        private Sprite[] touziSprites;
        public IReadOnlyList<Sprite> TouziSprites => touziSprites;
        [SerializeField] private NodeQueueManager[] nodeQueueManagers;
        public IReadOnlyList<NodeQueueManager> NodeQueueManagers => nodeQueueManagers;
        [SerializeField] private HoleCardManager[] holeCardManagers;
        public IReadOnlyList<HoleCardManager> HoleCardManagers => holeCardManagers;

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
            JackpotManager.Instance.NewGame();
            StageManager.Instance.NewGame();
            Debug.Log(GameUIPanel.Instance);

            curPlayerId = StageManager.Instance.firstPlayerId;



            // JackpotManager.Instance.NewHand();//开始游戏会自动
            // JackpotManager.Instance.EnterRaise(StageManager.LoseID);

            holeCardManagers[0].ResetAllHoleCards();
            holeCardManagers[1].ResetAllHoleCards();
        }

        #region 一个回合内发生的事

        public void NextToPlayerId()
        {
            SetNewHoleCard(CurPlayerId); //更新骰子，要在更新玩家id前调用
            curPlayerId++;
            curPlayerId %= MyGlobal.MAX_PLAYER_COUNT;
        }

        /// <summary>
        /// 下一回合，更新玩家id，得到这次的骰子点数，播放动画
        /// </summary>
        public void NextTurn()
        {
            NextToPlayerId();
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
            //如果移除了对面的骰子同时更新对面的UI，不过老实说这点性能无所谓的，应该把对面的和我的分数UI合在一起的
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
            AddTouzi(curPlayerId, id, CurScore);
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
        public void SetNewHoleCard(int playerId)
        {
            holeCardManagers[playerId].SetHoleCard();
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
            
            JackpotManager.Instance.JackpotCalculation(sumScore0, sumScore1);
            if (sumScore0 == 0 || sumScore1 == 0)
            {
                //TODO:彻底结束
            }

            // JackpotManager.Instance.NewHand();
            
        }

        //重新开始第二hand，清空棋盘，分数，奖池
        public void ResetChessboard()
        {

            foreach (var nodeQueueManager in nodeQueueManagers) //清空棋盘
            {
                nodeQueueManager.Reset();
            }

            GameUIPanel.Instance.UpdateScoreUI(0);
            GameUIPanel.Instance.UpdateScoreUI(1); //重新计算分数（清空分数

            JackpotManager.Instance.NewHand(); //奖池清零（奖池结算在
            StageManager.Instance.NewHand(); //

            curPlayerId = StageManager.Instance.firstPlayerId;
        }

        #endregion


        #region Debug

        [Space(10)][SerializeField] private int t1 = 0;
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
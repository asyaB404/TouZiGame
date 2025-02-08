// // ********************************************************************************************
// //     /\_/\                           @file       GameManager.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111820
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************


using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FishNet;
using GamePlay.Node;
using NetWork;
using UI.Panel;
// using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GamePlay.Core
{
    public partial class GameManager : MonoBehaviour
    {
        #region 字段以及属性

        public static GameMode GameMode { get; set; } = GameMode.Native;

        public static GameManager Instance { get; private set; }

        /// <summary>
        /// 当前进行操作的玩家Id
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
        private readonly JackpotManager _jackpotManager = new();
        private readonly StageManager _stageManager = new();
        [SerializeField] private Camera chessboardCamera;
        [SerializeField] private Transform chessboard;

        #endregion
        private Vector2 min_d=new Vector2(0, MyGlobal.MIN_CAMERA_DISTANCE);
        private Vector2 max_d=new Vector2(0, MyGlobal.MAX_CAMERA_DISTANCE);

        private void ChessboardOpen()
        {
            chessboard.gameObject.SetActive(false);
            // chessboardCamera.transform.DOLocalPath( 0.5f).OnComplete(() =>
            // chessboard.gameObject.SetActive(true));
        }
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

            gameObject.SetActive(false);
        }

        public void Exit()
        {
            Reset();
            gameObject.SetActive(false);
            GameUIPanel.Instance.HideMe();
            if (GameMode != GameMode.Online) return;
            NetWorkMgr.CloseConnection();
            NetWorkMgr.CloseServer();
            holeCardManagers[0].HideShader();
            holeCardManagers[1].HideShader();
        }
        public void Restart()
        {
            switch (GameMode)
            {
                case GameMode.Native:
                    StartNativeGame(Random.Range(int.MinValue, int.MaxValue));
                    break;
                case GameMode.Online:
                    InitForOnline();
                    break;
                case GameMode.SoloWithAi:
                    StartSoloWaitAiGame(Random.Range(int.MinValue, int.MaxValue));
                    break;
            }
        }
        public void Reset()
        {
            curPlayerId = 0;
            foreach (var nodeQueueManager in nodeQueueManagers)
            {
                nodeQueueManager.Reset();
            }

            _stageManager.Reset();
            _jackpotManager.Reset();
            GameUIPanel.Instance.UpdateScoreUI(0);
            GameUIPanel.Instance.UpdateScoreUI(1);
            GameUIPanel.Instance.UpdateOnlineUI();
            StageManager.SetStage(GameStage.Idle);
        }

        /// <summary>
        /// 更新玩家ID
        /// </summary>
        public void NextToPlayerId()
        {
            curPlayerId++;
            curPlayerId %= MyGlobal.MAX_PLAYER_COUNT;
        }

        /// <summary>
        /// 用于放完骰子的下一回合，更新玩家id，得到这次的骰子点数，播放动画
        /// </summary>
        private void NextTurn()
        {
            SetNewHoleCard(CurPlayerId); //更新骰子，要在更新玩家id前调用
            if (_stageManager.TryNextRound()) //看看有没有到5个回合
            {
                int curPlayerSumScore = nodeQueueManagers[curPlayerId].SumScore;
                int nextPlayerSumScore = nodeQueueManagers[MyTool.GetNextPlayerId(curPlayerId)].SumScore;
                if (curPlayerSumScore > nextPlayerSumScore)
                {
                    // Debug.Log(curPlayerId);
                    NextToPlayerId(); //确保是分数较小的一方先加注
                    // Debug.Log(curPlayerId);
                }
                else if (curPlayerSumScore == nextPlayerSumScore)
                {
                    //TODO:虽然概率很小，两边分数相同。。？
                }

                //让当前玩家进入加注阶段
                if (_jackpotManager.CheckIfCanRaise())
                {
                    _jackpotManager.EnterRaise();
                }
            }
            else
            {
                NextToPlayerId();
                StageManager.SetStage(GameStage.Place);
            }

            switch (GameMode)
            {
                case GameMode.Native:
                    ShowBlankScreen();
                    break;
                case GameMode.Online:
                    GameUIPanel.Instance.SetWaitPanel(CurPlayerId != 0);
                    break;
                case GameMode.SoloWithAi:
                    GameUIPanel.Instance.SetWaitPanel(CurPlayerId != 0);
                    // if (curPlayerId == 1 && StageManager.CurGameStage == GameStage.Place) AiAddTouZi();
                    // AiCall();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 添加骰子,将骰子添加到场上
        /// </summary>
        /// <param name="playerId">玩家id</param>
        /// <param name="id">第几行</param>
        /// <param name="score">骰子点数大小</param>
        public void AddTouzi(int playerId, int id, int score)
        {
            if (StageManager.CurGameStage != GameStage.Place) return;
            NodeQueueManager playerNodeQueueManager = nodeQueueManagers[playerId];
            if (!playerNodeQueueManager.AddTouzi(id, score)) return;
            GameUIPanel.Instance.UpdateScoreUI(curPlayerId);
            if (RemoveTouzi(NextPlayerId, id, score))
                GameUIPanel.Instance.UpdateScoreUI(NextPlayerId);
            if (playerNodeQueueManager.CheckIsGameOver())
            {
                OverOneHand(true);
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
        private void SetNewHoleCard(int playerId)
        {
            holeCardManagers[playerId].SetHoleCard();
        }


        #region 阶段（stage，两次加注之间的整个阶段）

        /// <summary>
        /// 跟注或加注
        /// </summary>
        /// <param name="isRaise"></param>
        public void Call(bool isRaise)
        {
            _jackpotManager.Call(curPlayerId, isRaise);
            if (_jackpotManager.CheckIfCanRaise())
            {
                NextToPlayerId();
                _jackpotManager.EnterRaise();
            }
            else
            {
                if (curPlayerId != StageManager.FirstPlayerId) NextToPlayerId();
                _stageManager.NewStage();
            }

            switch (GameMode)
            {
                case GameMode.Native:
                    ShowBlankScreen();
                    break;
                case GameMode.Online:
                    GameUIPanel.Instance.SetWaitPanel(CurPlayerId != 0);
                    break;
                case GameMode.SoloWithAi:
                    GameUIPanel.Instance.SetWaitPanel(CurPlayerId != 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        /// <summary>
        /// 玩家弃牌
        /// </summary>
        public void Fold()
        {
            OverOneHand();
            GameUIPanel.Instance.SetWaitPanel(false);
        }

        #endregion

        #region 一hand的起始和结束

        /// <summary>
        /// 结束一轮Hand，结算奖池
        /// </summary>
        /// <param name="isSpecial">是否特殊方式结束</param>
        /// <param name="isWinerWaiver">是否胜者弃权</param>
        public void OverOneHand(bool isSpecial = false)
        {
            // bool isWinerWaiver=
            int sumScore0 = NodeQueueManagers[0].SumScore;
            int sumScore1 = NodeQueueManagers[1].SumScore;

            _jackpotManager.JackpotCalculation(sumScore0, sumScore1);
            StageManager.SetStage(GameStage.Calculation);

        }

        //重新开始第二hand，清空棋盘，分数，奖池,重新发底牌
        // public void ClearData()
        // {

        // }
        public void NewHand()
        {
            foreach (var nodeQueueManager in nodeQueueManagers) //清空棋盘
            {
                nodeQueueManager.Reset();
            }
            GameUIPanel.Instance.UpdateScoreUI(0);
            GameUIPanel.Instance.UpdateScoreUI(1);


            _stageManager.NewHand();
            _jackpotManager.NewHand();
            int sumScore0 = _jackpotManager.JackpotP1;
            int sumScore1 = _jackpotManager.JackpotP2;

            holeCardManagers[0].ResetAllHoleCards();
            holeCardManagers[1].ResetAllHoleCards();

            if (sumScore0 == 0 || sumScore1 == 0)
            {
                Debug.Log("结束了");
                CalculationCGPanel.Instance.Show(sumScore0 == 0);
            }
            else _jackpotManager.EnterRaise();
            // Debug.LogError("");
        }

        #endregion


        #region Debug

        [Space(10)][SerializeField] private int t1 = 0;
        [SerializeField] private int t2 = 0;
        [SerializeField] private int t3 = 0;

        [ContextMenu("startGame")]
        private void TestStartGame()
        {
            StartNativeGame(123);
        }

        [ContextMenu("ReSetGame")]
        private void TestResetGame()
        {
            Exit();
        }

        [ContextMenu("add")]
        private void Test()
        {
            AddTouzi(t1, t2, t3);
        }

        [ContextMenu("add1")]
        private void Test1()
        {
            // AddTouzi(curPlayerId, t2, t3);
            GameUIPanel.Instance.UpdateHint("你好0");
        }

        [ContextMenu("clear")]
        private void Test2()
        {
            Exit();
        }

        #endregion
    }

    public enum GameMode
    {
        Native = 0,
        Online = 1,
        SoloWithAi,
    }
}
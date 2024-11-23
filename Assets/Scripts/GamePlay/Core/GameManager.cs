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
using GamePlay.Node;
using UI.Panel;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GamePlay.Core
{
    public enum GameState
    {
        Idle,
        Gaming
    }

    public enum Player
    {
        P1,
        P2
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public const int MAX_PLAYER_COUNT = 2;
        [SerializeField] private int curPlayerId = 0;
        [SerializeField] private int nextPlayerId = 1;
        [SerializeField] private Sprite[] touzi;
        [SerializeField] private NodeQueueManager[] nodeQueueManagers;
        public IReadOnlyList<Sprite> Touzi => touzi;
        public IReadOnlyList<NodeQueueManager> NodeQueueManagers => nodeQueueManagers;
        public int CurPlayerId => curPlayerId;

        public int curScore = -1;

        private void Awake()
        {
            Instance = this;
            for (int i = 0; i < nodeQueueManagers.Length; i++)
            {
                nodeQueueManagers[i].playerId = i;
                nodeQueueManagers[i].Init();
            }
        }

        private void Start()
        {
            curScore = Random.Range(0, 6);
            GameUIPanel.Instance.RollDiceAnimation(curScore);
        }

        public void NextTurn()
        {
            nextPlayerId++;
            nextPlayerId %= MAX_PLAYER_COUNT;
            curPlayerId++;
            curPlayerId %= MAX_PLAYER_COUNT;
            curScore = Random.Range(0, 6);
            GameUIPanel.Instance.RollDiceAnimation(curScore);
        }

        public void AddTouzi(int playerId, int id, int score)
        {
            NodeQueueManager playerNodeQueueManager = nodeQueueManagers[playerId];
            if (!playerNodeQueueManager.AddTouzi(id, score)) return;
            RemoveTouzi(nextPlayerId, id, score);
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
            GameUIPanel.Instance.RollDiceAnimation(curScore);
            curPlayerId = 0;
            nextPlayerId = 1;
            foreach (var nodeQueueManager in nodeQueueManagers)
            {
                nodeQueueManager.Clear();
            }
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
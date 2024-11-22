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
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private int nextPlayerId = 1;
        [SerializeField] private Sprite[] touzi;
        [SerializeField] private NodeQueueManager[] nodeQueueManagers;
        public IReadOnlyList<Sprite> Touzi => touzi;

        public int CurPlayerId
        {
            get => nextPlayerId;
            private set => nextPlayerId = value;
        }

        public int curScore = -1;

        private void Awake()
        {
            Instance = this;
        }

        public void NextTurn()
        {
            nextPlayerId++;
            nextPlayerId %= MAX_PLAYER_COUNT;
        }

        public bool AddTouzi(int playerId, int id, int score)
        {
            NodeQueueManager playerNodeQueueManager = nodeQueueManagers[playerId];
            return playerNodeQueueManager.AddTouzi(id, score);
        }

        public bool RemoveTouzi(int playerId, int id, int score)
        {
            NodeQueueManager playerNodeQueueManager = nodeQueueManagers[playerId];
            return playerNodeQueueManager.RemoveTouzi(id, score);
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

        #endregion
    }
}
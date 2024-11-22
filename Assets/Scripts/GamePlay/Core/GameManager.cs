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
        [SerializeField] private Sprite[] touzi;
        [SerializeField] public static Sprite[] touzi1;
        [SerializeField] private NodeQueueManager[] nodeQueueManagers;
        public static Player CurTurn;
        public IReadOnlyList<Sprite> Touzi => touzi;
        public int curScore = -1;

        private void Awake()
        {
            Instance = this;
        }

        public void NextTurn()
        {
        }

        public bool AddTouzi(int playerId, int id, int score)
        {
            NodeQueueManager playerNodeQueueManager = nodeQueueManagers[playerId];
            return playerNodeQueueManager.AddTouzi(id, score);
        }

        #region Debug

        [Space(10)]
        [SerializeField]private int t1 = 0;
        [SerializeField]private int t2 = 0;
        [SerializeField]private int t3 = 0;
        [ContextMenu("add")]
        private void Test()
        {
            AddTouzi(t1, t2, t3);
        }


        #endregion
    }
}
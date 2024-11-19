// // ********************************************************************************************
// //     /\_/\                           @file       GameManager.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111820
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using GameKit.Dependencies.Utilities;
using UnityEngine;

namespace GamePlay.Core
{
    public enum GameState
    {
        Idle,
        Gaming
    }

    public enum Turn
    {
        P1,
        P2
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] touzi;
        [SerializeField] private int[] randomList = { 0, 1, 2, 3, 4, 5 };
        public int curScore = -1;

        public void NextTurn()
        {
            randomList.ShuffleArray();
            curScore = randomList[^1];
        }
    }
}
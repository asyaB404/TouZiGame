// // ********************************************************************************************
// //     /\_/\                           @file       GameManager.NativeGame.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2025013122
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

using UI.Panel;
using UnityEngine;

namespace GamePlay.Core
{
    public partial class GameManager
    {
        /// <summary>
        /// 开始本地游戏
        /// </summary>
        /// <param name="seed"></param>
        public void StartNativeGame(int seed)
        {
            SceneInitialize();
            // holeCardManagers[1].gameObject.SetActive(true);
            holeCardManagers[curPlayerId].Hide();
            GameMode = GameMode.Native;
            Random.InitState(seed);

            _jackpotManager.NewGame();
            _stageManager.NewGame();
            
            holeCardManagers[0].ResetAllHoleCards();
            holeCardManagers[1].ResetAllHoleCards();
            
            
            _jackpotManager.EnterRaise();
            // Debug.Log("start");
        }

        /// <summary>
        /// 显示黑屏，本地多人游戏切换玩家专用
        /// </summary>
        public void ShowBlankScreen()
        {
            // Debug.Log("show");
            _stageManager.ShowBlankScreen();
        }

        public void HideBlankScreen()
        {
            _stageManager.ShowHoleCards();
        }
    }
}
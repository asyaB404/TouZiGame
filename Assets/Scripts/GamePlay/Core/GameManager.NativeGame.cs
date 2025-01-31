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
        public void StartNativeGame(int seed)
        {
            gameObject.SetActive(true);
            GameUIPanel.Instance.ShowMe();
            Random.InitState(seed);
            _jackpotManager.NewGame();
            _stageManager.NewGame();
            holeCardManagers[0].ResetAllHoleCards();
            holeCardManagers[1].ResetAllHoleCards();
            TryEnterRaiseStage(false);
        }

        /// <summary>
        /// 显示黑屏，本地多人游戏切换玩家专用
        /// </summary>
        public void ShowBlankScreen()
        {
            _stageManager.ShowBlankScreen();
        }

        public void HideBlankScreen()
        {
            _stageManager.HideBlankScreen();
        }
    }
}
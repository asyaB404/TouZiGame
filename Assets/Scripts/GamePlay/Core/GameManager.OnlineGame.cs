// // ********************************************************************************************
// //     /\_/\                           @file       GameManager.OnlineGame.cs
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
        public void InitForOnline()
        {
            GameMode = GameMode.Online;
            gameObject.SetActive(true);
            GameUIPanel.Instance.ShowMe();
        }

        public void StartOnlineGame()
        {
            _jackpotManager.NewGame();
            _stageManager.NewGame();
            holeCardManagers[0].ResetAllHoleCards();
            holeCardManagers[1].ResetAllHoleCards();
            _jackpotManager.EnterRaise();
        }
    }
}
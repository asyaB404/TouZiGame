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
        /// <summary>
        /// 进入房间时或创建房间时调用
        /// </summary>
        public void InitForOnline()
        {
            GameMode = GameMode.Online;
            gameObject.SetActive(true);
            GameUIPanel.Instance.ShowMe();
            holeCardManagers[1].gameObject.SetActive(false);
            holeCardManagers[0].HideShader();
            EnterCameraAnim();
        }
        /// <summary>
        /// 房主开始游戏时所有客户端调用
        /// </summary>
        public void StartOnlineGame()
        {
            _jackpotManager.NewGame();
            _stageManager.NewGame();
            holeCardManagers[0].ResetAllHoleCards();
            _jackpotManager.EnterRaise();
            GameUIPanel.Instance.UpdateOnlineUI();
            
        }
    }
}
// // ********************************************************************************************
// //     /\_/\                           @file       GameUIPanel.Online.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2025020200
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

using FishNet;
using GamePlay.Core;
using NetWork.Client;
using NetWork.Server;
using TMPro;
using UnityEngine.UI;

namespace UI.Panel
{
    public partial class GameUIPanel
    {
        private void AwakeForOnline()
        {
            GetControl<Button>("GetReady").onClick.AddListener(() =>
            {
                isReady = !isReady;
                GetControl<TextMeshProUGUI>("GetReady_Text").text = isReady ? "已准备" : "未准备";
                MyClient.Instance.GetReadyRequest(isReady);
            });

            GetControl<Button>("StartOnlineGame").onClick.AddListener(() => { MyServer.Instance.StartGame(); });
        }

        private void ShowForOnline()
        {
            isReady = false;
            UpdateOnlineUI();
        }
        
        /// <summary>
        /// 更新与网络模式的UI，例如准备按钮，准备标志
        /// </summary>
        public void UpdateOnlineUI()
        {
            GetControl<TextMeshProUGUI>("GetReady_Text").text = isReady ? "已准备" : "未准备";
            GetControl<Button>("StartOnlineGame").gameObject.SetActive(GameManager.GameMode == GameMode.Online && InstanceFinder.NetworkManager.IsHostStarted);
            GetControl<Button>("GetReady").gameObject.SetActive(GameManager.GameMode == GameMode.Online);
        }
    }
}
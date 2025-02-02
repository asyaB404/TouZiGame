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
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public partial class GameUIPanel
    {
        [SerializeField] private GameObject[] readySigns;

        private void InitForOnline()
        {
            GetControl<Button>("GetReady").onClick.AddListener(() =>
            {
                bool isReady = readySigns[0].activeSelf;
                isReady = !isReady;
                GetControl<TextMeshProUGUI>("GetReady_Text").text = isReady ? "已准备" : "未准备";
                SetReadySign(0, isReady);
                //发送准备状态取决于自己的readysign的状态，并没有单独的数据层
                MyClient.Instance.GetReadyRequest(isReady);
            });

            GetControl<Button>("StartOnlineGame").onClick.AddListener(() => { MyServer.Instance.StartGame(); });
        }

        private void ShowForOnline()
        {
            //第一次打开是全部都是未准备
            foreach (var sign in readySigns)
            {
                sign.SetActive(false);
            }

            UpdateOnlineUI();
        }

        /// <summary>
        /// 更新与网络模式的UI，例如准备按钮，准备标志
        /// </summary>
        public void UpdateOnlineUI()
        {
            if (StageManager.CurGameStage != GameStage.Idle)
            {
                foreach (var sign in readySigns)
                {
                    sign.SetActive(false);
                }
            }

            bool isReady = readySigns[0].activeSelf;
            GetControl<TextMeshProUGUI>("GetReady_Text").text = isReady ? "已准备" : "未准备";
            GetControl<Button>("StartOnlineGame").gameObject.SetActive(GameManager.GameMode == GameMode.Online &&
                                                                       InstanceFinder.NetworkManager.IsHostStarted &&
                                                                       StageManager.CurGameStage == GameStage.Idle);
            GetControl<Button>("GetReady").gameObject.SetActive(GameManager.GameMode == GameMode.Online &&
                                                                StageManager.CurGameStage == GameStage.Idle);
        }

        public void SetReadySign(int playerId, bool flag)
        {
            readySigns[playerId].SetActive(flag);
        }
    }
}
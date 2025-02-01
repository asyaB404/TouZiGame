// // ********************************************************************************************
// //     /\_/\                           @file       MyClient.Jackpot.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2025020204
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

using Cysharp.Threading.Tasks;
using FishNet.Connection;
using FishNet.Object;
using GamePlay.Core;
using NetWork.Server;
using UI.Panel;

namespace NetWork.Client
{
    public partial class MyClient
    {
        public void CallRequest(bool isRaise)
        {
            MyServer.Instance.HandleCallRequest(isRaise);
        }

        [ObserversRpc]
        public void CallResponse(bool isRaise, NetworkConnection conn = null)
        {
            GameManager.Instance.Call(isRaise);
        }

        public void FoldRequest()
        {
            MyServer.Instance.HandleFoldRequest();
        }

        [ObserversRpc]
        public void FoldResponse(NetworkConnection conn)
        {
            GameManager.Instance.Fold();
            UniTask.Create(async () =>
            {
                await UniTask.Delay(3000);
                GameUIPanel.Instance.HideHandOverPanel();
                GameManager.Instance.NewHand();
            }).Forget();
        }
    }
}
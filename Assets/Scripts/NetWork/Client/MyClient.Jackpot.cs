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
            if (!CheckRpcCoolDown()) return;
            GameManager.Instance.Call(isRaise);
            MyServer.Instance.HandleCallRequest(isRaise);
        }

        [ObserversRpc]
        public void CallResponse(bool isRaise, NetworkConnection conn = null)
        {
            if (conn != null && conn.IsLocalClient) return;
            GameManager.Instance.Call(isRaise);
        }

        public void FoldRequest()
        {
            if (!CheckRpcCoolDown()) return;
            GameManager.Instance.Fold();
            MyServer.Instance.HandleFoldRequest();
        }

        [ObserversRpc]
        public void FoldResponse(NetworkConnection conn)
        {
            if (conn != null && conn.IsLocalClient) return;
            GameManager.Instance.Fold();
        }
    }
}
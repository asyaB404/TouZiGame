// // ********************************************************************************************
// //     /\_/\                           @file       MyServer.Call.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2025020204
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

using FishNet.Connection;
using FishNet.Object;
using GamePlay.Core;
using NetWork.Client;

namespace NetWork.Server
{
    public partial class MyServer
    {
        [ServerRpc(RequireOwnership = false)]
        public void HandleCallRequest(bool isRaise, NetworkConnection conn = null)
        {
            MyClient.Instance.CallResponse(isRaise, conn);
        }
    }
}
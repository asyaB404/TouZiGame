/********************************************************************
    Author:			Basyyya
    Date:			2025:1:7 15:28
    Description:	服务端脚本实例
    //说是服务端实例，但其实只是形式上像
    //由于框架的P2P方式,其实每个客户端都有一个服务端实例
    //但是也只有在所有客户端中仅有一个而已
*********************************************************************/

using System.Collections.Generic;
using FishNet;
using FishNet.CodeGenerating;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using GamePlay.Core;
using NetWork.Client;
using UnityEngine;
using UnityEngine.Serialization;

namespace NetWork.Server
{
    /// <summary>
    /// 服务端实例
    /// </summary>
    public partial class MyServer
    {
        [AllowMutableSyncType] public SyncList<bool> isReady = new(new List<bool>(MyGlobal.MAX_PLAYER_COUNT));

        [ServerRpc(RequireOwnership = false)]
        public void HandleGetReady(NetworkConnection conn = null)
        {
        }

        [ServerRpc(RequireOwnership = false)]
        public void SyncReadyStatus()
        {
            isReady.DirtyAll();
        }
    }
}
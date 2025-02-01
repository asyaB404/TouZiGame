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
using NetWork.Data;
using UI.Panel;
using UnityEngine;
using UnityEngine.Serialization;

namespace NetWork.Server
{
    /// <summary>
    /// 服务端实例
    /// </summary>
    public partial class MyServer
    {
        [Server]
        public bool CheckIsAllReady()
        {
            if (_players.Count < 2) return false;
            foreach (var connection in _players)
            {
                ConnData data = connection.CustomData as ConnData;
                if (data == null || !data.isReady) return false;
            }
            
            return true;
        }

        private void OnClientLostConnForGetReady()
        {
            GameUIPanel.Instance.SetReadySign(1,false);
        }

        [ServerRpc(RequireOwnership = false)]
        public void HandleGetReady(bool isReady, NetworkConnection conn = null)
        {
            if (conn == null) return;
            int index = _players.IndexOf(conn);
            ConnData customData = _players[index].CustomData as ConnData;
            customData.isReady = isReady;
            MyClient.Instance.GetReadyResponse(isReady,conn);
        }

        #region Debug
    
        [ContextMenu("readyTest")]
        public void ReadyTest()
        {
            foreach (var connection in _players)
            {
                ConnData data = connection.CustomData as ConnData;
                Debug.Log(data.ToString());
            }
        }
        #endregion
    }
}
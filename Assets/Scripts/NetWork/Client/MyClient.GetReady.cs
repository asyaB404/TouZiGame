/********************************************************************
    Author:			Basyyya
    Date:			2025:1:7 15:28
    Description:	客户端脚本实例
*********************************************************************/

using FishNet.Object;
using GamePlay.Core;
using NetWork.Server;
using UnityEngine;

namespace NetWork.Client
{
    public partial class MyClient
    {
        public void GetReadyRequest(bool isReady)
        {
            MyServer.Instance.HandleGetReady(isReady);
        }

        [ObserversRpc]
        public void GetReadyResponse()
        {
            //TODO:准备成功的UI
        }
    }
}
/********************************************************************
    Author:			Basyyya
    Date:			2025:1:7 15:28
    Description:	客户端脚本实例
*********************************************************************/

using FishNet.Connection;
using FishNet.Object;
using NetWork.Server;
using UI.Panel;

namespace NetWork.Client
{
    public partial class MyClient
    {
        public void GetReadyRequest(bool isReady)
        {
            MyServer.Instance.HandleGetReady(isReady);
        }

        [ObserversRpc]
        public void GetReadyResponse(bool isReady, NetworkConnection conn)
        {
            int playerId = 0;
            if (!conn.IsLocalClient) playerId = 1;
            GameUIPanel.Instance.SetReadySign(playerId, isReady);
        }
    }
}
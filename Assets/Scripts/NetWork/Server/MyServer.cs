//说是服务端实例，但其实只是形式上像
//由于框架的P2P方式,其实每个客户端都有一个服务端实例
//但是也只有在所有客户端中仅有一个而已


using FishNet;
using FishNet.Connection;
using FishNet.Object;
using GamePlay.Core;
using NetWork.Client;

namespace NetWork.Server
{
    /// <summary>
    /// 服务端实例
    /// </summary>
    public class MyServer : NetworkBehaviour
    {
        public static MyServer Instance { get; private set; }

        [ServerRpc]
        public void HandleAddTouziRequest(int playerId, int id, int score, NetworkConnection conn = null)
        {
            if (conn == Owner)
            {
                GameManager.Instance.AddTouzi(playerId, id, score);
            }

            MyClient.Instance.AddTouziResponse();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Instance = this;
            if (!IsServerStarted) gameObject.SetActive(false);
        }
    }
}
using FishNet.Connection;
using FishNet.Object;
using GamePlay.Core;
using NetWork.Server;

namespace NetWork.Client
{
    public class MyClient : NetworkBehaviour
    {
        public static MyClient Instance { get; private set; }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (IsOwner)
            {
                Instance = this;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        [ObserversRpc]
        public void StartGameResponse()
        {
            //由于服务端那边已经在请求中处理了，所以这里不用做处理
            if (IsServerStarted) return;
            //让客户端后手
            GameManager.Instance.NextToPlayerId();
        }

        #region AddTouzi

        public void AddTouziRequest(int playerId, int id, int score)
        {
            MyServer.Instance.HandleAddTouziRequest(playerId, id, score);
        }

        [ObserversRpc]
        public void AddTouziResponse(int playerId, int id, int score)
        {
            if (IsServerStarted)
            {
                //由于服务端那边已经在请求中处理了，所以这里不用做处理
            }
            else
            {
                if (playerId == GameManager.Instance.CurPlayerId)
                {
                    GameManager.Instance.AddTouzi(playerId, id, score);
                }
                else
                {
                    GameManager.Instance.AddTouzi(MyTool.GetNextPlayerId(playerId), id, score);
                }
            }
        }

        #endregion
    }
}
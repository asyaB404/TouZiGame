using FishNet.Connection;
using FishNet.Object;
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

        public void AddTouziRequest(int playerId, int id, int score)
        {
            MyServer.Instance.HandleAddTouziRequest(playerId, id, score);
        }

        [ObserversRpc]
        public void AddTouziResponse()
        {
        }
    }
}
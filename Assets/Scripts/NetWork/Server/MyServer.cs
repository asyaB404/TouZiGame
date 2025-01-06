using FishNet.Connection;
using FishNet.Object;

namespace NetWork.Server
{
    public class MyServer:NetworkBehaviour
    {
        [ServerRpc]
        public void AddTouziRequest(int id, NetworkConnection conn = null)
        {
        }
    }
}
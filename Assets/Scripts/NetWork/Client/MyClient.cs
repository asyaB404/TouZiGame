using FishNet.Object;

namespace NetWork.Client
{
    public class MyClient : NetworkBehaviour
    {
        public MyClient Instance { get; private set; }

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
    }
}
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
    public partial class MyClient : NetworkBehaviour
    {
        public static MyClient Instance { get; private set; }

        public static GameObject CreateInstance()
        {
            GameObject prefabRes = Resources.Load<GameObject>(MyGlobal.CLIENT_PREFABS_PATH);
            return Instantiate(prefabRes);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (IsOwner)
            {
                Instance = this;
                GameManager.Instance.InitForOnline();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        #region GameStart

        [ObserversRpc]
        public void StartGameResponse()
        {
            //由于服务端那边已经在请求中处理了，所以这里不用做处理
            if (IsServerStarted) return;
            GameManager.GameMode = GameMode.Online;
            GameManager.Instance.NextToPlayerId();
            //让客户端后手
            GameManager.Instance.StartOnlineGame();
        }

        #endregion
    }
}
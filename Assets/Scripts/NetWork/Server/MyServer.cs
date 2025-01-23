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

namespace NetWork.Server
{
    /// <summary>
    /// 服务端实例
    /// </summary>
    public partial class MyServer : NetworkBehaviour
    {
        [AllowMutableSyncType] public SyncVar<int> Seed = new();

        public static MyServer Instance { get; private set; }

        public static GameObject CreateInstance()
        {
            GameObject prefabRes = Resources.Load<GameObject>(MyGlobal.SERVER_PREFABS_PATH);
            return Instantiate(prefabRes);
            // GameObject obj = new GameObject("MyServer");
            // obj.AddComponent<NetworkObject>();
            // return obj.AddComponent<MyServer>();
        }

        [Server]
        public void StartGame()
        {
            GameManager.GameMode = GameMode.Online;
            GameManager.Instance.StartGame(Seed.Value);
            MyClient.Instance.StartGameResponse();
        }
        

        public void ResetState()
        {
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Instance = this;
            if (IsServerStarted)
            {
                Seed.Value = Random.Range(int.MinValue, int.MaxValue);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
        }

        #region Debug

        [ContextMenu(nameof(TestForStartGame))]
        private void TestForStartGame()
        {
            StartGame();
        }

        #endregion
    }
}
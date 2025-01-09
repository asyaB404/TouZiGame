/********************************************************************
    Author:			Basyyya
    Date:			2025:1:7 15:28
    Description:	客户端脚本实例
*********************************************************************/

using FishNet;
using FishNet.Object;
using GamePlay.Core;
using NetWork.Server;
using UnityEngine;

namespace NetWork.Client
{
    public class MyClient : NetworkBehaviour
    {
        public static MyClient Instance { get; private set; }

        public static GameObject CreateInstance()
        {
            GameObject prefabRes = Resources.Load<GameObject>(MyGlobal.CLIENT_PREFABS_PATH);
            return Instantiate(prefabRes);
            // GameObject obj = new GameObject("MyClient");
            // obj.AddComponent<NetworkObject>();
            // return obj.AddComponent<MyClient>();
        }

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
            GameManager.GameMode = GameMode.Online;
            GameManager.Instance.StartGame(MyServer.Instance.Seed.Value);
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
                if (playerId == GameManager.CurPlayerId)
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
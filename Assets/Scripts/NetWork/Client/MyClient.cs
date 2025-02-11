/********************************************************************
    Author:			Basyyya
    Date:			2025:1:7 15:28
    Description:	客户端脚本实例
*********************************************************************/

using System;
using ChatUI;
using FishNet.Object;
using GamePlay.Core;
using NetWork.Server;
using UnityEngine;

namespace NetWork.Client
{
    public partial class MyClient : NetworkBehaviour
    {
        private DateTime _lastRequestTime = DateTime.MinValue;
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
                ClientManager.Broadcast(new ChatMessage("系统", "玩家NoName加入了游戏"));
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            ClientManager.Broadcast(new ChatMessage("系统", "玩家NoName退出了游戏"));
        }

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

        /// <summary>
        /// 检查RPC冷却时间，true 为可以发送RPC，false 为冷却中
        /// </summary>
        /// <returns></returns>
        private bool CheckRpcCoolDown()
        {
            if (DateTime.Now - _lastRequestTime < TimeSpan.FromSeconds(1.1))
            {
                Debug.Log("请求太频繁，请稍等一秒");
                return false;
            }

            _lastRequestTime = DateTime.Now;
            return true;
        }
    }
}
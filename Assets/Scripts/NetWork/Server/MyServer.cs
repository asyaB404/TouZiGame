/********************************************************************
    Author:			Basyyya
    Date:			2025:1:7 15:28
    Description:	服务端脚本实例
    //说是服务端实例，但其实只是形式上像
    //由于框架的P2P方式,其实每个客户端都有一个服务端实例
    //但是也只有在所有客户端中仅有一个而已
*********************************************************************/


using System;
using System.Collections.Generic;
using ChatUI;
using FishNet.CodeGenerating;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using GamePlay.Core;
using NetWork.Client;
using NetWork.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NetWork.Server
{
    /// <summary>
    /// 服务端实例
    /// </summary>
    public partial class MyServer : NetworkBehaviour
    {
        private readonly List<NetworkConnection> _players = new();
        [AllowMutableSyncType] public SyncVar<int> Seed = new();

        public static MyServer Instance { get; private set; }

        public static GameObject CreateInstance()
        {
            GameObject prefabRes = Resources.Load<GameObject>(MyGlobal.SERVER_PREFABS_PATH);
            return Instantiate(prefabRes);
        }

        [Server]
        public void StartGame(bool isForce=false)
        {
            if (!CheckIsAllReady() && !isForce)
            {
                ServerManager.Broadcast(new ChatMessage("系统", "双方玩家未准备完毕"));
                return;
            }

            GameManager.GameMode = GameMode.Online;
            ResetAllReadyState();
            GameManager.Instance.StartOnlineGame();
            MyClient.Instance.StartGameResponse();
        }


        public void ResetState()
        {
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            Instance = this;
            if (!IsServerStarted)
            {
                gameObject.SetActive(false);
                return;
            }

            Seed.Value = Random.Range(int.MinValue, int.MaxValue);
            ServerManager.OnRemoteConnectionState += OnRemoteConnected;
            _players.Add(LocalConnection);
            LocalConnection.CustomData = new ConnData
            {
                name = LocalConnection.ClientId.ToString(),
                isReady = false
            };
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            if (StageManager.CurGameStage != GameStage.Idle) GameManager.Instance.Exit();
        }

        private void OnRemoteConnected(NetworkConnection conn, RemoteConnectionStateArgs args)
        {
            var state = args.ConnectionState;
            switch (state)
            {
                case RemoteConnectionState.Stopped:
                    if (!_players.Remove(conn))
                    {
                        Debug.LogError("players同步错误");
                    }

                    OnClientLostConnForGetReady();
                    if (_players.Count < MyGlobal.MAX_PLAYER_COUNT)
                    {
                        GameManager.Instance.Reset();
                    }

                    break;
                case RemoteConnectionState.Started:
                    _players.Add(conn);
                    conn.CustomData = new ConnData
                    {
                        name = conn.ClientId.ToString(),
                        isReady = false
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
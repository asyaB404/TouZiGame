// // ********************************************************************************************
// //     /\_/\                           @file       NetWorkMgr.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111617
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using System;
using FishNet;
using FishNet.Transporting;
using NetWork.Server;

namespace NetWork
{
    public static class NetWorkMgr
    {
        static NetWorkMgr()
        {
            InstanceFinder.ServerManager.OnServerConnectionState += arg =>
            {
                switch (arg.ConnectionState)
                {
                    case LocalConnectionState.Stopped:
                        break;
                    case LocalConnectionState.Starting:
                        break;
                    case LocalConnectionState.Started:
                        break;
                    case LocalConnectionState.Stopping:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
            InstanceFinder.ClientManager.OnClientConnectionState += args =>
            {
                switch (args.ConnectionState)
                {
                    case LocalConnectionState.Stopped:
                        break;
                    case LocalConnectionState.Starting:
                        break;
                    case LocalConnectionState.Started:
                        break;
                    case LocalConnectionState.Stopping:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
            //其实场景加载成功会激活两次，一次由server调用，一次由client调用
            InstanceFinder.SceneManager.OnClientLoadedStartScenes += (connection, isSever) =>
            {
                if (!isSever) return;
                if (connection.IsHost)
                {
                    InstanceFinder.ServerManager.Spawn(MyServer.CreateInstance().gameObject, connection);
                }
                // InstanceFinder.ServerManager.Spawn(MyClient.CreateInstance().gameObject, connection);
            };
        }

        public static bool JoinServer(string address, ushort port)
        {
            var flag = InstanceFinder.ClientManager.StartConnection(address, port);
            return flag;
        }
        
        public static bool JoinServer()
        {
            var flag = InstanceFinder.ClientManager.StartConnection();
            return flag;
        }

        public static bool CreateServer(string roomName, ushort port)
        {
            var flag = InstanceFinder.ServerManager.StartConnection(port);
            return flag;
        }

        public static bool CloseServer()
        {
            var flag = InstanceFinder.ServerManager.StopConnection(true);
            return flag;
        }
    }
}
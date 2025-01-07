// // ********************************************************************************************
// //     /\_/\                           @file       NetWorkMgr.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111617
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using System;
using System.Linq;
using FishNet;
using FishNet.Connection;
using FishNet.Transporting;
using NetWork.Server;
using Random = UnityEngine.Random;

namespace NetWork
{
    public static class NetWorkMgr
    {
        static NetWorkMgr()
        {
            InstanceFinder.ServerManager.OnServerConnectionState += (arg) =>
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
            InstanceFinder.SceneManager.OnClientLoadedStartScenes += (connection, isSever) =>
            {
                if (isSever)
                {
                    InstanceFinder.ServerManager.Spawn(MyServer.CreateInstance().gameObject, connection);
                }
                else
                {
                }
            };
        }

        public static bool JoinServer(string address, ushort port)
        {
            var flag = InstanceFinder.ClientManager.StartConnection(address, port);
            return flag;
        }

        public static bool CreateServer(string roomName, ushort port)
        {
            int seed = Random.Range(int.MinValue, int.MaxValue);
            MyGlobal.CurSeed = seed;
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
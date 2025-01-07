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
using Random = UnityEngine.Random;

namespace NetWork
{
    public static class NetWorkMgr
    {
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
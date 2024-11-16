// // ********************************************************************************************
// //     /\_/\                           @file       NetWorkMgr.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111617
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using FishNet;

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
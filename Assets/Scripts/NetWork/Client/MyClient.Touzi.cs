/********************************************************************
    Author:			Basyyya
    Date:			2025:1:7 15:28
    Description:	客户端脚本实例
*********************************************************************/

using System;
using FishNet.Connection;
using FishNet.Object;
using GamePlay.Core;
using NetWork.Server;

namespace NetWork.Client
{
    public partial class MyClient
    {
        public void AddTouziRequest(int playerId, int id, int score)
        {
            if (!CheckRpcCoolDown()) return;
            MyServer.Instance.HandleAddTouziRequest(playerId, id, score);
        }

        [ObserversRpc]
        public void AddTouziResponse(int playerId, int id, int score, NetworkConnection conn = null)
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
    }
}
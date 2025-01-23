using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using UI.Panel;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 奖池管理
/// </summary>
public class JackpotManager
{
    public int AnteNub
    {
        set
        {
            anteNub = value;
            GameManager.Instance.SetAnte(value);
        }
        get => anteNub;
    } //底注大小
    private int anteNub;
    private int jackpotNub0; //0是自己的奖池，1是对手的奖池
    private int jackpotNub1;
    public int SumJackpotNub { get { return jackpotNub0 + jackpotNub1; } }

    public static JackpotManager Instance { get { if (instance == null) instance = new JackpotManager(); return instance; } }
    public static JackpotManager instance;

    public void NewHand() //在结束了一次距骨骰后开启新的一局使用
    {
        AnteNub = StageManager.Instance.handNub+1;
        jackpotNub0 = 0;
        jackpotNub1 = 0;
    }
    public void Call(int playerid)
    {
        if (playerid == 0)
        {
            jackpotNub0 += AnteNub;
            GameManager.Instance.MyJetton -= AnteNub;
        }
        else
        {
            jackpotNub1 += AnteNub;
            GameManager.Instance.TheJetton -= AnteNub;
        }
        GameManager.Instance.SetJackpot( SumJackpotNub);
    }
    public void Raise(int playerId)
    {
        AnteNub += 1;
        Call(playerId);
    }
}
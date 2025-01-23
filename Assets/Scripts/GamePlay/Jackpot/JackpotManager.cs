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
    public int MyJetton
    {
        set
        {
            myJetton = value;
            GameUIPanel.Instance.SetJetton(myJetton, theJetton);
        }
        get => myJetton;
    }
    private int myJetton;
    private int theJetton;
    public int TheJetton
    {
        get => theJetton;
        set
        {
            theJetton = value;
            GameUIPanel.Instance.SetJetton(myJetton, theJetton);
        }
    }
    public void NewGame()
    {
        MyJetton = MyGlobal.INITIAL_CHIP;
        TheJetton = MyGlobal.INITIAL_CHIP;
    }
    public int AnteNub
    {
        set
        {
            anteNub = value;
            GameUIPanel.Instance.SetAnte(value);
        }
        get => anteNub;
    } //底注大小
    private int anteNub;
    private int jackpotNub0; //0是自己的奖池，1是对手的奖池
    private int jackpotNub1;
    public int SumJackpotNub { get { return jackpotNub0 + jackpotNub1; } }

    public static JackpotManager Instance { get { if (instance == null) instance = new JackpotManager(); return instance; } }
    public static JackpotManager instance;
    public void JackpotCalculation(int willerId)
    {
        if (willerId == 0)myJetton+=SumJackpotNub;
        else if (willerId == 1)theJetton+=SumJackpotNub;
    }
    public void NewHand() //在结束了一次距骨骰后开启新的一局使用
    {
        AnteNub = StageManager.Instance.handNub + 1;
        jackpotNub0 = 0;
        jackpotNub1 = 0;
    }
    public void Call(int playerid)
    {
        if (playerid == 0)
        {
            jackpotNub0 += AnteNub;
            MyJetton -= AnteNub;
        }
        else
        {
            jackpotNub1 += AnteNub;
            TheJetton -= AnteNub;
        }
        GameUIPanel.Instance.SetJackpot(sumJackpotNub: SumJackpotNub);
    }
    public void Raise(int playerId)
    {
        AnteNub += 1;
        Call(playerId);
    }
}
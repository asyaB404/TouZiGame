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

    public void NewGame()
    {
        AnteNub = 1;
        GameUIPanel.Instance.SetJackpot(sumJackpotNub: SumJackpotNub);
        MyJetton = MyGlobal.INITIAL_CHIP;
        TheJetton = MyGlobal.INITIAL_CHIP;
    }
    //进入加注环节
    public void EnterRaise(int loseID)
    {
        GameUIPanel.Instance.ShowRaisePanel(loseID == 0, loseID == 0 ? MyJetton : TheJetton, AnteNub, gameMode: GameManager.GameMode);
    }
    public void NewHand() //在结束了一次距骨骰后开启新的一局使用
    {
        AnteNub = StageManager.Instance.HandNub + 1;
        // int nub = MyJetton > AnteNub ? AnteNub : MyJetton;
        // MyJetton -= nub;
        // jackpotNub0 = nub;
        // nub = TheJetton > AnteNub ? AnteNub : TheJetton;
        // TheJetton -= nub;
        // jackpotNub1 = nub;
        GameUIPanel.Instance.SetJackpot(sumJackpotNub: SumJackpotNub);
    }
    public void JackpotCalculation(int willerId)
    {
        if (willerId == 0) MyJetton += SumJackpotNub;
        else if (willerId == 1) TheJetton += SumJackpotNub;
    }
    public void JackpotCalculation(int score0, int score1)//暂时未处理赢家不跟注的情况
    {
        if (score0 > score1) MyJetton += SumJackpotNub;
        else if (score0 < score1) TheJetton += SumJackpotNub;
        else if (score0 == score1)
        {
            MyJetton += SumJackpotNub / 2;
            TheJetton += SumJackpotNub / 2;
        }
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
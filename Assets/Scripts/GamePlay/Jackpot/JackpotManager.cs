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
    private int extraJackpot;//上一局留下的筹码

    public int SumJackpotNub { get { return jackpotNub0 + jackpotNub1 + extraJackpot; } }
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
    public void EnterRaise(int FirstPlayerId, bool canFold = true)
    {
        GameUIPanel.Instance.ShowRaisePanel(FirstPlayerId == 0, FirstPlayerId == 0 ? MyJetton : TheJetton, AnteNub, canFold, gameMode: GameManager.GameMode);
        StageManager.SetStage(GameStage.Raise);
        if(GameManager.GameMode == GameMode.Native){
            GameUIPanel.Instance.ShowSwitchoverButton(isRaise:true);
        }
    }
    public void NewHand() //在结束了一次距骨骰后开启新的一局使用
    {
        AnteNub = StageManager.Instance.handNub + 1;
        jackpotNub0 = 0;
        jackpotNub1 = 0;
        // int nub = MyJetton > AnteNub ? AnteNub : MyJetton;
        // MyJetton -= nub;
        // jackpotNub0 = nub;
        // nub = TheJetton > AnteNub ? AnteNub : TheJetton;
        // TheJetton -= nub;
        // jackpotNub1 = nub;
        GameUIPanel.Instance.SetJackpot(sumJackpotNub: SumJackpotNub);
        HintManager.Instance.SetHint1("FirstRaise");
    }
    /// <summary>
    /// 根据分数分发处理奖池
    /// </summary>
    /// <param name="score0">p1得分</param>
    /// <param name="score1">p2得分</param>
    /// <param name="winerWaiver">赢家是否不跟注</param>
    public void JackpotCalculation(int score0, int score1, bool winerWaiver = false)//暂未处理赢家不跟注的情况
    {
        string title, text1, text2;
        title = $"你{(score0 > score1 ? "赢" : "输")}了{SumJackpotNub}个筹码";
        // Debug.Log($"jackpotNub0:{jackpotNub0},jackpotNub1:{jackpotNub1},extraJackpot:{extraJackpot}");
        if (score0 == score1 || winerWaiver)//（不跟注时与平局同样处理方式）
        {
            MyJetton += (int)SumJackpotNub / 2;
            TheJetton += (int)SumJackpotNub / 2;
            extraJackpot = SumJackpotNub % 2;

            if (score0 == score1) title = $"打平了。。。给你返还奖池一半的奖金（向下取整）{(int)SumJackpotNub / 2}个筹码";
            else if (winerWaiver) title = $"由于赢家不愿意继续加注，给双方返还奖池一半的奖金（向下取整）{(int)SumJackpotNub / 2}个筹码";

        }
        else
        {
            extraJackpot=0;
            if (score0 > score1) MyJetton += SumJackpotNub;
            else TheJetton += SumJackpotNub;
        }
        text1 = $"你一共获得了：{score0}分";
        text2 = $"对手一共获得了：{score1}分";
        StageManager.SetStage(GameStage.Calculation);
        GameUIPanel.Instance.ShowOverPanel(title, text1, text2);//todo添加关于赢了多少筹码的描述
    }

    public void Call(int playerid)
    {
        int nub;
        // string export;
        if (playerid == 0)
        {
            nub = MyJetton > AnteNub ? AnteNub : MyJetton;
            jackpotNub0 += nub;
            MyJetton -= nub;
            // export = $"p1加注{nub}";
        }
        else
        {
            nub = TheJetton > AnteNub ? AnteNub : TheJetton;
            jackpotNub1 += nub;
            TheJetton -= nub;
            // export = $"p2加注{nub}";
        }
        // export += $"此时奖池为{SumJackpotNub},其中我方奖池为{jackpotNub0},对方奖池为{jackpotNub1},额外奖池为{extraJackpot}，{playerid}的筹码为{(playerid == 0 ? MyJetton : TheJetton)},{(playerid == 0 ? "p2" : "p1")}的筹码为{(playerid == 0 ? TheJetton : MyJetton)}";

        // Debug.Log(export);
        GameUIPanel.Instance.SetJackpot(sumJackpotNub: SumJackpotNub);
    }
    public void Raise(int playerId)
    {
        AnteNub += 1;
        Call(playerId);
    }
}
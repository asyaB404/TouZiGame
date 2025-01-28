using System;
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
    public int JettonP1
    {
        set
        {
            _jettonP1 = value;
            GameUIPanel.Instance.SetJetton(_jettonP1, _jettonP2);
        }
        get => _jettonP1;
    }
    private int _jettonP1;
    private int _jettonP2;
    public int JettonP2
    {
        get => _jettonP2;
        set
        {
            _jettonP2 = value;
            GameUIPanel.Instance.SetJetton(_jettonP1, _jettonP2);
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
        JettonP1 = MyGlobal.INITIAL_CHIP;
        JettonP2 = MyGlobal.INITIAL_CHIP;
    }
    //进入加注环节
    public void EnterRaise(int firstPlayerId, bool canFold = true)
    {
        GameUIPanel.Instance.ShowRaisePanel(firstPlayerId == 0, firstPlayerId == 0 ? JettonP1 : JettonP2, AnteNub, canFold, gameMode: GameManager.GameMode);
        firstRaisePlayerId = firstPlayerId;//用来判断加注环节被双方都进行过了一遍
        curPlayerId = firstRaisePlayerId;
        Debug.Log($"firstRaisePlayerId:{firstRaisePlayerId},curPlayerId:{curPlayerId}");
        StageManager.SetStage(GameStage.Raise);
        switch (GameManager.GameMode)
        {
            case GameMode.Native:
                StageManager.Instance.ShowBlankScreen();
                Debug.Log("????");
                break;
            case GameMode.Online:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void NewHand() 
    {
        AnteNub = StageManager.Instance.HandNub + 1;
        jackpotNub0 = 0;
        jackpotNub1 = 0;
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
        if (score0 == score1 || winerWaiver)//（不跟注时与平局同样处理方式）
        {
            JettonP1 += (int)SumJackpotNub / 2;
            JettonP2 += (int)SumJackpotNub / 2;
            extraJackpot = SumJackpotNub % 2;

            if (score0 == score1) title = $"打平了。。。给你返还奖池一半的奖金（向下取整）{(int)SumJackpotNub / 2}个筹码";
            else if (winerWaiver) title = $"由于赢家不愿意继续加注，给双方返还奖池一半的奖金（向下取整）{(int)SumJackpotNub / 2}个筹码";

        }
        else
        {
            extraJackpot = 0;
            if (score0 > score1) JettonP1 += SumJackpotNub;
            else JettonP2 += SumJackpotNub;
        }
        text1 = $"你一共获得了：{score0}分";
        text2 = $"对手一共获得了：{score1}分";
        StageManager.SetStage(GameStage.Calculation);
        GameUIPanel.Instance.ShowOverPanel(title, text1, text2);//todo添加关于赢了多少筹码的描述
    }

    public void Call()
    {
        int nub;
        // string export;
        if (curPlayerId == 0)
        {
            nub = JettonP1 > AnteNub ? AnteNub : JettonP1;
            jackpotNub0 += nub;
            JettonP1 -= nub;
            // export = $"p1加注{nub}";
        }
        else
        {
            nub = JettonP2 > AnteNub ? AnteNub : JettonP2;
            jackpotNub1 += nub;
            JettonP2 -= nub;
            // export = $"p2加注{nub}";
        }
        // export += $"此时奖池为{SumJackpotNub},其中我方奖池为{jackpotNub0},对方奖池为{jackpotNub1},额外奖池为{extraJackpot}，{playerid}的筹码为{(playerid == 0 ? MyJetton : TheJetton)},{(playerid == 0 ? "p2" : "p1")}的筹码为{(playerid == 0 ? TheJetton : MyJetton)}";

        // Debug.Log(export);
        GameUIPanel.Instance.SetJackpot(sumJackpotNub: SumJackpotNub);
    }
    public void Raise()
    {
        AnteNub += 1;
        Call();
    }
    public void Fold(){
        GameManager.Instance.OverOneHand(isWinerWaiver: curPlayerId != firstRaisePlayerId);
    }

    public int curPlayerId; //哪个玩家进行的加注/跟注/弃牌

    private int firstRaisePlayerId;
    //一名玩家加注后另一名玩家进行加注
    public void NextPlayer()
    {
        if (GameManager.GameMode == GameMode.Native)
        {

            if (curPlayerId != firstRaisePlayerId)//说明双方都已经加过注了，可以进入下一个阶段了
            {
                GameUIPanel.Instance.HideRaisePanel();
                GameUIPanel.Instance.SetWaitPanel(false);
                StageManager.Instance.NewStage();
                StageManager.Instance.ShowBlankScreen();
            }
            else
            {
                bool isFirstRaise = StageManager.Stage == 0;
                curPlayerId = (curPlayerId + 1) % 2;
                // raisePanelTitleText.text = (curPlayerId == 0 ? "p1" : "p2") + "玩家的加注时间";
                GameUIPanel.Instance.SetRaiseButtons(curPlayerId == 0,
                    curPlayerId == 0 ? JackpotManager.Instance.JettonP1 : JackpotManager.Instance.JettonP2,
                        JackpotManager.Instance.AnteNub, !isFirstRaise);
                StageManager.Instance.ShowBlankScreen();
            }

        }
        else if (GameManager.GameMode == GameMode.Online)
        {
           
        }
    }
}
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
    private int _jackpotP1;
    private int _jackpotP2;

    /// <summary>
    /// P1剩余的筹码
    /// </summary>
    public int JackpotP1
    {
        set
        {
            _jackpotP1 = value;
            GameUIPanel.Instance.SetJetton(_jackpotP1, _jackpotP2);
        }
        get => _jackpotP1;
    }

    public int JackpotP2
    {
        get => _jackpotP2;
        set
        {
            _jackpotP2 = value;
            GameUIPanel.Instance.SetJetton(_jackpotP1, _jackpotP2);
        }
    }

    public int AnteNub
    {
        set
        {
            _anteNub = value;
            GameUIPanel.Instance.SetAnte(value);
        }
        get => _anteNub;
    } //底注大小

    private int _anteNub;
    private int _jackpotNub0; //当前奖池p1已经投入的筹码
    private int _jackpotNub1;
    private int _extraJackpot; //上一局留下的筹码

    public int SumJackpotNub => _jackpotNub0 + _jackpotNub1 + _extraJackpot;
    public static JackpotManager Instance => _instance ??= new JackpotManager();
    private static JackpotManager _instance;

    public void NewGame()
    {
        AnteNub = 1;
        JackpotP1 = MyGlobal.INITIAL_CHIP;
        JackpotP2 = MyGlobal.INITIAL_CHIP;
        GameUIPanel.Instance.SetJackpot(sumJackpotNub: SumJackpotNub);
    }

    //进入加注环节
    public void EnterRaise(int firstPlayerId, bool canFold = true)
    {
        GameUIPanel.Instance.ShowRaisePanel(firstPlayerId == 0, firstPlayerId == 0 ? JackpotP1 : JackpotP2, AnteNub,
            canFold, gameMode: GameManager.GameMode);
        firstRaisePlayerId = firstPlayerId; //用来判断加注环节被双方都进行过了一遍
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
        _jackpotNub0 = 0;
        _jackpotNub1 = 0;
        GameUIPanel.Instance.SetJackpot(sumJackpotNub: SumJackpotNub);
        HintManager.Instance.SetHint1("FirstRaise");
    }

    /// <summary>
    /// 根据分数分发处理奖池
    /// </summary>
    /// <param name="score0">p1得分</param>
    /// <param name="score1">p2得分</param>
    /// <param name="winerWaiver">赢家是否不跟注</param>
    public void JackpotCalculation(int score0, int score1, bool winerWaiver = false) //暂未处理赢家不跟注的情况
    {
        var title = $"你{(score0 > score1 ? "赢" : "输")}了{SumJackpotNub}个筹码";
        if (score0 == score1 || winerWaiver) //（不跟注时与平局同样处理方式）
        {
            JackpotP1 += (int)SumJackpotNub / 2;
            JackpotP2 += (int)SumJackpotNub / 2;
            _extraJackpot = SumJackpotNub % 2;

            if (score0 == score1) title = $"打平了。。。给你返还奖池一半的奖金（向下取整）{(int)SumJackpotNub / 2}个筹码";
            else if (winerWaiver) title = $"由于赢家不愿意继续加注，给双方返还奖池一半的奖金（向下取整）{(int)SumJackpotNub / 2}个筹码";
        }
        else
        {
            _extraJackpot = 0;
            if (score0 > score1) JackpotP1 += SumJackpotNub;
            else JackpotP2 += SumJackpotNub;
        }

        var text1 = $"你一共获得了：{score0}分";
        var text2 = $"对手一共获得了：{score1}分";
        StageManager.SetStage(GameStage.Calculation);
        GameUIPanel.Instance.ShowOverPanel(title, text1, text2); //todo添加关于赢了多少筹码的描述
    }

    public void Call()
    {
        int nub;
        if (curPlayerId == 0)
        {
            nub = JackpotP1 > AnteNub ? AnteNub : JackpotP1;
            _jackpotNub0 += nub;
            JackpotP1 -= nub;
        }
        else
        {
            nub = JackpotP2 > AnteNub ? AnteNub : JackpotP2;
            _jackpotNub1 += nub;
            JackpotP2 -= nub;
        }
        GameUIPanel.Instance.SetJackpot(sumJackpotNub: SumJackpotNub);
    }

    public void Raise()
    {
        AnteNub += 1;
        Call();
    }

    public void Fold()
    {
        GameManager.Instance.OverOneHand(isWinerWaiver: curPlayerId != firstRaisePlayerId);
    }

    public int curPlayerId; //哪个玩家进行的加注/跟注/弃牌

    private int firstRaisePlayerId;

    //一名玩家加注后另一名玩家进行加注
    public void NextPlayer()
    {
        if (GameManager.GameMode == GameMode.Native)
        {
            if (curPlayerId != firstRaisePlayerId) 
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
                GameUIPanel.Instance.SetRaiseButtons(curPlayerId == 0,
                    curPlayerId == 0 ? JackpotManager.Instance.JackpotP1 : JackpotManager.Instance.JackpotP2,
                    JackpotManager.Instance.AnteNub, !isFirstRaise);
                StageManager.Instance.ShowBlankScreen();
            }
        }
        else if (GameManager.GameMode == GameMode.Online)
        {
        }
    }
}
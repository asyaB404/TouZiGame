using System;
using Cysharp.Threading.Tasks;
// using System;
using GamePlay.Core;
using UI.Panel;
using UnityEngine;

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

    /// <summary>
    /// 目前底注大小
    /// </summary>
    public int AnteNub
    {
        set
        {
            _anteNub = value;
            GameUIPanel.Instance.UpdateAnteUI(value);
        }
        get => _anteNub;
    }

    private int _anteNub;
    private int _jackpotNub0; //当前奖池p1已经投入的筹码
    private int _jackpotNub1;
    private int _extraJackpot; //上一局留下的筹码

    public int SumJackpotNub => _jackpotNub0 + _jackpotNub1 + _extraJackpot;
    private int _raiseCount; //记录当前加注次数,当加注数量大于等于玩家数时结束加注跟注阶段

    public void NewGame()
    {
        AnteNub = 1;
        JackpotP1 = MyGlobal.INITIAL_CHIP;
        JackpotP2 = MyGlobal.INITIAL_CHIP;

        _raiseCount = 0;

        GameUIPanel.Instance.UpdateJackpotUI(sumJackpotNub: SumJackpotNub);
    }

    public bool CheckIfCanRaise()
    {
        if (_raiseCount >= MyGlobal.MAX_PLAYER_COUNT)
        {
            _raiseCount = 0;
            GameUIPanel.Instance.HideRaisePanel();
            return false;
        }

        return true;
    }

    /// <summary>
    /// 让当前玩家进入下注阶段，经过两次Call后，调用NewStage进入Place阶段
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void EnterRaise()
    {
        _raiseCount++;
        StageManager.SetStage(GameStage.Raise);
        switch (GameManager.GameMode)
        {
            case GameMode.Native:
                GameManager.Instance.ShowBlankScreen();
                // Debug.Log("");
                GameUIPanel.Instance.ShowRaisePanel(isP1: GameManager.CurPlayerId == 0,
                    haveJackpot: GameManager.CurPlayerId == 0 ? JackpotP1 : JackpotP2,
                    needJackpot: AnteNub, canFold: StageManager.Turn > 1);
                break;
            case GameMode.Online:
                if (GameManager.CurPlayerId == 0)
                {
                    GameUIPanel.Instance.ShowRaisePanel(isP1: GameManager.CurPlayerId == 0,
                        haveJackpot: GameManager.CurPlayerId == 0 ? JackpotP1 : JackpotP2,
                        needJackpot: AnteNub, canFold: StageManager.Turn > 1);
                }

                GameUIPanel.Instance.SetWaitPanel(GameManager.CurPlayerId != 0);
                break;
            case GameMode.SoloWithAi:
                if (GameManager.CurPlayerId == 0)
                {
                    GameUIPanel.Instance.ShowRaisePanel(isP1: GameManager.CurPlayerId == 0,
                        haveJackpot: GameManager.CurPlayerId == 0 ? JackpotP1 : JackpotP2,
                        needJackpot: AnteNub, canFold: StageManager.Turn > 1);
                }
                // if (GameManager.CurPlayerId == 1 && StageManager.CurGameStage == GameStage.Raise) GameManager.Instance.AiCall();
                GameUIPanel.Instance.SetWaitPanel(GameManager.CurPlayerId != 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // Debug.LogError("进入加注阶段");
    }

    public void NewHand()
    {
        _raiseCount = 0;
        _jackpotNub0 = 0;
        _jackpotNub1 = 0;
        AnteNub = StageManager.Hand;
        GameUIPanel.Instance.UpdateJackpotUI(sumJackpotNub: SumJackpotNub);
        HintManager.Instance.SetConditionHint("FirstRaise");
    }

    /// <summary>
    /// 根据分数分发处理奖池
    /// </summary>
    /// <param name="score0">p1得分</param>
    /// <param name="score1">p2得分</param>
    /// <param name="winerWaiver">赢家是否不跟注</param>
    public void JackpotCalculation(int score0, int score1)
    {
        var title = $"你{(score0 > score1 ? "赢" : "输")}了{(score0 > score1 ? _jackpotNub1 : _jackpotNub0)}个筹码";

        bool winerWaiver = _raiseCount == 2;
        // Debug.Log(_raiseCount);

        if (score0 == score1 || winerWaiver) //（不跟注时与平局同样处理方式）
        {
            JackpotP1 += (int)SumJackpotNub / 2;
            JackpotP2 += (int)SumJackpotNub / 2;
            _extraJackpot = SumJackpotNub % 2;
            if (winerWaiver)
            {
                title = $"由于赢家不愿意继续加注，给双方返还奖池一半的奖金（向下取整）{(int)SumJackpotNub / 2}个筹码";
                HintManager.Instance.SetUpHint(score0 > score1 ? 0 : 1, "WinerFold");
            }
            else if (score0 == score1) title = $"打平了。。。给你返还奖池一半的奖金（向下取整）{(int)SumJackpotNub / 2}个筹码";
            // else if (winerWaiver) 
        }
        else
        {
            _extraJackpot = 0;
            if (score0 > score1) JackpotP1 += SumJackpotNub;
            else JackpotP2 += SumJackpotNub;
        }
        if (score0 != score1)
        {
            if (GameManager.GameMode != GameMode.Native)
            {
                if (score0 > score1)
                {
                    GameManager.Instance.winParticles[0].Play();
                    GameManager.Instance.winParticles[1].Play();
                }
                else
                {
                    GameManager.Instance.loseParticles[0].Play();
                    GameManager.Instance.loseParticles[1].Play();
                }
            }
            else
            {
                int winerId = score0 > score1 ? 0 : 1;
                GameManager.Instance.winParticles[winerId].Play();
                GameManager.Instance.loseParticles[1 - winerId].Play();
            }
        }
        else
        {
            GameManager.Instance.winParticles[0].Play();
            GameManager.Instance.winParticles[1].Play();
        }
        var text1 = $"p1一共获得了：{score0}分";
        var text2 = $"p2一共获得了：{score1}分";
        GameUIPanel.Instance.ShowHandOverPanel(title, text1, text2);
        AudioMgr.Instance.PlaySFX("SFX/pop");
        //网络对战自动关闭面板
        if (GameManager.GameMode != GameMode.Online) return;
        UniTask.Create(async () =>
        {
            await UniTask.Delay(3000);
            GameUIPanel.Instance.HideHandOverPanel();
            // GameManager.Instance.NewHand();
        }).Forget();
    }
    
    /// <summary>
    /// 加注或跟注
    /// </summary>
    /// <param name="callPlayerId"></param>
    /// <param name="isRaise">是否加注</param>
    public void Call(int callPlayerId, bool isRaise)
    {
        if (isRaise) AnteNub += 1;
        int nub;
        if (callPlayerId == 0)
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
        AudioMgr.Instance.PlaySFX("SFX/call");
        GameUIPanel.Instance.UpdateJackpotUI(SumJackpotNub);
    }

    public void Reset()
    {
        _raiseCount = 0;
        _extraJackpot = 0;
        _jackpotNub0 = 0;
        _jackpotNub1 = 0;
        GameUIPanel.Instance.UpdateJackpotUI(SumJackpotNub);
    }
}
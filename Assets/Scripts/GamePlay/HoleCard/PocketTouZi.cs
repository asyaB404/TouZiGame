using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Core;
// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PocketTouZi : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerExitHandler
{
    public bool canChick;//是否可以点击,动画状态下就不能点击
    public int playerId = -1; //表示这个玩家
    public int id = -1; //表示第几个骰子
    public int touZiNub = -1;//这枚骰子的数字
    private const float HOVER_SCALE_FACTOR = 1.1f; // 鼠标悬停时放大的缩放因子
    public SpriteRenderer spriteRenderer;
    private Vector3 initialScale=new(1,1); // 初始缩放大小

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("Enter");
        if (GameManager.GameState == GameState.Idle) return;
        switch (GameManager.GameMode)
        {
            case GameMode.Native:
                if (playerId != GameManager.CurPlayerId) return; // 只允许当前玩家操作
                break;
            case GameMode.Online:
                // if(playerId == 1) return;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        transform.DOKill(); // 停止所有当前的动画
        transform.DOScale(initialScale * HOVER_SCALE_FACTOR, 0.3f); // 放大节点
    }
    private const int SPIN_COUNT = 15;
    private const float ANIMATION_DURATION = 2f;
    private static IReadOnlyList<Sprite> Touzi => GameManager.Instance.Touzi;



    //当鼠标离开节点时，恢复节点的原始大小
    public void OnPointerExit(PointerEventData data)
    {
        if (GameManager.GameState == GameState.Idle) return;
        transform.DOKill(); // 停止当前动画
        transform.DOScale(initialScale, 0.3f); // 恢复节点的原始缩放
    }

    //当鼠标按下时
    public void OnPointerDown(PointerEventData data)
    {
    }

    // 当鼠标松开时，根据游戏模式执行相应操作
    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerCurrentRaycast.gameObject != gameObject//不是这个节点
        || playerId != GameManager.CurPlayerId //不是当前玩家
        || GameManager.GameState == GameState.Idle//游戏状态不是Idle
        || !canChick//不能点击
            )
            return;
        switch (GameManager.GameMode)
        {
            case GameMode.Native:
                // GameManager.Instance.AddTouzi(id); // 在本地模式下，向游戏中添加骰子
                GameManager.Instance.SetCurScore(id,touZiNub);//TODO
                break;
            case GameMode.Online:
                if (playerId == 1) return;
                GameManager.Instance.SetCurScore(id,touZiNub);
                // MyClient.Instance.AddTouziRequest(GameManager.CurPlayerId, id,
                //     GameManager.CurScore);
                break;
            default:
                throw new ArgumentOutOfRangeException(); // 其他模式抛出异常
        }
        transform.DOKill(); // 停止动画
        transform.DOScale(initialScale, 0.3f);
    }
    public void RollDiceAnimation(int finalIndex)
    {
        touZiNub=finalIndex;
        canChick = false;
        finalIndex -= 1;
        Sequence diceSequence = DOTween.Sequence();
        // 添加持续摇晃效果
        Tweener doShakePosition =  transform.DOShakePosition(
            duration: ANIMATION_DURATION,//持续时间
            strength: new Vector3(1, 1, 0), // 水平和垂直方向抖动
            vibrato: 20,
            randomness: 90,
            fadeOut: true
        );
        Tween shakeTween =  transform.DOShakeRotation(
            duration: ANIMATION_DURATION, // 摇晃的总持续时间
            strength: new Vector3(0, 0, 180), // 主要在 Z 轴方向旋转
            vibrato: 30, // 震动频率
            randomness: 90, // 随机性
            fadeOut: true // 衰减
        );
        // Tween MoveTween=touziImage.transform.DOMove(point, ANIMATION_DURATION);
        // 添加骰子面滚动动画
        for (int i = 0; i < SPIN_COUNT; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, Touzi.Count);
            diceSequence.AppendCallback(() => {  spriteRenderer.sprite = Touzi[randomIndex]; });
            diceSequence.AppendInterval(ANIMATION_DURATION / SPIN_COUNT);
        }

        diceSequence.AppendCallback(() => {  spriteRenderer .sprite= Touzi[finalIndex];canChick = true; });
        diceSequence.Play();
    }
}

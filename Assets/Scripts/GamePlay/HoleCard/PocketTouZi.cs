using System;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


/// <summary>
/// 底牌骰子
/// </summary>
public class PocketTouZi : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler,
    IPointerExitHandler
{
    [FormerlySerializedAs("canChick")] public bool canClick; //是否可以点击,动画状态下就不能点击
    public int playerId = -1; //表示这个玩家
    public int id = -1; //表示第几个骰子
    public int TouZiScore { get; private set; } = -1; //这枚骰子的点数大小
    public SpriteRenderer spriteRenderer;

    [SerializeField] private SpriteRenderer halo;

    private Vector3 _initialPos;
    private Tweener _scaleAnim;

    public void SetHalo(bool isActive)
    {
        halo.gameObject.SetActive(isActive);
    }

    private void Awake()
    {
        _initialPos = transform.position;
        halo = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (StageManager.CurGameStage != GameStage.Place) return;
        HintManager.Instance.SetHint2("HoleCardChoose");
        switch (GameManager.GameMode)
        {
            case GameMode.Native:
                if (playerId != GameManager.CurPlayerId) return; // 只允许当前玩家操作
                break;
            case GameMode.Online:
                if(playerId == 1) return;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _scaleAnim?.Kill(); 
        _scaleAnim = transform.DOScale(MyGlobal.INITIAL_SCALE * MyGlobal.HOVER_SCALE_FACTOR, 0.3f); // 放大节点
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (StageManager.CurGameStage != GameStage.Place) return;
        _scaleAnim?.Kill(); 
        _scaleAnim = transform.DOScale(MyGlobal.INITIAL_SCALE, 0.3f); // 恢复节点的原始缩放
        HintManager.Instance.SetHint2("");
    }

    public void OnPointerDown(PointerEventData data)
    {
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerCurrentRaycast.gameObject != gameObject //不是这个节点
            || playerId != GameManager.CurPlayerId //不是当前玩家
            || StageManager.CurGameStage != GameStage.Place //游戏状态不是Idle
            || !canClick //不能点击
           )
            return;
        switch (GameManager.GameMode)
        {
            case GameMode.Native:
                GameManager.Instance.HoleCardManagers[playerId].CurIndex = id;
                break;
            case GameMode.Online:
                if (playerId == 1) return;
                GameManager.Instance.HoleCardManagers[playerId].CurIndex = id;
                break;
            default:
                throw new ArgumentOutOfRangeException(); // 其他模式抛出异常
        }

        if (_scaleAnim != null) _scaleAnim.Kill(); // 停止缩放相关的动画
        _scaleAnim = transform.DOScale(MyGlobal.INITIAL_SCALE, 0.3f); // 恢复节点的原始缩放
    }

    public void SetTouZiScore(int amount)
    {
        TouZiScore = amount;
    }

    private const int SPIN_COUNT = 15;
    private const float ANIMATION_DURATION = 2f;
    private static IReadOnlyList<Sprite> Touzi => GameManager.Instance.TouziSprites;
    private Sequence _rollAnim;
    private Tweener _doShakePosition;
    private Tween _shakeTween;
    private readonly Vector3 _point = new(0.2f, .2f);

    public void RollDiceAnimation(int finalIndex)
    {
        _rollAnim?.Kill();
        _doShakePosition?.Kill();
        _shakeTween?.Kill();

        transform.position = _initialPos;
        transform.localRotation = Quaternion.Euler(0, 0, 0);


        canClick = false;
        finalIndex -= 1;
        _rollAnim = DOTween.Sequence();
        // 添加持续摇晃效果
        _doShakePosition = transform.DOShakePosition(
            duration: ANIMATION_DURATION, //持续时间
            strength: _point, // 水平和垂直方向抖动
            vibrato: 20,
            randomness: 90,
            fadeOut: true
        );
        _shakeTween = transform.DOShakeRotation(
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
            _rollAnim.AppendCallback(() => { spriteRenderer.sprite = Touzi[randomIndex]; });
            _rollAnim.AppendInterval(ANIMATION_DURATION / SPIN_COUNT);
        }

        _rollAnim.AppendCallback(() =>
        {
            spriteRenderer.sprite = Touzi[finalIndex];
            canClick = true;
        });
        _rollAnim.Play();
    }
}
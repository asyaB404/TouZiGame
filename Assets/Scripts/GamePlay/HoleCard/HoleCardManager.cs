using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 底牌骰子管理器
/// </summary>
public class HoleCardManager : MonoBehaviour//非单例
{
    public int ownerId; //属于的玩家id

    /// <summary>
    /// 底牌骰子们
    /// </summary>
    [SerializeField] private PocketTouZi[] holeCards;

    /// <summary>
    /// 当前选中的骰子索引
    /// </summary>
    [SerializeField] private int curIndex;
    // [SerializeField] private GameObject myShader;
    public  IReadOnlyList<PocketTouZi> HoleCards => holeCards;
    public int CurIndex
    {
        get => curIndex;
        set
        {
            if (value < 0 || value >= holeCards.Length) return;
            curIndex = value;
            for (int i = 0; i < holeCards.Length; i++)
            {
                holeCards[i].SetHalo(i == curIndex);
            }
        }
    }

    /// <summary>
    /// 当前选中的骰子点数大小
    /// </summary>
    public int CurHoleCardScore => holeCards[curIndex].TouZiScore;

    public void Init(int playerId)
    {
        ownerId = playerId;
        for (int j = 0; j < holeCards.Length; j++)
        {
            holeCards[j].Init();
            holeCards[j].playerId = playerId;
            holeCards[j].id = j;
            holeCards[j].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置新的底牌
    /// </summary>
    public void SetHoleCard()
    {
        int nub = Random.Range(1, 7);
        SetHoleCard(curIndex, nub);
    }

    private void SetHoleCard(int index, int amount)
    {
        PocketTouZi pocketTouZi = holeCards[index];
        pocketTouZi.gameObject.SetActive(true);
        pocketTouZi.RollDiceAnimation(amount);
        pocketTouZi.SetTouZiScore(amount);
    }

    /// <summary>
    /// 重置所有底牌大小
    /// </summary>
    public void ResetAllHoleCards()
    {
        for (int i = 0; i < holeCards.Length; i++)
        {
            holeCards[i].gameObject.SetActive(true);
            SetHoleCard(i, Random.Range(1, 7));
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show(){
        gameObject.SetActive(true);
    }
}
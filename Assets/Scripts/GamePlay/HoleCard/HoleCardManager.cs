using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;

// using Microsoft.Unity.VisualStudio.Editor;
using UI.Panel;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 底牌骰子管理器
/// </summary>
public class HoleCardManager : MonoBehaviour
{
    public int playerId; //属于的玩家id

    [SerializeField] private PocketTouZi[] holeCards;

    public void Init(int playerId)
    {
        this.playerId = playerId;
        for (int j = 0; j < holeCards.Length; j++)
        {
            holeCards[j].playerId = playerId;
            holeCards[j].id = j;
            // GameManager.Instance.AddHoleCard(pocketTouZis[j]);
            holeCards[j].gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 设置新的底牌
    /// </summary>
    /// <param name="holeCardIndex"></param>
    /// <param name="amount"></param>
    public void SetHoleCard(int holeCardIndex,int amount)
    {
        PocketTouZi pocketTouZi = GetPocket(holeCardIndex);
        pocketTouZi.gameObject.SetActive(true);
        pocketTouZi.RollDiceAnimation(amount);
        pocketTouZi.SetTouZiNub(amount);
    }

    //获取第一份手牌
    public void SetFirstHoleCard()
    {
        for (int i = 0; i < holeCards.Length; i++)
        {
            holeCards[i].gameObject.SetActive(true);
            GameManager.Instance.SetNewHoleCard(playerId, i);
        }
    }

    public PocketTouZi GetPocket(int holeCardIndex)
    {
        return holeCards[holeCardIndex];
    }
}
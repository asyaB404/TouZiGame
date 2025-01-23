using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;

// using Microsoft.Unity.VisualStudio.Editor;
using UI.Panel;
using UnityEngine;
using UnityEngine.UI;

public class HoleCardManager : MonoBehaviour
{
    // public int maxCardNub;//最大底牌量
    public int playerId; //属于的玩家id

    [SerializeField]private PocketTouZi[] holeCards;

    //初始化
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

    //获取第一份手牌
    public void SetFirstHoleCard()
    {
        for (int i = 0; i < holeCards.Length; i++)
        {
            holeCards[i].gameObject.SetActive(true);
            GameManager.Instance.SetNewHoleCard(playerId, i);
        }
    }
    public PocketTouZi GetPocket(int holeCardNumber)
    {
        return holeCards[holeCardNumber];
    }
}
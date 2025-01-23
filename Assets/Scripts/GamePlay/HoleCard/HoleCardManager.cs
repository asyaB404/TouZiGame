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

    public PocketTouZi[] pocketTouZis;

    //初始化
    public void Init(int playerId)
    {
        this.playerId = playerId;
        for (int j = 0; j < pocketTouZis.Length; j++)
        {
            pocketTouZis[j].playerId = playerId;
            pocketTouZis[j].id = j;
            // GameManager.Instance.AddHoleCard(pocketTouZis[j]);
            pocketTouZis[j].gameObject.SetActive(false);
        }
    }

    //获取第一份手牌
    public void GetFirstHoleCard()
    {
        for (int i = 0; i < pocketTouZis.Length; i++)
        {
            pocketTouZis[i].gameObject.SetActive(true);
            GameManager.Instance.SetNewHoleCard(playerId, i);
        }
    }
    public PocketTouZi GetPocket(int holeCardNumber)
    {
        return pocketTouZis[holeCardNumber];
    }
}
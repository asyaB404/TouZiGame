using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using UI.Panel;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct HoleCardStruct
{
    public HoleCardStruct(int maxNub, GameObject TouZI)
    {
        maxCardNub = maxNub;
        holeCards = new();
        holeCardObj = new();
        for (int i = 0; i < maxCardNub; i++)
        {
            holeCards.Add(Random.Range(1, 7));
            holeCardObj.Add(GameObject.Instantiate(TouZI));
        }

    }
    public int maxCardNub;//最大拥有底牌量
    public List<int> holeCards;//手牌
    public List<GameObject> holeCardObj;
}
public class HoleCard : MonoBehaviour
{
    public GameObject TouZiPrefab;//TODO心心念念我的特殊骰子
    public static HoleCard Instance { get; private set; }
    public int defaultMaxCardNub;
    public List< HoleCardStruct> holeCards=new();
    public List<GameObject> holeCardPanels;
    private void Awake()
    {
        Instance = this;
    }
    //获得第一份底牌
    public void HoleCardsInit()
    {
        int maxPlayerNub = 2;
        
        holeCards.Clear();
        for (int i = 0; i < maxPlayerNub; i++)
        {
            holeCards.Add(new HoleCardStruct(defaultMaxCardNub, TouZiPrefab));
            for (int j = 0; j < holeCards[i].holeCards.Count; j++)
            {
                Image touZiImage = holeCards[i].holeCardObj[j].transform.GetChild(1).GetComponent<Image>();
                touZiImage.transform.parent.SetParent(holeCardPanels[i].transform);
                Debug.Log(GameUIPanel.Instance);
                Debug.Log(touZiImage);
                Debug.Log(holeCards[i].holeCards[j]);
                Debug.Log(holeCardPanels[i].transform.position);
                GameUIPanel.Instance.RollDiceAnimation(touZiImage, holeCards[i].holeCards[j], holeCardPanels[i].transform.position);
            }
        }
    }
}

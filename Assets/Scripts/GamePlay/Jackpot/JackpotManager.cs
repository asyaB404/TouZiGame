using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JackpotManager : MonoBehaviour
{
    public int anteNub; //底注大小
    public Text anteText; //显示底注的文本

    public Text jackpotText; //显示奖池的文本
    public List<int> jackpotNubs; //0是自己的奖池，1是对手的奖池

    public void NewHand() //在结束了一次距骨骰后开启新的一局使用
    {
    }
}
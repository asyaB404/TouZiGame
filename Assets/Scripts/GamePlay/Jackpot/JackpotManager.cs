using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;

public class JackpotManager 
{
    public int anteNub; //底注大小

    public int jackpotNub0; //0是自己的奖池，1是对手的奖池
    public int jackpotNub1; 
    public int SumJackpotNub{get {return jackpotNub0 + jackpotNub1;}}

    public static JackpotManager Instance{get {if(instance == null) instance = new JackpotManager(); return instance;}}
    public static JackpotManager instance;
    public void NewHand() //在结束了一次距骨骰后开启新的一局使用
    {
        anteNub=StageManager.Instance.handNub;
        jackpotNub0=0;
        jackpotNub1=0;
    }
    public void Call(int playerid){
        if(playerid==0){
            jackpotNub0+=anteNub;
        }
        else{
            jackpotNub1+=anteNub;
        }
    }
    public void Raise(int playerId){
        anteNub+=1;
        Call(playerId);
    }
}
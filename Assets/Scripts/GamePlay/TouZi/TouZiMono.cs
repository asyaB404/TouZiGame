using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouZiMono : MonoBehaviour
{
    int playerId;
    TouZiType touZiType;
    TouZiData touZiData;
    public void Init(int playId,TouZiType touZiType)
    {
        this.playerId=playId;
        this.touZiType=touZiType;
        touZiData=TouZiManager.Instance.touZiDatas[(int)touZiType];
    }
    
}

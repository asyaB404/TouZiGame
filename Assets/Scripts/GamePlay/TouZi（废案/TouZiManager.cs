using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouZiManager : MonoBehaviour
{
    public static TouZiManager Instance { get; private set; }
    // [System.Serializable]
    // public struct TouStruct{
    //     public TouZiType touZiType;
    //     public TouZiData touZiData;
    // }
    // public List<TouStruct> touStructs;
    [Header("按照枚举的顺序排列")]
    public List<TouZiData> touZiDatas;
    private void Awake() {
        Instance = this;
    }
}

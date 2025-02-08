using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouZi : MonoBehaviour
{
    public static List<Vector3> vector3s = new List<Vector3>(){//骰子的六个面的位置
        new Vector3(0,0,0),
        new Vector3(-90,-90,0),
        new Vector3(-90,0,0),
        new Vector3(90,0,0),
        new Vector3(90,90,0),
        new Vector3(180,0,0)
    };
    
    public void RollAnim()
    {

    }
}

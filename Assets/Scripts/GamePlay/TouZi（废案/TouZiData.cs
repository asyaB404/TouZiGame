using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TouZiData", menuName = "TouZiData")]
public class TouZiData : ScriptableObject
{
    public List<Sprite> sprites;//精灵图片
    public List<float> probabilitys;//每个数字的出现概率
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateLightAroundx : MonoBehaviour
{
    // 旋转速度，可在 Inspector 面板中调整
    public float rotationSpeed = 30f;

    void Update()
    {
        // 每帧更新灯光的旋转
        // Time.deltaTime 表示从上一帧到当前帧所经过的时间
        // rotationSpeed * Time.deltaTime 计算出这一帧需要旋转的角度
        transform.Rotate(-rotationSpeed * Time.deltaTime,0f,0f, Space.World);
    }
}

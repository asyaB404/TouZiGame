using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateLightAroundx : MonoBehaviour
{
    // ��ת�ٶȣ����� Inspector ����е���
    public float rotationSpeed = 30f;

    void Update()
    {
        // ÿ֡���µƹ����ת
        // Time.deltaTime ��ʾ����һ֡����ǰ֡��������ʱ��
        // rotationSpeed * Time.deltaTime �������һ֡��Ҫ��ת�ĽǶ�
        transform.Rotate(-rotationSpeed * Time.deltaTime,0f,0f, Space.World);
    }
}

using System.Collections;
using System.Collections.Generic;
using GameKit.Dependencies.Utilities;
using UnityEditor;
using UnityEngine;

public class TouZi : MonoBehaviour
{
    [SerializeField] private MeshRenderer materials;
    public static List<Vector3> vector3s = new List<Vector3>(){//骰子的六个面的位置
        new Vector3(0,0,0),
        new Vector3(-90,-90,0),
        new Vector3(-90,0,0),
        new Vector3(90,0,0),
        new Vector3(90,90,0),
        new Vector3(180,0,0)
    };
    void Awake()
    {
        if (materials == null) materials = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
    }
    public void SetNub(int nub)
    {
        transform.localEulerAngles = vector3s[nub];
    }
    public void RollAnim()
    {

    }
    [ContextMenu("clear")]
    public void clear()
    {
        StartCoroutine(AnimateFloatProperty());
    }
    public const float duration = 1; // 动画持续时间
    private IEnumerator AnimateFloatProperty()
    {
        // 创建一个材质属性块
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        // 获取当前物体的材质属性块
        materials.GetPropertyBlock(propertyBlock);


        float elapsedTime = 0f;
        float startValue = 0f;
        float endValue = 1f;

        while (elapsedTime < duration)
        {
            // 计算当前时间点的插值
            float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            propertyBlock.SetFloat("_Float", currentValue);

            // 将修改后的属性块应用到物体
            materials.SetPropertyBlock(propertyBlock);

            // 更新已用时间
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // 确保最终值为 1
        propertyBlock.SetFloat("_Float", endValue);
        materials.SetPropertyBlock(propertyBlock);
        Destroy(gameObject);
        // 输出修改后的属性值进行调试
        float newValue = materials.sharedMaterial.GetFloat("_Float");
        // Debug.Log($"Float 属性修改后的值: {newValue}");
    }
}



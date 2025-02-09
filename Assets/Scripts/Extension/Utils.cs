using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

public static class Utils
{
    public static void SetFullScreen(bool isFullScreen)
    {
        if (isFullScreen)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height,
                FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(1280, 960, FullScreenMode.Windowed);
        }
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static Vector2Int ToVectorInt(this Vector2 vector2)
    {
        return new Vector2Int(Mathf.FloorToInt(vector2.x), Mathf.FloorToInt(vector2.y));
    }

    public static Vector3Int ToVectorInt(this Vector3 vector3)
    {
        return new Vector3Int(Mathf.FloorToInt(vector3.x), Mathf.FloorToInt(vector3.y), Mathf.FloorToInt(vector3.z));
    }

    public static int BisectLeft<T>(this IList<T> arr, T value, int low = 0, int high = -1,
        IComparer<T> comparer = null) where T : IComparable<T>
    {
        if (high == -1)
            high = arr.Count;
        comparer ??= Comparer<T>.Default;
        while (low < high)
        {
            int mid = (low + high) >> 1;
            if (comparer.Compare(arr[mid], value) < 0)
                low = mid + 1;
            else
                high = mid;
        }

        return low;
    }

    public static Transform[] GetAllChildTransforms(this Transform transform)
    {
        var res = new Transform[transform.childCount];
        for (var i = 0; i < transform.childCount; i++) res[i] = transform.GetChild(i);

        return res;
    }

    /// <summary>
    ///     销毁一个transform下的所有子物体
    /// </summary>
    /// <param name="transform"></param>
    public static void DestroyAllChildren(this Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; i--) Object.DestroyImmediate(transform.GetChild(i).gameObject);
    }

    /// <summary>
    ///     得到枚举中的随机值
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <returns>随机枚举值</returns>
    public static T GetRandomEnumValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    /// <summary>
    ///     洗牌算法，将一个数组或者列表以O(N)的复杂度随机打乱
    /// </summary>
    /// <param name="array">需要打乱的数组或列表</param>
    /// <typeparam name="T">元素类型</typeparam>
    public static void ShuffleArray<T>(this IList<T> array)
    {
        for (var i = array.Count - 1; i > 0; i--)
        {
            var j = UnityEngine.Random.Range(0, i + 1);
            (array[j], array[i]) = (array[i], array[j]);
        }
    }

    public static void ShuffleArray<T>(this IList<T> array, int seed)
    {
        Random rand = new(seed);

        for (var i = array.Count - 1; i > 0; i--)
        {
            var j = rand.Next(0, i + 1);
            (array[j], array[i]) = (array[i], array[j]);
        }
    }

    /// <summary>
    ///     bezier2D抛物线插值
    /// </summary>
    /// <param name="t"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Vector2 Bezier(float t, Vector2 a, Vector2 b, Vector2 c)
    {
        var ab = Vector2.Lerp(a, b, t);
        var bc = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(ab, bc, t);
    }
}
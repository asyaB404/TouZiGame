// // ********************************************************************************************
// //     /\_/\                           @file       NodeFactory.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024112216
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using GamePlay.Core;
using UnityEngine;
namespace GamePlay.Node
{

    public static class NodeFactory
    {
        /// <summary>
        /// 创建一个NodeGameObject，并将其设置为指定父级的子对象。
        /// </summary>
        /// <param name="index">骰子面值</param>
        /// <param name="parent">生成对象的父级Transform。</param>
        /// <returns>创建的GameObject，若索引无效则返回 null。</returns>
        public static GameObject CreateNode(int index, Transform parent = null)
        {
            //验证GameManager实例和索引有效性
            if (GameManager.Instance == null || index < 0 || index >= GameManager.Instance.Touzi.Count)
            {
                Debug.LogError($"无效的索引 {index} 或 GameManager 未初始化。");
                return null;
            }
            Sprite sprite = GameManager.Instance.Touzi[index];
            GameObject nodeGameObject = new GameObject("Node");

            //设置父级
            if (parent != null)
            {
                nodeGameObject.transform.SetParent(parent, worldPositionStays: false);
            }

            //添加SpriteRenderer组件并设置Sprite
            SpriteRenderer spriteRenderer = nodeGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

            return nodeGameObject;
        }
    }

}
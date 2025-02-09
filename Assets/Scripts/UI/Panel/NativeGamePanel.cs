// // ********************************************************************************************
// //     /\_/\                           @file       NativeGamePanel.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \                           本地游戏的面板UI
// //   (       )                         @Modified   2025013000
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

using System;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Panel
{
    public class NativeGamePanel : BasePanel<NativeGamePanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("btn1").onClick.AddListener(() =>
            {
                // Debug.Log("开始本地单人游戏"); //TODO：跳转游戏界面
                GameManager.Instance.StartSoloWaitAiGame(Random.Range(int.MinValue, int.MaxValue));
                
            });
            GetControl<Button>("btn2").onClick.AddListener(() =>
            {
                GameManager.Instance.StartNativeGame(Random.Range(int.MinValue, int.MaxValue));
            });
            GetControl<Button>("btn3").onClick.AddListener(HideMe);
        }
    }
}
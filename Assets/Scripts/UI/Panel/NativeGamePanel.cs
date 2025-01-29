// // ********************************************************************************************
// //     /\_/\                           @file       NativeGamePanel.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \                           本地游戏的面板UI
// //   (       )                         @Modified   2025013000
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class NativeGamePanel: BasePanel<NativeGamePanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("btn1").onClick.AddListener(() =>
            {
                Debug.Log("开始本地单人游戏");  //TODO：跳转游戏界面VS单人AI
            });
            GetControl<Button>("btn2").onClick.AddListener(() =>
            {
                Debug.Log("开始本地多人游戏");  //TODO：跳转游戏界面本地多人的逻辑
            });
            GetControl<Button>("btn3").onClick.AddListener(HideMe);
        }
    }
}
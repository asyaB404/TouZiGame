// // ********************************************************************************************
// //     /\_/\                           @file       GameStartPanel.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111617
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class GameStartPanel : BasePanel<GameStartPanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("btn1").onClick.AddListener(() => { NativeGamePanel.Instance.ShowMe(); });
            GetControl<Button>("btn2").onClick.AddListener(() => { MulGamePanel.Instance.ShowMe(); });
            GetControl<Button>("btn3").onClick.AddListener(() => { Debug.Log("打开设置界面"); });
        }

        public override void OnPressedEsc()
        {
            Application.Quit();
        }
    }
}
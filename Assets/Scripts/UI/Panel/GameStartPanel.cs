// // ********************************************************************************************
// //     /\_/\                           @file       GameStartPanel.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111617
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using UnityEngine.UI;

namespace UI.Panel
{
    public class GameStartPanel : BasePanel<GameStartPanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("btn2").onClick.AddListener(() => { MulGamePanel.Instance.ShowMe(); });
        }
    }
}
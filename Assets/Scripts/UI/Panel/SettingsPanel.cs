// // ********************************************************************************************
// //     /\_/\                           @file       SettingsPanel.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2025020300
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class SettingsPanel : BasePanel<SettingsPanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Slider>("slider").onValueChanged.AddListener((float value) =>
            {
                AudioMgr.Instance.SetMusicVolume(value);
            });
            GetControl<Slider>("slider1").onValueChanged.AddListener((float value) =>
            {
                AudioMgr.Instance.SetSFXVolume(value);
            });
            GetControl<Toggle>("fullScreenToggle").onValueChanged.AddListener(Utils.SetFullScreen);
            GetControl<Button>("return").onClick.AddListener(HideMe);
        }

        public override void ShowAnim()
        {
            base.ShowAnim();
            GetControl<Slider>("slider").value = AudioMgr.Instance.musicVolume;
            GetControl<Slider>("slider1").value = AudioMgr.Instance.sfxVolume;
            GetControl<Toggle>("fullScreenToggle").isOn = Screen.fullScreen;
        }
    }
}
using GamePlay.Core;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace UI.Panel
{
    public class SwitchoverPanel : BasePanel<SwitchoverPanel>
    {
        public override void Init()
        {
            base.Init();
            GetControl<Button>("SwitchoverButton").onClick.AddListener(SwitchoverButtonChick);
        }

        //单机多人模式使用的按钮，用于切换玩家使遮挡屏幕
        public void ShowSwitchoverButton(string title, bool isRaise = false)
        {
            GetControl<TextMeshProUGUI>("SwitchText").text = title;
            // GetControl<Button>("SwitchoverPanel").gameObject.SetActive(true);
            ShowMe();
            Debug.Log("show");
        } 

        private void SwitchoverButtonChick() //确认切换了玩家后的按钮的点击事件
        {
            // GetControl<Button>("SwitchoverPanel").gameObject.SetActive(false);
            HideMe();
            // Debug.Log("Hide");
            HintManager.Instance.SetHint1("");
            GameManager.Instance.HideBlankScreen();
        }
        public override void CallBackWhenHeadPush(IBasePanel oldPanel)
        {
            // base.CallBackWhenHeadPop();
            ShowAnim();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using UI;
using UI.Panel;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : BasePanel<MenuPanel>
{
    public override void Init()
    {
        base.Init();
        GetControl<Button>("exit").onClick.AddListener(() =>
        {
            GameManager.Instance.Exit();
            Debug.Log("退出游戏");
        });
        GetControl<Button>("return").onClick.AddListener(HideMe);
    }

    public override void CallBackWhenHeadPush(IBasePanel oldPanel)
    {
        ShowAnim();
    }
}
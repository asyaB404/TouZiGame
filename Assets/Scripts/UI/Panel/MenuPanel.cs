using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : BasePanel<MenuPanel>
{
    public override void Init()
    {
        base.Init();
        GetControl<Button>("exit").onClick.AddListener(() => { GameManager.Instance.Exit(); });
        GetControl<Button>("return").onClick.AddListener(HideMe);
    }

    public override void CallBackWhenHeadPush(IBasePanel oldPanel)
    {
        ShowAnim();
    }
}
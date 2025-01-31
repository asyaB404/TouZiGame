using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : BasePanel<MenuPanel>
{
    public override void Init()
        {
            base.Init();
            GetControl<Button>("exit").onClick.AddListener(() => {  });
            GetControl<Button>("return").onClick.AddListener(() => {HideMe();});
        }
}

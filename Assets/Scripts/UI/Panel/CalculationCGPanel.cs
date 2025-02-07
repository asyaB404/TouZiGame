using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class CalculationCGPanel : BasePanel<CalculationCGPanel>
{
    [SerializeField] Sprite buleWin;
    [SerializeField] Sprite redWin;
    Image winImage;
    public override void Init()
    {
        base.Init();
        GetControl<Button>("AgainBtn").onClick.AddListener(Again);
        GetControl<Button>("ExitBtn").onClick.AddListener(Exit);
        winImage = GetControl<Image>("Image");

    }
    public override void CallBackWhenHeadPush(IBasePanel popPanel)
    {
        ShowAnim();
    }
    public void Show(bool isBuleWin)
    {
        ShowMe();
    }
    public override void ShowAnim()
    {
        base.ShowAnim();
    }
    public override void OnPressedEsc()
    {
        base.OnPressedEsc();
        Again();
    }
    private void Again()
    {
        HideMe();
        GameManager.Instance.Restart();
    }
    private void Exit()
    {
        HideMe();
        GameManager.Instance.Exit();
    }
}

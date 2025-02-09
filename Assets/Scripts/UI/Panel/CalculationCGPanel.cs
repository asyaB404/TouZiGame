using System.Collections;
using System.Collections.Generic;
using FishNet;
using GamePlay.Core;
using TMPro;
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

    public void Show(bool isBlue)
    {
        winImage.sprite = isBlue ? buleWin : redWin;
        ShowMe();
    }

    public override void ShowAnim()
    {
        base.ShowAnim();
        if (GameManager.GameMode == GameMode.Online)
        {
            GetControl<Button>("AgainBtn").gameObject.SetActive(InstanceFinder.NetworkManager.IsHostStarted);
            GetControl<TextMeshProUGUI>("onlineTip").gameObject.SetActive(!InstanceFinder.NetworkManager.IsHostStarted);
        }
        else
        {
            GetControl<Button>("AgainBtn").gameObject.SetActive(true);
            GetControl<TextMeshProUGUI>("onlineTip").gameObject.SetActive(false);
        }
    }

    public override void OnPressedEsc()
    {
        base.OnPressedEsc();
        Exit();
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
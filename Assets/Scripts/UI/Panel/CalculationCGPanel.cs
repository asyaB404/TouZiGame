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
    public void ShowMe(bool isBuleWin)
    {
        if (isBuleWin) winImage.sprite = buleWin;
        else winImage.sprite = redWin;
        base.ShowMe();
    }
    public override void OnPressedEsc()
    {
        base.OnPressedEsc();
        Again();
    }
    private void Again() {
        GameManager.Instance.Restart();
     }
    private void Exit() { 
        GameManager.Instance.Exit();
    }
}

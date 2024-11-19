// // ********************************************************************************************
// //     /\_/\                           @file       GameUIPanel.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111617
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using System.Collections.Generic;
using DG.Tweening;
using FishNet;
using GamePlay.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panel
{
    public class GameUIPanel : BasePanel<GameUIPanel>
    {
        private const int spinCount = 15;
        private const float animationDuration = 2f;
        private IReadOnlyList<Sprite> touzi => GameManager.Instance.Touzi;
        [SerializeField] private Image touziImage;

        public void UpdateTouZi()
        {
        }

        [ContextMenu("test")]
        public void RollDiceAnimation(int finalIndex)
        {
            Sequence diceSequence = DOTween.Sequence();
            for (int i = 0; i < spinCount; i++)
            {
                int randomIndex = Random.Range(0, touzi.Count); 
                diceSequence.AppendCallback(() =>
                {
                    touziImage.sprite = touzi[randomIndex]; 
                });
                diceSequence.AppendInterval(animationDuration / spinCount); 
            }
            diceSequence.AppendCallback(() => { touziImage.sprite = touzi[finalIndex]; });
            diceSequence.Play(); 
        }
    }
}
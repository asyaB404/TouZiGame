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
        private const int SPIN_COUNT = 15;
        private const float ANIMATION_DURATION = 2f;
        private static IReadOnlyList<Sprite> Touzi => GameManager.Instance.Touzi;
        [SerializeField] private Image touziImage;

        public void UpdateTouZi()
        {
        }

        public void RollDiceAnimation(int finalIndex)
        {
            Sequence diceSequence = DOTween.Sequence();
            // 添加持续摇晃效果
            Tweener doShakePosition = touziImage.transform.DOShakePosition(
                duration: ANIMATION_DURATION ,
                strength: new Vector3(10, 10, 0), // 水平和垂直方向抖动
                vibrato: 20,
                randomness: 90,
                fadeOut: true
            );
            Tween shakeTween = touziImage.transform.DOShakeRotation(
                duration: ANIMATION_DURATION, // 摇晃的总持续时间
                strength: new Vector3(0, 0, 180), // 主要在 Z 轴方向旋转
                vibrato: 30, // 震动频率
                randomness: 90, // 随机性
                fadeOut: true // 衰减
            );       

            // 添加骰子面滚动动画
            for (int i = 0; i < SPIN_COUNT; i++)
            {
                int randomIndex = Random.Range(0, Touzi.Count);
                diceSequence.AppendCallback(() => { touziImage.sprite = Touzi[randomIndex]; });
                diceSequence.AppendInterval(ANIMATION_DURATION / SPIN_COUNT);
            }

            diceSequence.AppendCallback(() =>
            {
                touziImage.sprite = Touzi[finalIndex]; 
            });
            diceSequence.Play();
        }



        #region debug

        [SerializeField] [Range(0, 5)] private int testIndex;

        [ContextMenu("test")]
        public void Test()
        {
            RollDiceAnimation(testIndex);
        }

        #endregion
    }
}
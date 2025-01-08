// // ********************************************************************************************
// //     /\_/\                           @file       GameUIPanel.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111617
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using System;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Core;
using GamePlay.Node;
using NetWork.Server;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Panel
{
    public class GameUIPanel : BasePanel<GameUIPanel>
    {
        private const int SPIN_COUNT = 15;
        private const float ANIMATION_DURATION = 2f;
        private static IReadOnlyList<Sprite> Touzi => GameManager.Instance.Touzi;
        [SerializeField] private Image touziImage;
        [SerializeField] private TextMeshProUGUI[] p1ScoreTexts;
        [SerializeField] private TextMeshProUGUI[] p2ScoreTexts;

        private void Awake()
        {
            GetControl<Button>("testBtn").onClick.AddListener(() => {MyServer.Instance.StartGame(); });
        }

        public void UpdateScoreUI(int playerId, NodeQueueManager nodeQueueManager)
        {
            TextMeshProUGUI[] texts;
            if (playerId == 0)
            {
                texts = p1ScoreTexts;
            }
            else
            {
                texts = p2ScoreTexts;
            }

            texts[^1].text = nodeQueueManager.SumScore.ToString();
            int i = 0;
            foreach (var nodeQueue in nodeQueueManager.NodeQueues)
            {
                texts[i].text = nodeQueue.SumScore.ToString();
                i++;
            }
        }

        public void RollDiceAnimation(int finalIndex)
        {
            finalIndex -= 1;
            Sequence diceSequence = DOTween.Sequence();
            // 添加持续摇晃效果
            Tweener doShakePosition = touziImage.transform.DOShakePosition(
                duration: ANIMATION_DURATION,
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

            diceSequence.AppendCallback(() => { touziImage.sprite = Touzi[finalIndex]; });
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
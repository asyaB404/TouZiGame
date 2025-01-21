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

        // [SerializeField] private Image touziImage;
        [SerializeField] private TextMeshProUGUI[] p1ScoreTexts;
        [SerializeField] private TextMeshProUGUI[] p2ScoreTexts;


        private void Awake()
        {
            GetControl<Button>("testBtn").onClick.AddListener(() => { MyServer.Instance.StartGame(); });
            SetButtonClick();
        }

        //计算分数
        public void UpdateScoreUI(int playerId)
        {
            var texts = playerId == 0 ? p1ScoreTexts : p2ScoreTexts;
            var nodeQueueManager = GameManager.Instance.NodeQueueManagers[playerId]; 
            texts[^1].text = nodeQueueManager.SumScore.ToString();
            int i = 0;
            foreach (var nodeQueue in nodeQueueManager.NodeQueues)
            {
                texts[i].text = nodeQueue.SumScore.ToString();
                i++;
            }
        }

        //摇骰子动画，finalIndex是最终结果
        public void RollDiceAnimation(Image touziImage, int finalIndex)
        {
            PocketTouZi pocketTouZi = touziImage.GetComponent<PocketTouZi>();
            finalIndex -= 1;
            Sequence diceSequence = DOTween.Sequence();
            // 添加持续摇晃效果
            Tweener doShakePosition = touziImage.transform.DOShakePosition(
                duration: ANIMATION_DURATION, //持续时间
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
            // Tween MoveTween=touziImage.transform.DOMove(point, ANIMATION_DURATION);
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

        #region 底注和回合数相关

        [SerializeField] private TextMeshProUGUI handNubText; //第几局
        [SerializeField] private TextMeshProUGUI stageNubText; //第几次加注
        [SerializeField] private TextMeshProUGUI roundNubText; //第几回合

        [SerializeField] private RectTransform buttonPanel; //按钮页面
        [SerializeField] private Button CallButton; //跟注按钮
        [SerializeField] private Button raiseButton; //加注按钮
        [SerializeField] private Button foldButton; //弃牌按钮

        public void SetRaiseButton(bool flag)
        {
            buttonPanel.gameObject.SetActive(flag);
            Debug.Log(flag);
        }

        public void SetHandNub(int handNub)
        {
            handNubText.text = handNub.ToString();
        }

        public void SetStageNub(int stageNub)
        {
            stageNubText.text = stageNub.ToString();
        }

        public void SetRoundNub(int roundNub)
        {
            roundNubText.text = roundNub.ToString();
        }

        private void SetButtonClick()
        {
            CallButton.onClick.AddListener(CallButtonClick);
            raiseButton.onClick.AddListener(RaiseButtonClick);
            foldButton.onClick.AddListener(FoldButtonClick);
        }

        private void CallButtonClick()
        {
            SetRaiseButton(false);
            StageManager.Instance.NewStage();
        }

        private void RaiseButtonClick()
        {
            SetRaiseButton(false);
            StageManager.Instance.NewStage();
        }

        private void FoldButtonClick()
        {
            SetRaiseButton(false);
            StageManager.Instance.NewHand(1);
        }

        #endregion

        #region debug

        [SerializeField] [Range(0, 5)] private int testIndex;

        [ContextMenu("test")]
        public void Test()
        {
            // RollDiceAnimation(testIndex);
        }

        #endregion
    }
}
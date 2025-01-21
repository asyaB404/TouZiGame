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
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI.Panel
{
    public class GameUIPanel : BasePanel<GameUIPanel>
    {
        private static IReadOnlyList<Sprite> Touzi => GameManager.Instance.TouziSprites;

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

        #region 底注和回合数相关

        [SerializeField] private TextMeshProUGUI handNubText; //第几局
        [SerializeField] private TextMeshProUGUI stageNubText; //第几次加注
        [SerializeField] private TextMeshProUGUI roundNubText; //第几回合

        [SerializeField] private RectTransform buttonPanel; //按钮页面
        [FormerlySerializedAs("CallButton")] [SerializeField] private Button callButton; //跟注按钮
        [SerializeField] private Button raiseButton; //加注按钮
        [SerializeField] private Button foldButton; //弃牌按钮

        public void SetRaiseButton(bool flag)
        {
            buttonPanel.gameObject.SetActive(flag);
            Debug.Log(flag);
        }

        public void SetNub(int handNub, int stageNub, int roundNub){
            roundNubText.text = (roundNub+1).ToString();
            handNubText.text = (handNub+1).ToString();
            stageNubText.text = (stageNub+1).ToString();
        }

        private void SetButtonClick()
        {
            callButton.onClick.AddListener(CallButtonClick);
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
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
            SetConfirmButton();
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



        [SerializeField] private GameObject HandOverPanel;//结束一次发牌到收牌后的页面，用于确认双方玩家分数以确认赢家
        [SerializeField] private TextMeshProUGUI[] HandOverTexts;//显示玩家分数
        [SerializeField] private Button confirmButton;//确认按钮
        private void SetConfirmButton() => confirmButton.onClick.AddListener(() =>
        {
            HandOverPanel.SetActive(false);
            GameManager.Instance.ResetChessboard();
        });
        public void ShowOverPanel(int score0, int score1)
        {
            HandOverPanel.SetActive(true);
            HandOverTexts[0].text = "你" + (score0 > score1 ? "赢" : "输") + "了";
            HandOverTexts[1].text = $"你一共获得了：{score0}分";
            HandOverTexts[2].text = $"对手一共获得了：{score1}分";
        }
        [SerializeField] private TextMeshProUGUI[] JackpotTexts; //
        public void SetJetton(int jackpot0, int jackpot1)
        {
            JackpotTexts[0].text = jackpot0.ToString();
            JackpotTexts[1].text = jackpot1.ToString();
            Debug.Log(jackpot0 + "   " + jackpot1);
        }
        #region 回合数

        [SerializeField] private TextMeshProUGUI handNubText; //第几局
        [SerializeField] private TextMeshProUGUI stageNubText; //第几次加注
        [SerializeField] private TextMeshProUGUI roundNubText; //第几回合



        public void SetStageNub(int handNub, int stageNub, int roundNub)
        {
            handNubText.text = handNub.ToString();
            stageNubText.text = stageNub.ToString();
            roundNubText.text = roundNub.ToString();
        }



        #endregion
        #region 筹码底注和奖池
        [SerializeField] private RectTransform buttonPanel; //按钮页面
        [FormerlySerializedAs("CallButton")][SerializeField] private Button callButton; //跟注按钮
        [SerializeField] private Button raiseButton; //加注按钮
        [SerializeField] private Button foldButton; //弃牌按钮
        [SerializeField] private TextMeshProUGUI raisePanelTitleText;//加注页面标题(显示谁来加注)


        // [SerializeField] private Button testButton; //对手加注按钮

        [SerializeField] private TextMeshProUGUI anteText;//底注
        [SerializeField] private TextMeshProUGUI jackpotText;//奖池
        [SerializeField] private RectTransform WaitPanel;

        public void SetAnte(int anteNub) =>
            anteText.text = anteNub.ToString();
        public void SetJackpot(int sumJackpotNub) =>
            jackpotText.text = sumJackpotNub.ToString();
        public void ShowRaisePanel(int loseID){
            if(loseID==0)ShowRaisePanel(loseID,JackpotManager.Instance.MyJetton);
            else ShowRaisePanel(loseID,JackpotManager.Instance.TheJetton);
        }
        //打开加注页面
        public void ShowRaisePanel(int loseID,int losePlayerJetton)
        {
            // WaitPanel.gameObject.SetActive(false);
            curPlayerId = loseID;
            this.loseID = loseID;
            if (GameManager.GameMode == GameMode.Native)
            {
                raisePanelTitleText.text = (loseID == 0 ? "你" : "对手")+"加注时间";
                ShowRaiseButtons(losePlayerJetton);
                buttonPanel.gameObject.SetActive(true);
            }
            else if (GameManager.GameMode == GameMode.Online)
            {
                if (curPlayerId == 0)
                {
                    // WaitPanel.gameObject.SetActive(false);//我猜可能应该这样写
                    // ShowRaiseButton(GameManager.Instance.TheJetton);
                }
                else if (curPlayerId == 1){
                    // HideRaiseButton();
                    // WaitPanel.gameObject.SetActive(true);
                }
            }
        }
        private void ShowRaiseButtons(int jeeton)
        {
            raiseButton.gameObject.SetActive(jeeton >= JackpotManager.Instance.AnteNub);//设置是否可以跟注和加注
            callButton.gameObject.SetActive(jeeton != 0);
        }
        public void HideRaiseButton()
        {
            buttonPanel.gameObject.SetActive(false);
        }
        // 设置按钮的点击事件监听器。
        private void SetButtonClick()
        {
            callButton.onClick.AddListener(CallButtonClick);
            raiseButton.onClick.AddListener(RaiseButtonClick);
            foldButton.onClick.AddListener(FoldButtonClick);

            // testButton.onClick.AddListener(TestButtonClick);
        }
        private int curPlayerId;//哪个玩家进行的加注/跟注/弃牌
        private int loseID;
        // 跟注按钮的点击事件。
        private void CallButtonClick()
        {
            // HideRaiseButton();
            JackpotManager.Instance.Call(curPlayerId);
            NextPlayer();
        }
        // 加注按钮的点击事件。
        private void RaiseButtonClick()
        {
            JackpotManager.Instance.Raise(curPlayerId);
            NextPlayer();
        }
        private void NextPlayer()
        {
            if (GameManager.GameMode == GameMode.Native)
            {
                
                if (curPlayerId != loseID)
                {
                    HideRaiseButton();
                    StageManager.Instance.NewStage();
                }
                else
                {
                    curPlayerId++;
                    Debug.Log("curPlayerId"+curPlayerId);
                }
                raisePanelTitleText.text = (loseID == 0 ? "你" : "对手")+"加注时间";
            }
            else if (GameManager.GameMode == GameMode.Online)
            {
                if (curPlayerId == 0)
                {
                    // WaitPanel.gameObject.SetActive(false);//我猜可能应该这样写，我也不懂
                    // ShowRaiseButton(GameManager.Instance.TheJetton);
                }
                else if (curPlayerId == 1){
                    // HideRaiseButton();
                    // WaitPanel.gameObject.SetActive(true);
                }
            }

        }
        // 弃牌按钮的点击事件。
        private void FoldButtonClick()
        {
            GameManager.Instance.OverOneHand();
        }
        #endregion
        #region debug

        [SerializeField][Range(0, 5)] private int testIndex;

        [ContextMenu("test")]
        public void Test()
        {
            // RollDiceAnimation(testIndex);
        }

        #endregion
    }
}
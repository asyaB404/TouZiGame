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
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Panel
{
    public class GameUIPanel : BasePanel<GameUIPanel>
    {
        [SerializeField] private TextMeshProUGUI[] p1ScoreTexts;
        [SerializeField] private TextMeshProUGUI[] p2ScoreTexts;

        private void Awake()
        {
            GetControl<Button>("testBtn").onClick.AddListener(() => { MyServer.Instance.StartGame(); });
            SetButtonClick();
            SetConfirmButton();
        }

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


        [FormerlySerializedAs("HandOverPanel")]
        [SerializeField]
        private GameObject handOverPanel; //结束一次发牌到收牌后的页面，用于确认双方玩家分数以确认赢家

        [FormerlySerializedAs("HandOverTexts")]
        [SerializeField]
        private TextMeshProUGUI[] handOverTexts; //显示玩家分数

        /// <summary>
        /// 打开分数确认页面
        /// </summary>
        /// <param name="score0">p1</param>
        /// <param name="score1">p2</param>
        /// <param name="score"></param>
        public void ShowOverPanel(string title, string score1, string score2)
        {
            handOverPanel.SetActive(true);
            handOverTexts[0].text = title;
            handOverTexts[1].text = score1;
            handOverTexts[2].text = score2;
        }
        [SerializeField] private Button confirmButton; //分数确认页面的关闭按钮

        //设置分数确认页面的关闭按钮的监听
        private void SetConfirmButton() => confirmButton.onClick.AddListener(() =>
        {
            handOverPanel.SetActive(false);
            GameManager.Instance.ResetChessboard();
        });

        [FormerlySerializedAs("JackpotTexts")]
        [SerializeField]
        private TextMeshProUGUI[] jackpotTexts; //双方的筹码的显示ui

        //设置筹码ui数值
        public void SetJetton(int jackpot0, int jackpot1)
        {
            jackpotTexts[0].text = jackpot0.ToString();
            jackpotTexts[1].text = jackpot1.ToString();
            // Debug.Log(jackpot0 + "   " + jackpot1);
        }

        #region 回合数

        [SerializeField] private TextMeshProUGUI handNubText; //第几局
        [SerializeField] private TextMeshProUGUI stageNubText; //第几次加注
        [SerializeField] private TextMeshProUGUI roundNubText; //第几回合


        //设置回合数据的ui
        public void SetStageNub(int handNub, int stageNub, int roundNub)
        {
            handNubText.text = handNub.ToString();
            stageNubText.text = stageNub.ToString();
            roundNubText.text = roundNub.ToString();
        }


        bool isRaise;//表示是加注阶段的切换玩家还是放置阶段的切换玩家
        //单机多人模式使用的按钮，用于切换玩家使遮挡屏幕
        public void ShowSwitchoverButton(bool isRaise = false)
        {
            this.isRaise = isRaise;
            GetControl<Button>("SwitchoverButton").gameObject.SetActive(true);
        }
        private void SwitchoverButtonChick()//确认切换了玩家后的按钮的点击事件
        {
            GetControl<Button>("SwitchoverButton").gameObject.SetActive(false);
            if (!isRaise) StageManager.Instance.HideBlankScreen();
            else StageManager.Instance.HideBlankScreen(curPlayerId);
        }
        #endregion

        #region 筹码底注和奖池

        [SerializeField] private RectTransform buttonPanel; //按钮页面

        [FormerlySerializedAs("CallButton")]
        [SerializeField]
        private Button callButton; //跟注按钮

        [SerializeField] private Button raiseButton; //加注按钮
        [SerializeField] private Button foldButton; //弃牌按钮
        [SerializeField] private TextMeshProUGUI raisePanelTitleText; //加注页面标题(显示谁来加注)
        [SerializeField] private TextMeshProUGUI anteText; //底注
        [SerializeField] private TextMeshProUGUI jackpotText; //奖池

        [FormerlySerializedAs("WaitPanel")]
        [SerializeField]
        private RectTransform waitPanel; //等待对方加注（计划在线模式使用

        //设置底牌ui
        public void SetAnte(int anteNub) =>
            anteText.text = anteNub.ToString();

        //设置奖池ui
        public void SetJackpot(int sumJackpotNub) =>
            jackpotText.text = sumJackpotNub.ToString();

        private int curPlayerId; //哪个玩家进行的加注/跟注/弃牌

        private int firstRaisePlayerId;


        [SerializeField]
        private TextMeshProUGUI callText;
        //打开加注页面
        public void ShowRaisePanel(bool isP1, int haveJackpot, int needFackpot, bool canFold = true, GameMode gameMode = GameMode.Native)
        {

            firstRaisePlayerId = isP1 ? 0 : 1;//用来判断加注环节被双方都进行过了一遍
            curPlayerId = firstRaisePlayerId;
            if (callText == null) callText = callButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            SetRaiseButtons(isP1, haveJackpot, needFackpot, canFold);

            if (gameMode == GameMode.Native) buttonPanel.gameObject.SetActive(true);
            // else if (gameMode == GameMode.Online)//我觉得应该这样写
            // {
            //     waitPanel.gameObject.SetActive(!isP1);
            //     buttonPanel.gameObject.SetActive(isP1);
            // }
        }
        //设置加注页面按钮
        private void SetRaiseButtons(bool isP1, int haveJackpot, int needFackpot, bool canFold = true)
        {
            raisePanelTitleText.text = isP1 ? "p1的加注时间" : "p2的加注时间";

            callButton.gameObject.SetActive(haveJackpot != 0);
            raiseButton.gameObject.SetActive(haveJackpot > needFackpot);
            callText.text = haveJackpot > needFackpot ? "跟注" : "AllIn!!!!";

            foldButton.gameObject.SetActive(canFold);
        }
        //关闭加注页面
        private void HideRaisePanel()
        {
            buttonPanel.gameObject.SetActive(false);
        }

        // 设置按钮的点击事件监听器。
        private void SetButtonClick()
        {
            SetButtonHint(callButton, "CallButton");
            SetButtonHint(raiseButton, "RaiseButton");
            SetButtonHint(foldButton, "FoldButton");

            callButton.onClick.AddListener(CallButtonClick);
            // callButton.mo
            raiseButton.onClick.AddListener(RaiseButtonClick);
            foldButton.onClick.AddListener(FoldButtonClick);

            GetControl<Button>("SwitchoverButton").onClick.AddListener(SwitchoverButtonChick);
        }
        // 为按钮添加事件触发器,用于显示鼠标进入的提示
        private void SetButtonHint(Button button, string str)
        {
            EventTrigger trigger = button.GetComponent<EventTrigger>();
            if (trigger == null) trigger = button.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entryEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
            };
            entryEnter.callback.AddListener((data) =>
            {
                HintManager.Instance.SetHint2(str);
            });
            EventTrigger.Entry entryExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit,
            };
            entryExit.callback.AddListener((data) =>
            {
                HintManager.Instance.SetHint2("");
            });
            Debug.Log(trigger);
            Debug.Log(entryEnter);
            Debug.Log(trigger.triggers);
            trigger.triggers.Add(entryEnter);
            trigger.triggers.Add(entryExit);
        }
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

        //一名玩家加注后另一名玩家进行加注
        private void NextPlayer()
        {
            if (curPlayerId != firstRaisePlayerId)//说明双方都已经加过注了，可以进入下一个阶段了
            {
                HideRaisePanel();
                waitPanel.gameObject.SetActive(false);
                StageManager.Instance.NewStage();
            }
            if (GameManager.GameMode == GameMode.Native)
            {
                bool isFirstRaise = StageManager.Stage == 0;
                curPlayerId = (curPlayerId + 1) % 2;
                raisePanelTitleText.text = (curPlayerId == 0 ? "p1" : "p2") + "玩家的加注时间";
                SetRaiseButtons(curPlayerId == 0,
                    curPlayerId == 0 ? JackpotManager.Instance.MyJetton : JackpotManager.Instance.TheJetton,
                        JackpotManager.Instance.AnteNub, !isFirstRaise);
                ShowSwitchoverButton(isRaise: true);
            }
            else if (GameManager.GameMode == GameMode.Online)
            {
                //     if (curPlayerId == 0)
                //     {
                //         // WaitPanel.gameObject.SetActive(false);//我猜可能应该这样写
                //         // ShowRaiseButton(GameManager.Instance.TheJetton);
                //     }
                //     else if (curPlayerId == 1)
                //     {
                //         // HideRaiseButton();
                //         // WaitPanel.gameObject.SetActive(true);
                //     }
            }
        }

        // 弃牌按钮的点击事件。
        private void FoldButtonClick()
        {
            HideRaisePanel();
            GameManager.Instance.OverOneHand(isWinerWaiver: curPlayerId != firstRaisePlayerId);
        }

        public void SetHint(string str)
        {
            GetControl<TextMeshProUGUI>("HintText").text = str;
            Debug.Log(GetControl<TextMeshProUGUI>("HintText"));
        }

        #endregion

        // public void GameStart(GameMode gameMode){//也许应该通过判断在线还是本地来调整一下ui排版
        //     if(gameMode == GameMode.Native){
        //         GetControl<Image>("shader")
        //     }
        // }


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
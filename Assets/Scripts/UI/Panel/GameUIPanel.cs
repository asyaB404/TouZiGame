// // ********************************************************************************************
// //     /\_/\                           @file       GameUIPanel.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111617
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************


using System;
using GamePlay.Core;
using NetWork.Client;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Panel
{
    public partial class GameUIPanel : BasePanel<GameUIPanel>
    {
        [SerializeField] private TextMeshProUGUI[] p1ScoreTexts;
        [SerializeField] private TextMeshProUGUI[] p2ScoreTexts;

        public override void Init()
        {
            base.Init();
            InitForOnline();
        }



        private void Awake()
        {
            SetButtonClick();
            SetConfirmButton();
        }

        public override void ShowAnim()
        {
            base.ShowAnim();
            ShowForOnline();
        }

        public override void HideAnim()
        {
            base.HideAnim();
            HideRaisePanel();
            HideHandOverPanel(false);
            GameManager.Instance.ExitCameraAnim();
        }

        public override void OnPressedEsc()
        {
            MenuPanel.Instance.ShowMe();
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

        public override void CallBackWhenHeadPop(IBasePanel popPanel)
        {
            popPanel?.HideAnim();
            if (popPanel is not MenuPanel
               && popPanel is not SwitchoverPanel
                && popPanel is not CalculationCGPanel
                ) ShowAnim();
        }

        [FormerlySerializedAs("HandOverPanel")]
        [SerializeField]
        private GameObject handOverPanel; //结束一次发牌到收牌后的页面，用于确认双方玩家分数以确认赢家

        [FormerlySerializedAs("HandOverTexts")]
        [SerializeField]
        private TextMeshProUGUI[] handOverTexts; //显示玩家分数

        public void ShowHandOverPanel(string title, string score1, string score2)
        {
            handOverPanel.SetActive(true);
            handOverTexts[0].text = title;
            handOverTexts[1].text = score1;
            handOverTexts[2].text = score2;
            confirmButton.gameObject.SetActive(GameManager.GameMode != GameMode.Online);
        }

        public void HideHandOverPanel(bool flag = true)
        {
            handOverPanel.SetActive(false);

            if (flag) GameManager.Instance.NewHand();
        }

        [SerializeField] private Button confirmButton; //分数确认页面的关闭按钮

        //设置分数确认页面的关闭按钮的监听
        private void SetConfirmButton() => confirmButton.onClick.AddListener(() =>
        {
            HideHandOverPanel();

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
        public void UpdateStageUI(int handNub, int stageNub, int turnNub)
        {
            handNubText.text = $"第{handNub.ToString()}轮";
            stageNubText.text = $"第{stageNub.ToString()}次下注";
            roundNubText.text = $"第{turnNub}次落子";
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

        public void SetWaitPanel(bool flag)
        {
            waitPanel.gameObject.SetActive(flag);
            if (flag) HideRaisePanel();
            // Debug.LogError(flag);
        }

        /// <summary>
        /// 设置底牌大小UI
        /// </summary>
        /// <param name="anteNub"></param>
        public void UpdateAnteUI(int anteNub) =>
            anteText.text = anteNub.ToString();

        /// <summary>
        /// 更新奖池UI
        /// </summary>
        /// <param name="sumJackpotNub"></param>
        public void UpdateJackpotUI(int sumJackpotNub) =>
            jackpotText.text = sumJackpotNub.ToString();


        [SerializeField] private TextMeshProUGUI callText;

        //打开加注页面
        public void ShowRaisePanel(bool isP1, int haveJackpot, int needJackpot, bool canFold = true,
            GameMode gameMode = GameMode.Native)
        {
            if (callText == null) callText = callButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            SetRaiseButtons(isP1, haveJackpot, needJackpot, canFold);
            if (gameMode == GameMode.Native) buttonPanel.gameObject.SetActive(true);
        }

        /// <summary>
        /// 设置加注页面按钮
        /// </summary>
        /// <param name="isP1">是否是p1</param>
        /// <param name="haveJackpot">拥有的筹码</param>
        /// <param name="needJackpot">需要的筹码</param>
        /// <param name="canFold">是否可以弃权（第一回合不能弃权）</param>
        private void SetRaiseButtons(bool isP1, int haveJackpot, int needJackpot, bool canFold = true)
        {
            raisePanelTitleText.text = isP1 ? "p1的加注时间" : "p2的加注时间";

            callButton.gameObject.SetActive(haveJackpot != 0);
            raiseButton.gameObject.SetActive(haveJackpot > needJackpot);
            callText.text = haveJackpot > needJackpot ? "跟注" : "AllIn!!!!";

            foldButton.gameObject.SetActive(canFold);
        }

        //关闭加注页面
        public void HideRaisePanel()
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
            raiseButton.onClick.AddListener(RaiseButtonClick);
            foldButton.onClick.AddListener(FoldButtonClick);

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
            entryEnter.callback.AddListener((data) => { HintManager.Instance.SetEventHint(str); });
            EventTrigger.Entry entryExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit,
            };
            entryExit.callback.AddListener((data) => { HintManager.Instance.SetEventHint(""); });
            trigger.triggers.Add(entryEnter);
            trigger.triggers.Add(entryExit);
        }

        // 跟注按钮的点击事件。
        private void CallButtonClick()
        {
            switch (GameManager.GameMode)
            {
                case GameMode.Native:
                    GameManager.Instance.Call(false);
                    break;
                case GameMode.Online:
                    MyClient.Instance.CallRequest(false);
                    break;
                case GameMode.SoloWithAi:
                    GameManager.Instance.Call(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // 加注按钮的点击事件。
        private void RaiseButtonClick()
        {
            switch (GameManager.GameMode)
            {
                case GameMode.Native:
                    GameManager.Instance.Call(true);
                    break;
                case GameMode.Online:
                    MyClient.Instance.CallRequest(true);
                    break;
                case GameMode.SoloWithAi:
                    GameManager.Instance.Call(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        // 弃牌按钮的点击事件。
        private void FoldButtonClick()
        {
            switch (GameManager.GameMode)
            {
                case GameMode.Native:
                    GameManager.Instance.Fold();
                    break;
                case GameMode.Online:
                    MyClient.Instance.FoldRequest();
                    break;
                case GameMode.SoloWithAi:
                    GameManager.Instance.Fold();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            HideRaisePanel();
        }

        public void UpdateDownHint(string str)
        {
            GetControl<TextMeshProUGUI>("HintTextDown").text = str;
        }
        public void UpdateUpHint(string str)
        {
            GetControl<TextMeshProUGUI>("HintTextUp").text = str;
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
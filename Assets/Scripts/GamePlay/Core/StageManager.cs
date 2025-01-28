using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using UI.Panel;
using UnityEngine;

namespace GamePlay.Core
{
    public enum GameStage
    {
        Idle,
        Raise,//加注阶段
        Place,//放牌阶段
        BlankScreen,//黑屏切换游戏玩家，单机双人使用
        Calculation,//结算阶段
    }

    public class StageManager
    {
        public static StageManager Instance
        {
            get { return _instance ??= new StageManager(); }
        }

        private static StageManager _instance;
        public int HandNub { get; private set; } = -1;// 游戏中完成一次一方获胜而筹码增加的过程；此处代表经过了几次“hand”

        public static int Round { get; private set; } = 0; //回合数（敌方放一次我放一次为一个回合//?这个什么时候变成静态的了？
        public static int Stage { get; private set; } = 0; //阶段数（决定是否加注的时候为一个阶段
        public int firstPlayerId = -1; //这个hand内先手玩家的id

        public static GameStage CurGameStage { get; private set; }
        public static void SetStage(GameStage stage)
        {
            CurGameStage = stage;
            HintManager.Instance.SetHint0(stage.ToString());
        }
        public static int LoseID =>
            GameManager.Instance.NodeQueueManagers[0].SumScore >
            GameManager.Instance.NodeQueueManagers[1].SumScore
                ? 1
                : 0;
        public void NewGame()
        {
            HandNub = 0;
            Stage = 0;
            Round = 0;
            if (GameManager.GameMode == GameMode.Native)//本地模式默认起手玩家为p1
            {

                firstPlayerId = 0;
            }
            Debug.Log($"1^firstPlayerId:{1^firstPlayerId}");
            Debug.Log($"firstPlayerId:{firstPlayerId}");
            JackpotManager.Instance.EnterRaise(1^firstPlayerId, false);
            

            SetText();
        }
        public void NewHand()
        {
            firstPlayerId = (firstPlayerId + 1) % 2;
            Debug.Log($"firstPlayerId:{firstPlayerId}");
            Debug.Log($"1^firstPlayerId:{1^firstPlayerId}");
            JackpotManager.Instance.EnterRaise(1^firstPlayerId, false);

            HandNub++;
            Round = 0;
            Stage = 0;
            SetText();
        }
        public void NewStage()
        {
            if (GameManager.GameMode == GameMode.Native)
            {
                ShowBlankScreen();
            }
            SetStage(GameStage.Place);
            Round = 0;
            SetText();

        }
        /// <summary>
        /// 返回是否进入下一个阶段
        /// </summary>
        /// <param name="nowPlayerId">下个回合玩家的id，会在内部确认是否进入了新的回合</param>
        public bool NewRound(int nowPlayerId)
        {

            if (GameManager.GameMode == GameMode.Native)
            {
                ShowBlankScreen();
            }
            if (nowPlayerId != firstPlayerId) return false;
            Round++;
            if (Round == MyGlobal.A_STAGE_ROUND - 1)
            {
                HintManager.Instance.SetHint1("EndPlace");
            }
            if (Round >= MyGlobal.A_STAGE_ROUND)
            {
                // GameUIPanel.Instance.ShowRaise(LoseID==0);
                JackpotManager.Instance.EnterRaise(LoseID);
                Stage++;
                return true;
            }

            SetText();
            return false;
        }
        int blankId = -1;
        public void ShowBlankScreen()
        {

            bool isRaise = CurGameStage == GameStage.Raise;
            int playerId=isRaise?JackpotManager.Instance.curPlayerId:GameManager.CurPlayerId;
            Debug.Log($"playerId:{playerId},blankId:{blankId}");
            if (blankId == playerId) return;

            blankId = playerId;
            string str;
            if (playerId == 0) str = "P1";
            else str = "P2";
            if (isRaise) str += "加注";
            else str += "回合";
            GameManager.Instance.HoleCardManagers[0].ShowShader();
            GameManager.Instance.HoleCardManagers[1].ShowShader();
            GameUIPanel.Instance.ShowSwitchoverButton(str, isRaise);
            HintManager.Instance.SetHint1("BlankScreen");
            Debug.Log("打开黑幕");
        }
        public void HideBlankScreen()
        {

            GameManager.Instance.HoleCardManagers[blankId].HideShader();
        }
        public void HideBlankScreen(int playerId)
        {
            GameManager.Instance.HoleCardManagers[playerId].HideShader();
        }

        private void SetText()
        {
            // Debug.Log($"handNub:{handNub},round:{Round},stage:{Stage}");
            GameUIPanel.Instance.SetStageNub(handNub: HandNub + 1, roundNub: Round + 1, stageNub: Stage + 1);
        }

        public void Reset()
        {
            CurGameStage = GameStage.Idle;
        }
    }
}
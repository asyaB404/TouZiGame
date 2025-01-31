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
        Raise, //加注阶段
        Place, //放牌阶段
        BlankScreen, //黑屏切换游戏玩家，单机双人使用  
        Calculation, //结算阶段
    }

    public class StageManager
    {
        public static int Hand { get; private set; } = 0; // 游戏中完成一次一方获胜而筹码增加的过程；此处代表经过了几次“hand”

        public static int Stage { get; private set; } = 1; //阶段数（决定是否加注的时候为一个阶段
        public static int Turn { get; private set; } = 1;
        public static int FirstPlayerId { get; private set; } = -1; //当前回合先手玩家

        public static GameStage CurGameStage { get; private set; }

        public static void SetStage(GameStage stage)
        {
            CurGameStage = stage;
            HintManager.Instance.SetHint0(stage.ToString());
        }


        public void NewGame()
        {
            FirstPlayerId = GameManager.CurPlayerId;
            UpdateUI();
        }

        public void NewHand()
        {
            SetStage(GameStage.Place);
            FirstPlayerId = (FirstPlayerId + 1) % MyGlobal.MAX_PLAYER_COUNT;
            Hand++;
            Turn = 1;
            Stage = 1;
            UpdateUI();
        }

        /// <summary>
        /// 所有玩家下注完毕，进行新的阶段
        /// </summary>
        public void NewStage()
        {
            SetStage(GameStage.Place);
            if (GameManager.GameMode == GameMode.Native)
            {
                ShowBlankScreen();
            }

            Turn = 1;
            UpdateUI();
        }

        /// <summary>
        /// 尝试进入下一Round，返回是否进入下一Round
        /// </summary>
        public bool TryNextRound()
        {
            Turn++;
            if (Turn >= MyGlobal.A_STAGE_ROUND - 2)
            {
                HintManager.Instance.SetHint1("EndPlace");
            }

            if (Turn > MyGlobal.A_STAGE_ROUND)
            {
                Stage++;
                return true;
            }
            UpdateUI();
            return false;
        }

        int blankId = -1;

        public void ShowBlankScreen()
        {
            bool isRaise = CurGameStage == GameStage.Raise;
            int playerId = GameManager.CurPlayerId;
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
            // Debug.Log("打开黑幕");
        }

        public void HideBlankScreen()
        {
            GameManager.Instance.HoleCardManagers[blankId].HideShader();
        }

        public void HideBlankScreen(int playerId)
        {
            GameManager.Instance.HoleCardManagers[playerId].HideShader();
        }

        private void UpdateUI()
        {
            GameUIPanel.Instance.UpdateStageUI(handNub: Hand, roundNub: (int)(Turn+1)/2, stageNub: Stage);
        }

        public void Reset()
        {
            Hand = 1;
            Stage = 1;
            Turn = 1;
            CurGameStage = GameStage.Idle;
            UpdateUI();
        }
    }
}
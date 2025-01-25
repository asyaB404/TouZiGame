using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using UI.Panel;
using UnityEngine;

namespace GamePlay.Core
{
    public class StageManager
    {
        public static StageManager Instance
        {
            get { return _instance ??= new StageManager(); }
        }

        private static StageManager _instance;
        public int HandNub { get; private set; } = -1;// 游戏中完成一次一方获胜而筹码增加的过程；此处代表经过了几次“hand”

        public static int Round { get; private set; } = 0; //回合数（敌方放一次我放一次为一个回合
        public static int Stage { get; private set; } = 0; //阶段数（决定是否加注的时候为一个阶段
        public int firstPlayerId = -1; //这个hand内先手玩家的id



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
                
                firstPlayerId=0;
            }
            JackpotManager.Instance.EnterRaise(~firstPlayerId,false);
            SetText();
        }
        public void NewHand()
        {
            firstPlayerId = (firstPlayerId + 1) % 2;
            JackpotManager.Instance.EnterRaise(~firstPlayerId,false);
            HandNub++;
            Round = 0;
            Stage = 0;
            SetText();
        }
        public void NewStage()
        {
            
            Round = 0;
            SetText();
        }
        /// <summary>
        /// 返回是否进入下一个阶段
        /// </summary>
        /// <param name="nowPlayerId">下个回合玩家的id，会在内部确认是否进入了新的回合</param>
        public bool NewRound(int nowPlayerId)
        {
            if (nowPlayerId != firstPlayerId) return false;
            Round++;
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



        private void SetText()
        {
            Debug.Log($"handNub:{HandNub},round:{Round},stage:{Stage}");
            GameUIPanel.Instance.SetStageNub(handNub: HandNub + 1, roundNub: Round + 1, stageNub: Stage+1 );
        }
    }
}
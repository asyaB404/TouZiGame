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
            get { return instance ??= new StageManager(); }
        }

        private static StageManager instance;

        //TODO开放性有待商榷
        public int handNub { get; private set; } //Hand 主要用于扑克等卡牌游戏，表示从发牌到结束的一局，即游戏中完成一次一方获胜而筹码增加的过程；此处代表经过了几次“hand”
        public int round { get; private set; } //回合数（敌方放一次我放一次为一个回合
        public int stage { get; private set; } //阶段数（决定是否加注的时候为一个阶段
        public int firstPlayerId; //这个hand内先手玩家的id

        public void NewHand(int newPlayerId)
        {
            firstPlayerId = newPlayerId;
            handNub++;
            round = 0;
            stage = 0;
            SetText();
        }

        /// <summary>
        /// 返回是否进入下一个阶段
        /// </summary>
        /// <param name="nowPlayerId">下个回合玩家的id，会在内部确认是否进入了新的回合</param>
        public bool NewRound(int nowPlayerId)
        {
            if (nowPlayerId != firstPlayerId) return false;
            round++;
            SetText();
            if (round >= MyGlobal.A_STAGE_ROUND)
            {
                GameUIPanel.Instance.SetRaiseButton(true);
                return true;
                //进入下一个阶段
            }

            return false;
        }

        public void NewStage()
        {
            stage++;
            round = 0;
            SetText();
        }

        public void SetText()
        {
            GameUIPanel.Instance.SetNub(handNub: handNub + 1, roundNub: round + 1, stageNub: stage + 1);
        }
    }
}
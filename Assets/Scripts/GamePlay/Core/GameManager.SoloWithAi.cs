using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UI.Panel;
using UnityEngine;
namespace GamePlay.Core
{
    public partial class GameManager
    {
        public void StartSoloWaitAiGame(int seed)
        {
            holeCardManagers[1].gameObject.SetActive(false);
            GameMode = GameMode.SoloWithAi;
            gameObject.SetActive(true);
            Random.InitState(seed);

            _jackpotManager.NewGame();
            _stageManager.NewGame();

            holeCardManagers[0].ResetAllHoleCards();
            holeCardManagers[1].ResetAllHoleCards();

            GameUIPanel.Instance.ShowMe();

            _jackpotManager.EnterRaise();
            Debug.Log("start");
        }

        private void AiCall()
        {
            int time = (int)(1000 * Random.Range(MyGlobal.MIN_AI_PONDER_Time, MyGlobal.MAX_AI_PONDER_Time));
            int isRaise = Random.Range(0, 3);//TODO
            UniTask.Create(async () =>
            {
                await UniTask.Delay(time);
                if (isRaise == 0) Call(false);
                else if (isRaise == 1) Call(true);
                else if (isRaise == 2) Fold();
            }).Forget();
        }
        private void AiAddTouZi()
        {
            int time = (int)(1000 * Random.Range(MyGlobal.MIN_AI_PONDER_Time, MyGlobal.MAX_AI_PONDER_Time));
            int maxValue=0,maxClear,nodeNub=0;
            for (int i = 0; i < nodeQueueManagers[1].NodeQueues.Count; i++)
            {
                // if(nodeQueueManagers[1].NodeQueues[i])
                //TODO
            }
            UniTask.Create(async () =>
            {
                await UniTask.Delay(time);

            }).Forget();
        }
    }

}
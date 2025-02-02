using System.Collections;
using System.Collections.Generic;
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
    }
}
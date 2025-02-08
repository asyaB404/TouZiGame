using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            holeCardManagers[0].HideShader();

            GameMode = GameMode.SoloWithAi;
            gameObject.SetActive(true);
            Random.InitState(seed);

            _jackpotManager.NewGame();
            _stageManager.NewGame();

            holeCardManagers[0].ResetAllHoleCards();
            holeCardManagers[1].ResetAllHoleCards();

            GameUIPanel.Instance.ShowMe();

            _jackpotManager.EnterRaise();
            // Debug.Log("start");
            ChessboardOpen();
        }

        public void AiCall()
        {
            if (curPlayerId != 1) return;
            int time = (int)(1000 * Random.Range(MyGlobal.MIN_AI_PONDER_Time, MyGlobal.MAX_AI_PONDER_Time));
            List<int> ints = new();
            if (StageManager.Stage != 1)ints.Add(2);

            if (_jackpotManager.JackpotP2>_jackpotManager.AnteNub) ints.Add(1); 
            if(_jackpotManager.JackpotP2>0) ints.Add(0);
            if(ints.Count==0) {
                Debug.LogError("Error");
                return;
            }
            int isRaise = ints[Random.Range(0, ints.Count)];//TODO
            // isRaise=2;
            UniTask.Create(async () =>
            {
                await UniTask.Delay(time);
                if (curPlayerId != 1) return;
                if (isRaise == 0) Call(false);
                else if (isRaise == 1) Call(true);
                else if (isRaise == 2) Fold();
            }).Forget();
        }
        public void AiAddTouZi()
        {
            if (curPlayerId != 1) return;
            int time = (int)(1000 * Random.Range(MyGlobal.MIN_AI_PONDER_Time, MyGlobal.MAX_AI_PONDER_Time));
            // 初始化变量，用于存储最大价值、最大清除分数和底牌编号和棋盘位置
            int maxValue = 0, maxClear = 0, handNub = 0, nodeNub = 0;

            // 遍历每一列（NodeQueues）
            for (int i = 0; i < nodeQueueManagers[1].NodeQueues.Count; i++)
            {
                // Debug.Log($"i:{i}");
                var currentQueue = nodeQueueManagers[1].NodeQueues[i];//获得当前列
                // 如果当前列已经满了（有3个骰子），跳过这一列
                if (currentQueue.Scores.Count == 3) continue;
                // 初始化一个数组，用于记录每个骰子值（0-6）的出现次数
                int[] diceCounts = new int[7];
                // 初始化基础分数，用于计算当前列的分数
                foreach (var score in currentQueue.Scores)
                {
                    diceCounts[score]++; // 记录每个点数出现的次数
                }

                // 遍历底牌（HoleCards）
                for (int t = 0; t < 3; t++)
                {
                    // Debug.Log($"t:{t}");
                    var holeCard = HoleCardManagers[1].HoleCards[t];
                    // 复制当前骰子计数，避免修改原始数组
                    int[] tempDiceCounts = (int[])diceCounts.Clone();
                    // 将当前底牌的骰子值加入统计
                    tempDiceCounts[holeCard.TouZiScore]++;

                    // 计算新的分数
                    int newValue = 0;
                    for (int j = 0; j < tempDiceCounts.Count(); j++)
                    {
                        newValue += j * tempDiceCounts[j] * tempDiceCounts[j];
                    }
                    // 减去当前列的基础分数，得到净增分数
                    newValue -= currentQueue.SumScore;

                    // 计算对面列（NodeQueues[0]）被扣除的分数
                    int nub = 0;
                    foreach (var score in nodeQueueManagers[0].NodeQueues[i].Scores)
                    {
                        if (score == holeCard.TouZiScore)
                        {
                            nub++; // 统计对面列中与当前底牌骰子值相同的次数
                        }
                    }
                    // 计算扣除的分数
                    int clear = holeCard.TouZiScore * nub * nub;
                    // Debug.Log($"handNub:{t},nodeNub:{i},clear:{clear},nub:{nub},newValue:{newValue}");
                    // 如果当前组合的分数（newValue + clear）比之前的最大值更大，更新最大值
                    if (newValue + clear > maxValue + maxClear)
                    {
                        maxValue = newValue;
                        maxClear = clear;
                        handNub = t; // 记录当前底牌的索引
                        nodeNub = i; // 记录当前列的索引
                    }
                }
            }
            holeCardManagers[1].CurIndex = handNub;
            UniTask.Create(async () =>
            {
                await UniTask.Delay(time);
                if (curPlayerId != 1) return;
                AddTouzi(nodeNub);
            }).Forget();
        }
    }

}
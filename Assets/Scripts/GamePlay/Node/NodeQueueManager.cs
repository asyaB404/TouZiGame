// // ********************************************************************************************
// //     /\_/\                           @file       NodeQueueManager.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111820
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************


using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GamePlay.Node
{
    public class NodeQueueManager : MonoBehaviour
    {
        public int playerId = -1;
        [SerializeField] private NodeQueue[] nodeQueues;
        public const int MAX_QUEUE_NODE = 3;
        public int SumScore => NodeQueues.Sum(nodeQueue => nodeQueue.SumScore);

        public IReadOnlyList<NodeQueue> NodeQueues => nodeQueues;

        public void Init()
        {
            for (int i = 0; i < nodeQueues.Length; i++)
            {
                nodeQueues[i].id = i;
                nodeQueues[i].playerId = playerId;
            }
        }

        public bool AddTouzi(int id, int score)
        {
            return NodeQueues[id].AddNode(score);
        }

        public bool RemoveTouzi(int id, int score)
        {
            return NodeQueues[id].RemoveNode(score);
        }

        public void Clear()
        {
            foreach (var nodeQueue in NodeQueues)
            {
                nodeQueue.Clear();
            }
        }

        public bool CheckIsGameOver()
        {
            foreach (var nodeQueue in NodeQueues)
            {
                if (nodeQueue.Scores.Count < MAX_QUEUE_NODE) return false;
            }
            return true;
        }
    }
}
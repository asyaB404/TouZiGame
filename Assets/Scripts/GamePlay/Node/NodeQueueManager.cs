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
        [SerializeField] private int playerId = -1;

        [SerializeField] private NodeQueue[] nodeQueues;
        public int PlayerId => playerId;

        //一列最多存多少个Node
        public const int MAX_QUEUE_NODE = 3;
        public int SumScore => NodeQueues.Sum(nodeQueue => nodeQueue.SumScore);

        public IReadOnlyList<NodeQueue> NodeQueues => nodeQueues;

        public void Init(int id)
        {
            playerId = id;
            for (int i = 0; i < nodeQueues.Length; i++)
            {
                nodeQueues[i].id = i;
                nodeQueues[i].playerId = id;
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

        public void Reset()
        {
            foreach (var nodeQueue in NodeQueues)
            {
                nodeQueue.Reset();
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
// // ********************************************************************************************
// //     /\_/\                           @file       NodeQueueManager.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111820
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using System;
using UnityEngine;

namespace GamePlay.Node
{
    public class NodeQueueManager : MonoBehaviour
    {
        [SerializeField] private NodeQueue[] nodeQueues;
        public const int MaxNode = 3;

        private void Awake()
        {
            for (int i = 0; i < nodeQueues.Length; i++)
            {
                nodeQueues[i].id = i;
            }
        }

        public bool AddTouzi(int id, int score)
        {
            return nodeQueues[id].AddNode(score);
        }

        public bool RemoveTouzi(int id, int score)
        {
            return nodeQueues[id].RemoveNode(score);
        }

        private bool CheckIsGameOver()
        {
            foreach (var nodeQueue in nodeQueues)
            {
                if (nodeQueue.Scores.Count < MaxNode) return false;
            }

            return true;
        }
    }
}
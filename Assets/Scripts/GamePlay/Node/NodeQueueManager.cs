// // ********************************************************************************************
// //     /\_/\                           @file       NodeQueueManager.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111820
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using UnityEngine;

namespace GamePlay.Node
{
    public class NodeQueueManager : MonoBehaviour
    {
        [SerializeField] private NodeQueue[] nodeQueues;
        public const int maxNode = 3;
        public int MaxNode => maxNode;

        private bool CheckIsGameOver()
        {
            foreach (var nodeQueue in nodeQueues)
            {
                if (nodeQueue.Scores.Count < maxNode) return false;
            }
            return true;
        }
    }
}
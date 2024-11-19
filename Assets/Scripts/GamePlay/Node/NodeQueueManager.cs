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
        public const int MaxNode = 3;

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
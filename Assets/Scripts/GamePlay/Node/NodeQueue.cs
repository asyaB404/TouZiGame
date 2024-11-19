// // ********************************************************************************************
// //     /\_/\                           @file       NodeQueue.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111619
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.Node
{
    public class NodeQueue : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerExitHandler
    {
        public int id = -1;
        private const float HoverScaleFactor = 1.1f;
        private static int MaxNode => NodeQueueManager.MaxNode;
        public int SumScore { get; private set; }
        [SerializeField] private Vector3 initialScale;
        [SerializeField] private Transform[] nodePos;
        [SerializeField] private List<int> scores;
        public IReadOnlyList<int> Scores => scores;
        private readonly Dictionary<int, int> _scoreCounts = new();

        public void OnPointerEnter(PointerEventData data)
        {
            transform.DOKill();
            transform.DOScale(initialScale * HoverScaleFactor, 0.3f);
        }

        public void OnPointerExit(PointerEventData data)
        {
            transform.DOKill();
            transform.DOScale(initialScale, 0.3f);
        }

        public void OnPointerDown(PointerEventData data)
        {
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (data.pointerCurrentRaycast.gameObject != gameObject)
                return;
            Debug.Log("Pointer clicked and released on the same component.");
        }

        public bool AddNode(int score)
        {
            if (scores.Count == MaxNode) return false;
            scores.Add(score);
            if (!_scoreCounts.TryAdd(score, 1))
            {
                _scoreCounts[score]++;
            }

            return true;
        }

        public bool RemoveNode(int index)
        {
            if (scores.Count == 0 || index < 0 || index >= MaxNode) return false;
            int score = scores[index];
            if (_scoreCounts.TryGetValue(score, out int count))
            {
                if (count == 1) _scoreCounts.Remove(score);
            }
            else
            {
                _scoreCounts[score]--;
            }

            scores.RemoveAt(index);
            return true;
        }

        private void UpdateSumScore()
        {
            SumScore = 0;
            foreach (var kv in _scoreCounts)
            {
                int score = kv.Key;
                int count = kv.Value;
                if (count >= 2)
                {
                    SumScore += score * count * count;
                }
                else
                {
                    SumScore += score;
                }
            }
        }
    }
}
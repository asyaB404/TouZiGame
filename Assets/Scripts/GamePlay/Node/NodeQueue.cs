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
        private const int MaxNode = 3;
        public int SumScore { get; private set; }
        [SerializeField] private Vector3 initialScale;
        [SerializeField] private Transform[] nodePos;
        [SerializeField] private int[] scores;
        private int _lastIndex = 0;

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
            if (_lastIndex == MaxNode) return false;
            scores[_lastIndex] = score;
            _lastIndex++;
            return true;
        }

        private void UpdateSumScore()
        {
            
        }
    }
}
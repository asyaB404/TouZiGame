// // ********************************************************************************************
// //     /\_/\                           @file       NodeQueue.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2024111619
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GamePlay.Node
{
    public class NodeQueue : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler,
        IPointerExitHandler
    {
        private const float HoverScaleFactor = 1.2f;
        private const float MaxClickDuration = 0.5f;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private float initialScale;
        private float _mouseDownTime;

        public void OnPointerEnter(PointerEventData data)
        {
            if (sr == null) return;
            sr.transform.DOKill();
            sr.transform.DOScale(initialScale * HoverScaleFactor, 0.3f);
        }

        public void OnPointerExit(PointerEventData data)
        {
            if (sr == null) return;
            sr.transform.DOKill();
            sr.transform.DOScale(initialScale, 0.3f);
        }

        public void OnPointerDown(PointerEventData data)
        {
            _mouseDownTime = Time.time;
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (Time.time - _mouseDownTime < MaxClickDuration)
            {
                
            }
        }
    }
}
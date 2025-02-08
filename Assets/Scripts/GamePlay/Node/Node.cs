using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Node;
using UnityEngine;
using UnityEngine.EventSystems;
namespace GamePlay.Node
{
    public class Node : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler,
            IPointerExitHandler
    {
        private NodeQueue nodeQueue;
        private Vector3 MyScale;
        public void Init(NodeQueue nodeQueue)
        {
            MyScale = transform.localScale;
            this.nodeQueue = nodeQueue;
        }
        public void SetScale( float multiple,float time)
        {
            transform.DOScale(MyScale * multiple, time);
        }
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            nodeQueue.OnPointerEnter(eventData);
            Debug.Log("OnPointerEnter");
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            nodeQueue.OnPointerExit(eventData);
            Debug.Log("OnPointerExit");
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            nodeQueue.OnPointerDown(eventData);
        }



        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            nodeQueue.OnPointerUp(eventData);
        }
    }
}
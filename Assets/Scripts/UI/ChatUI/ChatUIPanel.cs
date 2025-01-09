/********************************************************************
    Author:			Basyyya
    Date:			2025:1:9 15:07
    Description:	使用Fishnet广播基础的简易聊天面板
*********************************************************************/

using DG.Tweening;
using FishNet;
using FishNet.Connection;
using FishNet.Transporting;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.ChatUI
{
    public class ChatUIPanel : MonoBehaviour
    {
        [SerializeField] private GameObject messagePrefab;
        [SerializeField] private RectTransform content;
        [SerializeField] private Button switchBtn;
        [SerializeField] private TMP_InputField messageInput;

        public void SendChatMessage(string sender, string text)
        {
            var chatMessage = new ChatMessage
            {
                Sender = sender,
                Message = text
            };
            if (InstanceFinder.IsServerStarted)
                InstanceFinder.ServerManager.Broadcast(chatMessage);
            else if (InstanceFinder.IsClientStarted)
                InstanceFinder.ClientManager.Broadcast(chatMessage);
        }

        private void Awake()
        {
            messageInput.onSubmit.AddListener(OnInputSubmit);
            switchBtn.onClick.AddListener(OnSwitchBtnClick);
            messageInput.onValidateInput += ValidateInput;
        }

        private void OnSwitchBtnClick()
        {
            float x = transform.localPosition.x;
            if (x >= 0)
            {
                transform.DOLocalMoveX(-((RectTransform)transform).sizeDelta.x, 0.1f);
            }
            else
            {
                transform.DOLocalMoveX(0, 0.1f);
            }
        }

        private void OnInputSubmit(string message)
        {
            SendChatMessage("NoName", messageInput.text);
            messageInput.text = null;
            EventSystem.current.SetSelectedGameObject(messageInput.gameObject, null);
            messageInput.OnPointerClick(new PointerEventData(EventSystem.current));
        }

        private char ValidateInput(string text, int charIndex, char addedChar)
        {
            // 忽略首个Enter键的输入
            if (charIndex == 0 && addedChar is '\n' or '\r') return '\0'; //返回空字符表示忽略该输入
            return addedChar; //返回输入的字符
        }

        private void OnEnable()
        {
            InstanceFinder.ClientManager.RegisterBroadcast<ChatMessage>(OnClientChatMessageReceived);
            InstanceFinder.ServerManager.RegisterBroadcast<ChatMessage>(OnServerChatMessageReceived);
        }

        private void OnDisable()
        {
            InstanceFinder.ClientManager.UnregisterBroadcast<ChatMessage>(OnClientChatMessageReceived);
            InstanceFinder.ServerManager.UnregisterBroadcast<ChatMessage>(OnServerChatMessageReceived);
        }

        private void OnClientChatMessageReceived(ChatMessage chatMessage, Channel channel)
        {
            SpawnMsg(chatMessage);
        }

        private void OnServerChatMessageReceived(
            NetworkConnection networkConnection,
            ChatMessage chatMessage,
            Channel channel)
        {
            InstanceFinder.ServerManager.Broadcast(chatMessage);
        }

        private void SpawnMsg(ChatMessage chatMessage)
        {
            // if (string.IsNullOrEmpty(chatMessage.Message)) return;
            // var newMessage = Instantiate(messagePrefab, content, false);
            // Vector2 pos = newMessage.transform.localPosition;
            // pos.y = -_totalLineCount * lineHeight;
            // newMessage.transform.localPosition = pos;
            // newMessage.transform.localScale = Vector3.zero;
            // newMessage.transform.DOScale(1, 0.5f);
            // _totalLineCount += newMessage.GetComponent<Message>().Init(chatMessage.Sender, chatMessage.Message) + 1;
            // var size = content.sizeDelta;
            // size.y = lineHeight * (_totalLineCount + 2);
            // content.sizeDelta = size;
            // if (size.y >= 700)
            //     content.localPosition = new Vector3(0, size.y - 700);
        }
    }
}
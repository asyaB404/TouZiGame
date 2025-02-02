// // ********************************************************************************************
// //     /\_/\                           @file       ChatPanel.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \                          @Description 单独的聊天面板
// //   (       )                         @Modified   202481820
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using DG.Tweening;
using FishNet;
using FishNet.Broadcast;
using FishNet.Connection;
using FishNet.Transporting;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChatUI
{
    public struct ChatMessage : IBroadcast
    {
        public string Sender;
        public string Message;

        public ChatMessage(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }

    public class ChatPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private TMP_InputField messageInput;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private GameObject messagePrefab;
        [SerializeField] private int lineHeight = 40;
        [SerializeField] private Button[] buttons;
        private int _totalLineCount;

        #region Private

        private void Awake()
        {
            buttons = GetComponentsInChildren<Button>(true);
        }

        private void OnEnable()
        {
            buttons[0].onClick.AddListener(SendInputField);
            buttons[1].onClick.AddListener(OnSwitchBtnClick);
            messageInput.onSubmit.AddListener(OnInputSubmit);
            messageInput.onValidateInput += ValidateInput;
            InstanceFinder.ClientManager.RegisterBroadcast<ChatMessage>(OnClientChatMessageReceived);
            InstanceFinder.ServerManager.RegisterBroadcast<ChatMessage>(OnServerChatMessageReceived);
            // InstanceFinder.ServerManager.OnRemoteConnectionState += OnRemoteConnection;
            // InstanceFinder.ServerManager.OnAuthenticationResult += OnAuthentication;
        }

        private void OnDisable()
        {
            Clear();
            buttons[0].onClick.RemoveListener(SendInputField);
            buttons[1].onClick.RemoveListener(OnSwitchBtnClick);
            messageInput.onSubmit.RemoveListener(OnInputSubmit);
            messageInput.onValidateInput -= ValidateInput;
            InstanceFinder.ClientManager.UnregisterBroadcast<ChatMessage>(OnClientChatMessageReceived);
            InstanceFinder.ServerManager.UnregisterBroadcast<ChatMessage>(OnServerChatMessageReceived);
            if (InstanceFinder.ServerManager == null) return;
            // InstanceFinder.ServerManager.OnRemoteConnectionState -= OnRemoteConnection;
            // InstanceFinder.ServerManager.OnAuthenticationResult -= OnAuthentication;
        }

        private void OnSwitchBtnClick()
        {
            var rectTransform = (RectTransform)transform;
            var x = rectTransform.anchoredPosition.x;
            if (x >= 0)
            {
                rectTransform.DOAnchorPosX(-rectTransform.sizeDelta.x, 0.1f);
            }
            else
            {
                rectTransform.DOAnchorPosX(0, 0.1f);
            }
        }

        private char ValidateInput(string text, int charIndex, char addedChar)
        {
            // 忽略首个Enter键的输入
            if (charIndex == 0 && addedChar is '\n' or '\r') return '\0'; //返回空字符表示忽略该输入

            return addedChar; //返回输入的字符
        }

        private void OnInputSubmit(string message)
        {
            SendInputField();
            EventSystem.current.SetSelectedGameObject(messageInput.gameObject, null);
            messageInput.OnPointerClick(new PointerEventData(EventSystem.current));
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
            if (string.IsNullOrEmpty(chatMessage.Message)) return;
            var newMessage = Instantiate(messagePrefab, content, false);
            Vector2 pos = newMessage.transform.localPosition;
            pos.y = -_totalLineCount * lineHeight;
            newMessage.transform.localPosition = pos;
            newMessage.transform.localScale = Vector3.zero;
            newMessage.transform.DOScale(1, 0.5f);
            _totalLineCount += newMessage.GetComponent<ChatMessageComponent>().Init(chatMessage.Sender, chatMessage.Message) + 1;
            var size = content.sizeDelta;
            size.y = lineHeight * (_totalLineCount + 2);
            content.sizeDelta = size;
            if (size.y >= 350)
                content.localPosition = new Vector3(0, size.y - 350);
        }

        private void SendInputField()
        {
            SendChatMessage("NoName:", messageInput.text);
            messageInput.text = null;
        }

        # endregion


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

        public void Clear()
        {
            content.DestroyAllChildren();
            Vector3 size = content.sizeDelta;
            size.y = 700;
            content.sizeDelta = size;
            content.localPosition = new Vector3(0, 0);
            _totalLineCount = 0;
        }

        // private void OnRemoteConnection(NetworkConnection connection, RemoteConnectionStateArgs obj)
        // {
        //     switch (obj.ConnectionState)
        //     {
        //         case RemoteConnectionState.Stopped:
        //             if (connection.IsAuthenticated)
        //             {
        //                 InstanceFinder.ServerManager.Broadcast(new ChatMessage("   ", "玩家" + connection + "离开了游戏"));
        //             }
        //
        //             break;
        //     }
        // }
        //
        // private void OnAuthentication(NetworkConnection connection, bool flag)
        // {
        //     if (flag)
        //     {
        //         InstanceFinder.ServerManager.Broadcast(new ChatMessage("   ", "玩家" + connection + "加入了游戏"));
        //     }
        // }
    }
}
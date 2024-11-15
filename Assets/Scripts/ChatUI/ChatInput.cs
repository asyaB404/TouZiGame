using UnityEngine;
using UnityEngine.EventSystems;

namespace ChatUI
{
    public class ChatInput : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        private ChatPanel _chatPanel;
        public bool IsSelected { get; private set; }

        private void Awake()
        {
            _chatPanel = GetComponentInParent<ChatPanel>();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            IsSelected = false;
            _chatPanel.HideChatPanel();
        }

        public void OnSelect(BaseEventData eventData)
        {
            IsSelected = true;
            _chatPanel.ShowChatPanel();
        }
    }
}
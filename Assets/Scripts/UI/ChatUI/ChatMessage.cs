using FishNet.Broadcast;

namespace UI.ChatUI
{
    public struct ChatMessage:IBroadcast
    {
        public string Sender;
        public string Message;

        public ChatMessage(string sender, string message)
        {
            Sender = sender;
            Message = message;
        }
    }
}
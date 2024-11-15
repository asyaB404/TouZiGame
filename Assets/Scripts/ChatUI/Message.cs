using Extension;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ChatUI
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI myname;

        [FormerlySerializedAs("text")] [SerializeField]
        private TextMeshProUGUI message;

        public int Init(string name, string text)
        {
            myname.text = name;
            message.text = text;
            return message.ReSetHeightFromText() + 1;
        }
    }
}
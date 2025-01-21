// // ********************************************************************************************
// //     /\_/\                           @file       ChatMessageComponent.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \                          @Description 单独的聊天消息组件
// //   (       )                         @Modified   202481820
// //   (___)___)                         @Copyright  Copyright (c) 2024, Basya
// // ********************************************************************************************

using Extension;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ChatUI
{
    public class ChatMessageComponent : MonoBehaviour
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
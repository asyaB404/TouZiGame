using TMPro;
using UnityEngine;

namespace Extension
{
    public static class ExtensionForUI
    {
        /// <summary>
        ///     以text组件的文本内容行数正确设置该组件的大小
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        /// <returns></returns>
        public static int ReSetHeightFromText(this TextMeshProUGUI textMeshProUGUI)
        {
            var line = textMeshProUGUI.GetTextInfo(textMeshProUGUI.text).lineCount;
            var height = textMeshProUGUI.rectTransform.rect.height;
            var sizeDelta = textMeshProUGUI.rectTransform.sizeDelta;
            sizeDelta.y = line * height;
            textMeshProUGUI.rectTransform.sizeDelta = sizeDelta;
            return line;
        }

        /// <summary>
        ///     以text组件的文本内容行数以及自己所给的单行高度来正确设置该组件的大小
        /// </summary>
        /// <param name="textMeshProUGUI"></param>
        /// <param name="height">单行高度</param>
        /// <returns></returns>
        public static int ReSetHeightFromText(this TextMeshProUGUI textMeshProUGUI, float height)
        {
            var line = textMeshProUGUI.GetTextInfo(textMeshProUGUI.text).lineCount;
            var sizeDelta = textMeshProUGUI.rectTransform.sizeDelta;
            sizeDelta.y = line * height;
            textMeshProUGUI.rectTransform.sizeDelta = sizeDelta;
            return line;
        }

        public static RectTransform[] GetAllChildRectTransforms(this RectTransform rectTransform)
        {
            var res = new RectTransform[rectTransform.childCount];
            for (var i = 0; i < rectTransform.childCount; i++) res[i] = (RectTransform)rectTransform.GetChild(i);

            return res;
        }

        public static void ResetSizeFromChilds(this RectTransform content, float duration = 0)
        {
            var size = content.sizeDelta;
            float y = 0;
            var childs = content.GetAllChildRectTransforms();
            foreach (var item in childs)
            {
                y += item.sizeDelta.y;
                y += duration;
            }

            y -= duration;
            size.y = y;
            content.sizeDelta = size;
        }

        /// <summary>
        ///     销毁一个transform下的所有子物体
        /// </summary>
        /// <param name="transform"></param>
        public static void DestroyAllChildren(this Transform transform)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
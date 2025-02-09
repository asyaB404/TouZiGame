/********************************************************************
    Author:			Basyyya
    Date:			2025:1:7 15:29
    Description:	工具类
*********************************************************************/

using DG.Tweening;
using UnityEngine;

public static class MyTool
{
    public static int GetNextPlayerId(int curPlayerId)
    {
        return (curPlayerId + 1) % MyGlobal.MAX_PLAYER_COUNT;
    }
    
    public static void PlayParabola(Transform target, Vector3 endPos, float height, float duration)
    {
        if (target == null) return;
        // XZ 轴线性移动
        target.DOMoveX(endPos.x, duration).SetEase(Ease.Linear);
        target.DOMoveZ(endPos.z, duration).SetEase(Ease.Linear);

        // Y 轴抛物线运动
        target.DOMoveY(target.position.y + height, duration / 2).SetEase(Ease.OutQuad)
            .OnComplete(() => target.DOMoveY(endPos.y, duration / 2).SetEase(Ease.InQuad));

        // 旋转动画（到达终点时绕 Z 轴旋转 360°）
        target.DORotate(new Vector3(0, 0, 360), duration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear);
    }
}
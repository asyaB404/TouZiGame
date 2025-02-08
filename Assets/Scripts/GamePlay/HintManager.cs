using System.Collections;
using System.Collections.Generic;
using UI.Panel;
using UnityEngine;

public class HintManager
{
    public static HintManager Instance
    {
        get
        {
            if (instance == null) instance = new HintManager();
            return instance;
        }
    }
    static HintManager instance;
    #region 下方提示栏，用于提示现在该做什么
    string hint0 = "";
    string hint1 = "";
    string hint2 = "";
    // string hintStr;


    private void SetHint()
    {
        if (hint2 != "") GameUIPanel.Instance.UpdateDownHint(HintText.HintTextEventDic[hint2]);
        else if (hint1 != "") GameUIPanel.Instance.UpdateDownHint(HintText.HintTextConditionDic[hint1]);
        else if (hint0 != "") GameUIPanel.Instance.UpdateDownHint(HintText.HintTextStageDic[hint0]);
    }
    public void SetStageHint(string str)//设置阶段性提示
    {
        hint0 = str;
        hint1 = "";
        hint2 = "";
        SetHint();
    }

    public void SetConditionHint(string str)//设置条件性提示
    {
        hint1 = str;
        hint2 = "";
        SetHint();
    }
    public void SetEventHint(string str)//设置事件性提示（如鼠标进入xxx
    {
        hint2 = str;
        SetHint();
    }
    #endregion

    #region 上方提示栏，用于提示刚刚做了什么
    public void SetUpHint(int playerId, string str)
    {
        string str1=playerId==0?"P1":"P2";
        string str2=playerId==0?"P2":"P1";
        string title=HintText.HintTextUpDic[str];
        title=title.Replace("玩家1",str1);
        title=title.Replace("玩家2",str2);
        GameUIPanel.Instance.UpdateUpHint(title);
    }
    #endregion
}

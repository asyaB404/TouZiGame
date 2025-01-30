using System.Collections;
using System.Collections.Generic;
using UI.Panel;
using UnityEngine;

public class HintManager
{
    string hint0="";
    string hint1="";
    string hint2="";
    // string hintStr;
    public static HintManager Instance
    {
        get
        {
            if (instance == null) instance = new HintManager();
            return instance;
        }
    }
    static HintManager instance;

    private void SetHint()
    {
        if (hint2 != "") GameUIPanel.Instance.UpdateHint(HintText.HintText2Dic[hint2]);
        else if (hint1 != "") GameUIPanel.Instance.UpdateHint(HintText.HintText1Dic[hint1]);
        else if (hint0 != "") GameUIPanel.Instance.UpdateHint(HintText.HintText0Dic[hint0]);
        
    }
    public void SetHint0(string str)
    {
        hint0 = str;
        hint1="";
        hint2="";
        SetHint();
    }

    public void SetHint1(string str)
    {
        hint1 = str;
        hint2="";
        SetHint();
    }
    public void SetHint2(string str)
    {
        hint2 = str;    
        SetHint();
    }
}

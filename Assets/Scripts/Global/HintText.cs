/********************************************************************
    Author:			kexin
    Date:			2025:1:26 18:22
    Description:	文本描述，用于做类似新手教程的东西
*********************************************************************/

using System.Collections.Generic;

public static class HintText
{
    public static readonly Dictionary<string, string> HintText0Dic = new Dictionary<string, string>()//阶段性提示\
    {
        {"",""},
        {"Raise","下注阶段：依据对自己胜利的信心进行下注"},
        {"Place","放置阶段：选择最有利的底牌放入棋盘中，也许将重要的牌在关键时候放置能逆转局势"},
        {"Calculation","结算阶段：依照双方的分数进行结算瓜分奖池"},
        // {"Raise",""},
    };
    public static readonly Dictionary<string, string> HintText1Dic = new Dictionary<string, string>()//阶段性提示
    {
        {"",""},
        {"FirstRaise", "后手玩家(P2)进行第一次加注"},
        { "EndPlace","最后一个放置回合，回合结束后进行结算"},
        // { "",""},
        // { "",""},
    };
public static readonly Dictionary<string, string> HintText2Dic = new Dictionary<string, string>()//瞬时性提示
    {
        {"",""},
        {"RaiseButton", "给底注增加1，在此基础上付出底注的筹码"},
        {"CallButton", "付出与底注相同的筹码"},
        {"FoldButton", "输家弃牌赢家获得所有奖池，赢家弃牌与输家平分奖池"},
        {"HoleCardChoose","选择准备使用的底牌"},
        {"NodeQueue","选择底牌放置的位置，当敌方同排有与你底牌相同的骰子将会被消除，同一排相同的骰子越多，获得的分数越多"},

    };
}
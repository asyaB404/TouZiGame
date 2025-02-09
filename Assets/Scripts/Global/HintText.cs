/********************************************************************
    Author:			kexin
    Date:			2025:1:26 18:22
    Description:	文本描述，用于做类似新手教程的东西
*********************************************************************/

using System.Collections.Generic;

public static class HintText
{
    public static readonly Dictionary<string, string> HintTextStageDic = new Dictionary<string, string>()//阶段性提示\
    {
        {"",""},
        {"Idle","闲置阶段：等待双方玩家加入并准备后，由房主开始游戏"},
        {"Raise","下注阶段：依据对自己胜利的信心进行下注"},
        {"Place","放置阶段：选择最有利的底牌放入棋盘中，也许将重要的牌在关键时候放置能逆转局势"},
        {"Calculation","结算阶段：依照双方的分数进行结算瓜分奖池"},
        // {"Raise",""},
    };
    public static readonly Dictionary<string, string> HintTextConditionDic = new Dictionary<string, string>()//条件性性提示
    {
        {"",""},
        {"FirstRaise", "后手玩家进行第一次加注"},
        { "EndPlace","最后一个放置回合，回合结束后进行结算"},
        {"BlankScreen","切换屏幕，请对应的玩家回避"},
        // { "",""},
        // { "",""},
    };
    public static readonly Dictionary<string, string> HintTextEventDic = new Dictionary<string, string>()//瞬时性提示
    {
        {"",""},
        {"RaiseButton", "给底注增加1，在此基础上付出底注的筹码"},
        {"CallButton", "付出与底注相同的筹码"},
        {"FoldButton", "输家弃牌赢家获得所有奖池，赢家弃牌与输家平分奖池"},
        {"HoleCardChoose","选择准备使用的底牌"},
        {"NodeQueue","选择底牌放置的位置，当敌方同排有与你底牌相同的骰子将会被消除，同一排相同的骰子越多，获得的分数越多"},
    };
    public static readonly Dictionary<string, string> HintTextUpDic = new Dictionary<string, string>()//上方的提示，用于提示刚刚发生了什么
    {//其中的玩家1，玩家2会被替换为p1p2
        {"",""},
        {"Raise","玩家1选择了加注，看来他对胜利的渴望难以控制"},
        {"Fold","玩家1选择了弃牌，啧啧，看来玩家1的胆量和他的筹码一样的少。"},
        {"Call","玩家1选择了跟注，看来他并不准备冒险"},
        {"WinerFold","玩家1选择了弃牌，看来他不准备再和玩家2纠缠下去了"},
        {"Hit1","玩家1随便放了个骰子，这是在测试桌子的硬度吗？还是说他的策略就是‘随缘’？"},
        {"Hit2","玩家1达成了双连，玩家2危险了"},
        {"Hit3","玩家1达成了三连！多么可怕的局面"},
        {"clear","玩家1的骰子和他的心态一起崩塌了"},
    };
}
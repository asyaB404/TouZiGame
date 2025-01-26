/********************************************************************
    Author:			Basyyya
    Date:			2025:1:9 18:02
    Description:	全局字段和常量
*********************************************************************/

public static class MyGlobal
{
    public const int MAX_PLAYER_COUNT = 2; //说不定呢，以后能做个2V2
    public const int A_STAGE_ROUND = 1; //一个阶段几个回合
    public const int INITIAL_CHIP = 10; //最大局数
    public const string CLIENT_PREFABS_PATH = "Prefabs/Network/MyClient";
    public const string SERVER_PREFABS_PATH = "Prefabs/Network/MyServer";
    
    public const float HOVER_SCALE_FACTOR = 1.2f; // 鼠标悬停时放大的缩放因子
    public const float INITIAL_SCALE = 1; // 初始缩放大小
}
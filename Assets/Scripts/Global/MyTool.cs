/********************************************************************
    Author:			Basyyya
    Date:			2025:1:7 15:29
    Description:	工具类
*********************************************************************/

public static class MyTool
{
    public static int GetNextPlayerId(int curPlayerId)
    {
        return (curPlayerId + 1) % MyGlobal.MAX_PLAYER_COUNT;
    }
}
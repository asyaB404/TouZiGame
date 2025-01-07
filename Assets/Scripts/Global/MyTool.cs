public static class MyTool
{
    public static int GetNextPlayerId(int curPlayerId)
    {
        return (curPlayerId + 1) % MyGlobal.MAX_PLAYER_COUNT;
    }
}
public static class MyGlobal
{
    public static int CurSeed { get; set; }
    public const int MAX_PLAYER_COUNT = 2; //说不定呢，以后能做个2V2
    public const string CLIENT_PREFABS_PATH = "Prefabs/Network/MyClient";
    public const string SERVER_PREFABS_PATH = "Prefabs/Network/MyServer";
}
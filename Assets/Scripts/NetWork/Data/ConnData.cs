// // ********************************************************************************************
// //     /\_/\                           @file       ConnData.cs
// //    ( o.o )                          @brief     Game07
// //     > ^ <                           @author     Basya
// //    /     \
// //   (       )                         @Modified   2025020117
// //   (___)___)                         @Copyright  Copyright (c) 2025, Basya
// // ********************************************************************************************

namespace NetWork.Data
{
    /// <summary>
    /// 作为CustomData并不需要同步，仅需要在服务端表示状态即可
    /// </summary>
    public class ConnData
    {
        public string name;
        public bool isReady;
    }
}
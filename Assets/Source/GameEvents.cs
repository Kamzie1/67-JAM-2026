using System;

namespace YANG
{
    public static class GameEvents
    {
        public static Action<int> DeadPlayer; 
        public static Action<int> Winner; 
    } 
}
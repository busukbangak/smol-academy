using Godot;
using System;

namespace Globals
{
    public class Constants : Node
    {

        public static class Units
        {

        }

        public static class Environments
        {
            public static string LANE1 = "res://Scenes/Environments/LANE1.tscn";
        }

        public static class Screens
        {
            public static string MAIN = "res://Scenes/Screens/MainScreen.tscn";

            public static string PAUSE = "res://Scenes/Screens/PauseMenu.tscn";

            public static string LOADING = "res://Scenes/Screens/LoadingScreen.tscn";

            public static string DEBUG_OVERLAY = "res://Scenes/Screens/DebugOverlay.tscn";
        }
    }

}

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

        public static class UI
        {
            public static string MAIN_SCREEN = "res://Scenes/Screens/MainScreen.tscn";

            public static string PAUSE_SCREEN = "res://Scenes/Screens/PauseMenu.tscn";

            public static string LOADING_SCREEN = "res://Scenes/Screens/LoadingScreen.tscn";

            public static string TRANSITION_SCREEN = "res://Scenes/Screens/TransitionScreen.tscn";

            public static string DEBUG_OVERLAY = "res://Scenes/Screens/DebugOverlay.tscn";

            public static string STATS_OVERLAY = "res://Scenes/Screens/StatsOverlay.tscn";

            public static string MINIMAP_OVERLAY = "res://Scenes/Screens/MiniMapOverlay.tscn";

            public static string DEAD_OVERLAY = "res://Scenes/Screens/DeadOverlay.tscn";

            public static string VICTORY_OVERLAY = "res://Scenes/Screens/VictoryOverlay.tscn";

            public static string DEFEAT_OVERLAY = "res://Scenes/Screens/DefeatOverlay.tscn";

            public static string ANNOUNCER_OVERLAY = "res://Scenes/Screens/AnnouncerOverlay.tscn";
        }
    }

}

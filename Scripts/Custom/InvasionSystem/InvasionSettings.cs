using System;

namespace Server.Custom.InvasionSystem
{
    internal static class InvasionSettings
    {
        internal static bool InvasionSysEnabled { get; private set; } = true;

        internal static void ToggleInvasionActive()
        {
            InvasionSysEnabled = !InvasionSysEnabled;

            if (InvasionSysEnabled)
            {
                InvasionEngine.StartInvasionTimer();
            }
            else
            {
                InvasionEngine.StopInvasionTimer();
            }
        }

        internal static bool InvasionSysDEBUG { get; set; } = false;

        internal const byte SYS_MARK = 101;

        internal const byte UNREST_LIMIT = 100;

        internal const byte CROWD_RANGE = 10;

        internal const byte CROWD_LIMIT = 5;

        internal const double INVADE_CHANCE = 0.1;

        internal const double SPAWN_CHANCE = 0.3;

        internal static TimeSpan MIN_INVADE = TimeSpan.FromMinutes(Utility.RandomMinMax(15, 60));

        internal static TimeSpan BASE_INVADE = TimeSpan.FromHours(Utility.RandomMinMax(1, 3));

        internal static TimeSpan MAX_INVADE = TimeSpan.FromHours(Utility.RandomMinMax(2, InvasionEngine.I_Players.Count + 3));
    }
}

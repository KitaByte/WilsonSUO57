using System;
using Server.Items;

namespace Server.Custom.UOSurvivalEvent
{
    internal static class SurvivalSettings
    {
        // Core
        internal static bool SurvivalEnabled { get; set; } = false;

        internal static bool IsAutoOff { get; set; } = true;

        internal static int MinPlayers { get; set; } = 2;

        internal static bool CanJoinLate { get; set; } = false;

        internal static int MatchDelay { get; set; } = 10; // seconds

        internal static bool StaffOnlyStart { get; set; } = false;

        // Rewards
        internal static bool IsGoldPrize { get; set; } = true;

        internal static int GoldAmount { get; set; } = 50;

        internal static bool IsItemPrize { get; set; } = false;

        internal static Type ItemPrize { get; set; } = typeof(ClothingBlessDeed);
    }
}

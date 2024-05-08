namespace Server.Custom.MovementSystem
{
    internal static class MoveSettings
    {
        internal const bool AutoLogPlayer = false; // Auto Logs Players into Movement System

        // Climbing Settings

        internal static bool ClimbActive = true; // Climbing Active

        internal static bool ClimbCasting = false; // Casting allowed?

        internal const int ClimbTargetRange = 3; // range in tiles

        internal const int ClimbRopeMod = 1; // per 3 tiles

        internal const int ClimbStamMod = 5; // per tile

        internal const double ClimbChance = 0.5; // Base before Skill Check!

        internal const double ClimbFallChance = 0.1; // Base before Skill Check!

        internal const int ClimbDamageMax = 10; // Max damage for falling!

        internal static Skill GetClimbingSkill(Mobile from)
        {
            return from.Skills.Mining; // Base climbing skill
        }

        // Swimming

        internal static bool SwimActive = true; // Swimming Active

        internal static bool SwimCasting = false; // Casting allowed?

        internal const int SwimTargetRange = 4; // range in tiles

        internal static int SwimSinkMod = 12; // How far player is submerged (-z)

        internal static int SwimSpeedMod = 1500; // 1000 = 1 sec

        internal static int SwimStamMod = 2; // Stam loss on slow swim in swallow water

        internal static int SwimHueMod = 2; // Hue of the foot and outer wares when in water, simulate being wet!

        internal const int SwimMaxDamage = 9; // Hypertheria for Deep Water!
    }
}

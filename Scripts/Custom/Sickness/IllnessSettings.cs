using System.IO;

namespace Server.Engines.Sickness
{
	public static class IllnessSettings
	{
		// Version
		public const int SysVersion = 11; // For Developer : Should not be tampered with!

		// FilePath is the file where the PlayerIllnessDict will be serialized to and deserialized from.
		public static readonly string FilePath = Path.Combine(@"Saves\Sickness101", $"Sickness{SysVersion}.bin");

		// SickChance is the percentage chance that an illness will be spawned when a creature dies.
		public const int SickChance = 1; // 1% Default

		// CureLootChance is the chance for a cure item to drop on a creature's death, 10% of n.
		public const int CureLootChance = 10000; // Default 10000 = 1 in 1000 chance to spawn item

		// Maximum doctor fee accepted to discover sickness
		public const int MaxDoctorFee = 200; // Gold

		// Adjustment for Immunity, 4 default, + would make it easier to catch and - harder to catch.
		public const int ImmunityMod = 4;

		// Min that immunity must be before player can catch infections
		public const int MinImmunity = 5; // out of 10

		// InDebug is a flag that controls whether debug messages will be printed to the console
		public const bool InDebug = false; // Override here to start in debug or in game for live toggle!
	}
}

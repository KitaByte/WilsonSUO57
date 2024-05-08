namespace Server.Services.DynamicNPC
{
	static internal class DynamicSettings
	{
		// System
		public static int Version = 1; // Dev Used Only : Versioning Project and Save Files
		public static bool InDebug { get; set; } = false; // Internal Debug for Help/Feedback

		// Buy & Sell Mods
		public static int BonusMod { get; set; } = 200; // Modify bonus gold
		public static int ChanceToLevel { get; set; } = 5; // Vendor level chance

		//Vendor 

		public static int MinAge = 18;

		public static int MaxAge = 100 + Utility.Random(20); // Max 120

		public static int MinLevel = 20;

		public static int MaxLevel = 100;

		public static int BaseChance = 5; // Extra bonuses from vendor

		public static bool WorksNight = false; // Will vendors work at night?

		// Task 

		public static int BaseDrawChance = 5; // inc/dec the randomness of tasks picked!
	}
}

using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	public class BaseSickness
	{
		// Represents the name of the sickness.
		public string IllName { get; set; }

		// Represents the type of sickness.
		public SicknessType Sickness { get; private set; }

		// Represents the type of remedy needed to cure the sickness.
		public RemedyType Remedy { get; private set; }

		// Represents the severity of the sickness.
		public int Severity { get; set; }

		// Represents the damage dealt to the player by the symptoms of the sickness.
		public int SymptomDamage { get; set; }

		// Represents the delay between symptoms of the sickness.
		public int SymptomDelay { get; set; }

		// Determines whether the sickness is contagious.
		public bool IsContagious { get; set; } = false;

		// Determines whether the sickness is transmitted through contact.
		// If false, than its air born.
		public bool IsContact { get; set; } = false;

		// Determines whether the sickness has been cured.
		public bool IsCured { get; set; } = false;

		// Determines whether the sickness has been discovered by the player.
		public bool IsDiscovered { get; set; } = false;

		// Constructor for the BaseSickness class.
		public BaseSickness(SicknessType sickness, RemedyType remedy)
		{
			IllName = IllnessUtility.SplitCamelCase(sickness.ToString());
			Sickness = sickness;
			Remedy = remedy;

			Severity = GetInfectionVal();
			SymptomDamage = GetInfectionVal();
			SymptomDelay = GetInfectionVal();
		}

		// Generates a random value between 1 and 10
		private int GetInfectionVal()
		{
			return Utility.RandomMinMax(1, 10);
		}

		// Overrides the ToString method to return the name of the sickness if it
		// has been discovered, or "Unknown Infection!" if it has not.
		public override string ToString()
		{
			return IsDiscovered? IllName : "Unknown Infection!";
		}
	}
}

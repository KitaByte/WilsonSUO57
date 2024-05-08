using Server.Engines.Sickness.IllnessTypes;
using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness
{
	// BaseRemedy represents a remedy for an illness.
	// It has a string representation of its name, a RemedyType enum representing its type,
	// and two ints representing its potency and damage.
	// It also has a bool indicating whether it has a side effect or not.
	public class BaseRemedy
	{
		// The name of the remedy
		public string RemName { get; set; }

		// The type of the remedy
		public RemedyType RemType { get; private set; }

		// The potency of the remedy
		public int RemPotency { get; set; } = Utility.RandomMinMax(1, 9);

		// The damage of the remedy
		public int RemModMinMax { get; set; } = Utility.RandomMinMax(1, 9);

		// Whether the remedy has a side effect or not
		public bool RemSideEffect { get; set; } = false;

		// Whether the remedy has a side effect or not
		public bool IsPositiveSE { get; set; } = false;

		// Constructor that takes in a RemedyType and initializes the RemType and RemName properties
		public BaseRemedy(RemedyType remType)
		{
			RemName = IllnessUtility.SplitCamelCase(remType.ToString());

			RemType = remType;
		}
	}
}

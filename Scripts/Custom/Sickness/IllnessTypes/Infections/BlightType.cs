using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class BlightType : BaseSickness
	{
		public BlightType() : base(SicknessType.Blight, RemedyType.HealingUnguent)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}

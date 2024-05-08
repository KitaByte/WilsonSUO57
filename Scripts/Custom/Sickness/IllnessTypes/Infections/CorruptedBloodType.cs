using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class CorruptedBloodType : BaseSickness
	{
		public CorruptedBloodType() : base(SicknessType.CorruptedBlood, RemedyType.Antidote)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}

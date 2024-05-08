using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class RedFeverType : BaseSickness
	{
		public RedFeverType() : base(SicknessType.RedFever, RemedyType.HealingBalm)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
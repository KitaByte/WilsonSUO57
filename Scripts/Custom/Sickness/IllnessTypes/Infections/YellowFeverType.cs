using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class YellowFeverType : BaseSickness
	{
		public YellowFeverType() : base(SicknessType.YellowFever, RemedyType.HealingTincture)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class WhiteFeverType : BaseSickness
	{
		public WhiteFeverType() : base(SicknessType.WhiteFever, RemedyType.HealingLiniment)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
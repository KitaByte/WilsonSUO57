using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class AncientCurseType : BaseSickness
	{
		public AncientCurseType() : base(SicknessType.AncientCurse, RemedyType.HolyWater)
		{
			IsContagious = false;
			IsContact = false;
		}
	}
}
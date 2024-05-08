using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class WarlockFeverType : BaseSickness
	{
		public WarlockFeverType() : base(SicknessType.WarlockFever, RemedyType.CureCursePotion)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
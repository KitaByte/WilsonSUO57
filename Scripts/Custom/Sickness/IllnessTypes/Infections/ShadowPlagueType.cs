using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class ShadowPlagueType : BaseSickness
	{
		public ShadowPlagueType() : base(SicknessType.ShadowPlague, RemedyType.UnicornHorn)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
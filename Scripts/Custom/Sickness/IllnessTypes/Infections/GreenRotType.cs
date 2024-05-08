using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class GreenRotType : BaseSickness
	{
		public GreenRotType() : base(SicknessType.GreenRot, RemedyType.HealingSalve)
		{
			IsContagious = false;
			IsContact = true;
		}
	}
}
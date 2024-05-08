using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class BlackDeathType : BaseSickness
	{
		public BlackDeathType() : base(SicknessType.BlackDeath, RemedyType.CureDiseasePotion)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}

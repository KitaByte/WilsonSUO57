using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class DragonPoxType : BaseSickness
	{
		public DragonPoxType() : base(SicknessType.DragonPox, RemedyType.HealingPoultice)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}

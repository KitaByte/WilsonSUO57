using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class ZombieVirusType : BaseSickness
	{
		public ZombieVirusType() : base(SicknessType.ZombieVirus, RemedyType.DemonHeart)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
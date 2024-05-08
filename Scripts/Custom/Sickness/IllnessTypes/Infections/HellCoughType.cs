using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class HellCoughType : BaseSickness
	{
		public HellCoughType() : base(SicknessType.HellCough, RemedyType.PhoenixFeather)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
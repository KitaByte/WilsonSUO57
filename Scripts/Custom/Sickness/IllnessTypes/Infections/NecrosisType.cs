using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class NecrosisType : BaseSickness
	{
		public NecrosisType() : base(SicknessType.Necrosis, RemedyType.BloodyTear)
		{
			IsContagious = false;
			IsContact = false;
		}
	}
}
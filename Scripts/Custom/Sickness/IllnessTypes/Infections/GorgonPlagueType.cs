using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class GorgonPlagueType : BaseSickness
	{
		public GorgonPlagueType() : base(SicknessType.GorgonPlague, RemedyType.ElixirOfLife)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
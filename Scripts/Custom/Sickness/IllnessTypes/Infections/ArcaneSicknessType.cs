using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class ArcaneSicknessType : BaseSickness
	{
		public ArcaneSicknessType() : base(SicknessType.ArcaneSickness, RemedyType.ArcaneElixir)
		{
			IsContagious = true;
			IsContact = false;
		}
	}
}
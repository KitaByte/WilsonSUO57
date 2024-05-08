using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class WyrmPoxType : BaseSickness
	{
		public WyrmPoxType() : base(SicknessType.WyrmPox, RemedyType.DragonfirePotion)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
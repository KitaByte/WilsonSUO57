using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class WeepingSoresType : BaseSickness
	{
		public WeepingSoresType() : base(SicknessType.WeepingSores, RemedyType.StarlightPotion)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
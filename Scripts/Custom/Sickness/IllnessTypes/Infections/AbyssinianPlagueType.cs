using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class AbyssinianPlagueType : BaseSickness
	{
		public AbyssinianPlagueType() : base (SicknessType.AbyssinianPlague, RemedyType.EnchantedForestMoss)
		{
			IsContagious = false; 
			IsContact = false;
		}
	}
}
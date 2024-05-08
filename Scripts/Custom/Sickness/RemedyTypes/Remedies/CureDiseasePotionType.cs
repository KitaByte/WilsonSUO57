namespace Server.Engines.Sickness.RemedyTypes
{
	internal class CureDiseasePotionType : BaseRemedy
	{
		public CureDiseasePotionType() : base(RemedyType.CureDiseasePotion)
		{
			RemSideEffect = false;
		}
	}
}

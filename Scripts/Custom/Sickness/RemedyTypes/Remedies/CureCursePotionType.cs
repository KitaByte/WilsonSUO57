namespace Server.Engines.Sickness.RemedyTypes
{
	internal class CureCursePotionType : BaseRemedy
	{
		public CureCursePotionType() : base(RemedyType.CureCursePotion)
		{
			RemSideEffect = true;
		}
	}
}

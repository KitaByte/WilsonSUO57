namespace Server.Engines.Sickness.RemedyTypes
{
	internal class StarlightPotionType : BaseRemedy
	{
		public StarlightPotionType() : base(RemedyType.StarlightPotion)
		{
			RemSideEffect = true;

			IsPositiveSE = true;
		}
	}
}

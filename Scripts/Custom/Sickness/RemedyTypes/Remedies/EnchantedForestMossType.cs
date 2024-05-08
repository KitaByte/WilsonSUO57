namespace Server.Engines.Sickness.RemedyTypes
{
	internal class EnchantedForestMossType : BaseRemedy
	{
		public EnchantedForestMossType() : base(RemedyType.EnchantedForestMoss)
		{
			RemSideEffect = true;

			IsPositiveSE = true;
		}
	}
}

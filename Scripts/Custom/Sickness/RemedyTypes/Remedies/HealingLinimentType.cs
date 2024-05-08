namespace Server.Engines.Sickness.RemedyTypes
{
	internal class HealingLinimentType : BaseRemedy
	{
		public HealingLinimentType() : base(RemedyType.HealingLiniment)
		{
			RemSideEffect = false;
		}
	}
}

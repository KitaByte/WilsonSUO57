namespace Server.Engines.Sickness.RemedyTypes
{
	internal class HealingBalmType : BaseRemedy
	{
		public HealingBalmType() : base(RemedyType.HealingBalm)
		{
			RemSideEffect = false;
		}
	}
}

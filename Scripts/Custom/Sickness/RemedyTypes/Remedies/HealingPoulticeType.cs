namespace Server.Engines.Sickness.RemedyTypes
{
	internal class HealingPoulticeType : BaseRemedy
	{
		public HealingPoulticeType() : base(RemedyType.HealingPoultice)
		{
			RemSideEffect = false;
		}
	}
}

namespace Server.Engines.Sickness.RemedyTypes
{
	internal class HealingUnguentType : BaseRemedy
	{
		public HealingUnguentType() : base(RemedyType.HealingUnguent)
		{
			RemSideEffect = false;
		}
	}
}

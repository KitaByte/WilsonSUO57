namespace Server.Engines.Sickness.RemedyTypes
{
	internal class HealingOintmentType : BaseRemedy
	{
		public HealingOintmentType() : base(RemedyType.HealingOintment)
		{
			RemSideEffect = false;
		}
	}
}

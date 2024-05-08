namespace Server.Engines.Sickness.RemedyTypes
{
	internal class HealingTinctureType : BaseRemedy
	{
		public HealingTinctureType() : base(RemedyType.HealingTincture)
		{
			RemSideEffect = false;
		}
	}
}

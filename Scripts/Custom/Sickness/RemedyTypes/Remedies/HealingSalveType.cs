namespace Server.Engines.Sickness.RemedyTypes
{
	internal class HealingSalveType : BaseRemedy
	{
		public HealingSalveType() : base(RemedyType.HealingSalve)
		{
			RemSideEffect = false;
		}
	}
}

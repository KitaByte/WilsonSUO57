namespace Server.Engines.Sickness.RemedyTypes
{
	internal class UnicornHornType : BaseRemedy
	{
		public UnicornHornType() : base(RemedyType.UnicornHorn)
		{
			RemSideEffect = true;

			IsPositiveSE = true;
		}
	}
}

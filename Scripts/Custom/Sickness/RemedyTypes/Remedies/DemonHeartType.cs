namespace Server.Engines.Sickness.RemedyTypes
{
	internal class DemonHeartType : BaseRemedy
	{
		public DemonHeartType() : base(RemedyType.DemonHeart)
		{
			RemSideEffect = true;
		}
	}
}

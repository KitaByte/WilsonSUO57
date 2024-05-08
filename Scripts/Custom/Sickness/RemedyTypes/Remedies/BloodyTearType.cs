namespace Server.Engines.Sickness.RemedyTypes
{
	internal class BloodyTearType : BaseRemedy
	{
		public BloodyTearType() : base(RemedyType.BloodyTear)
		{
			RemSideEffect = true;
		}
	}
}

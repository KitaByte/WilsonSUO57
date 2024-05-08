namespace Server.Engines.Sickness.RemedyTypes
{
	internal class AntidoteType : BaseRemedy
	{
		public AntidoteType() : base(RemedyType.Antidote)
		{
			RemSideEffect = false;
		}
	}
}

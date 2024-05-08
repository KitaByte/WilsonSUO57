namespace Server.Engines.Sickness.RemedyTypes
{
	internal class ArcaneElixirType : BaseRemedy
	{
		public ArcaneElixirType() : base(RemedyType.ArcaneElixir)
		{
			RemSideEffect = true;
		}
	}
}

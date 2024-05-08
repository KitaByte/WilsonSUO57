namespace Server.Engines.Sickness.RemedyTypes
{
	internal class ElixirOfLifeType : BaseRemedy
	{
		public ElixirOfLifeType() : base(RemedyType.ElixirOfLife)
		{
			RemSideEffect = true;

			IsPositiveSE = true;
		}
	}
}

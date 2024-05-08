namespace Server.Engines.Sickness.RemedyTypes
{
	internal class HolyWaterType : BaseRemedy
	{
		public HolyWaterType() : base(RemedyType.HolyWater)
		{
			RemSideEffect = false;
		}
	}
}

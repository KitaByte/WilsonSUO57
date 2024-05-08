namespace Server.Engines.Sickness.RemedyTypes
{
	internal class DragonfirePotionType : BaseRemedy
	{
		public DragonfirePotionType() : base(RemedyType.DragonfirePotion)
		{
			RemSideEffect = true;
		}
	}
}

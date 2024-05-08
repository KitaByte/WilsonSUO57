using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.IllnessTypes
{
	internal class FilthFeverType : BaseSickness
	{
		public FilthFeverType() : base(SicknessType.FilthFever, RemedyType.HealingOintment)
		{
			IsContagious = true;
			IsContact = true;
		}
	}
}
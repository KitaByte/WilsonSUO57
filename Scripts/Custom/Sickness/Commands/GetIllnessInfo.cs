using Server.Commands;
using Server.Commands.Generic;
using Server.Engines.Sickness.IllnessTypes;
using Server.Engines.Sickness.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Sickness.Commands
{
	public class GetIllnessInfo
	{
		public static void Initialize()
		{
			CommandSystem.Register("GetIllness", AccessLevel.Counselor, GetIllness_OnCommand);
		}

		[Usage("GetIllness")]
		[Description("Gets Player Illness Info.")]
		private static void GetIllness_OnCommand(CommandEventArgs e)
		{
			e.Mobile.Target = new GetHealthTarget();
		}
	}

	internal class GetHealthTarget : Target
	{
		public GetHealthTarget() : base(-1, true, TargetFlags.None)
		{ }

		protected override void OnTarget(Mobile from, object target)
		{
			if (!BaseCommand.IsAccessible(from, target))
			{
				from.SendMessage("That is not accessible.");
			}
			else if (target is PlayerMobile playerTarget)
			{
				if (IllnessHandler.GetPlayerIllness(playerTarget.Name) is Illness illness && illness != null)
				{
					var isHealthy = illness.GetFirstInfection() == null;

					var immuneLevel = illness.GetImmunityLevel(playerTarget) > IllnessSettings.MinImmunity - 1;

					from.SendMessage(isHealthy? 62:42, $"{playerTarget.Name}, Is healthy = {isHealthy}");

					from.SendMessage(53, $"{playerTarget.Name}, Active infections = {illness.GetActiveTotal(out var inActive)}");

					from.SendMessage(53, $"{playerTarget.Name}, Cured infections = {inActive}");

					from.SendMessage(immuneLevel? 62:42, $"{playerTarget.Name}, Immunity = {immuneLevel}");

					if (!IllnessHandler.InDebug && from.AccessLevel > AccessLevel.Player)
						return;

					if (from.Backpack.FindItemByType(typeof(Thermometer)) is Thermometer thermo && thermo != null)
					{
						if (thermo.HasGerms)
						{
							illness.TryInfection(playerTarget, SicknessUtility.GetSickness((int)thermo.GermType));
						}

						if (!isHealthy)
						{
							thermo.HasGerms = true;

							thermo.GermType = illness.GetFirstInfection().Sickness;
						}
					}
				}
			}
			else
			{
				from.SendMessage("That can't be targeted by this tool!");
			}
		}
	}
}

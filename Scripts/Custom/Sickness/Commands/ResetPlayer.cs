using Server.Commands;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Engines.Sickness.Commands
{
	public class ResetPlayer
	{
		public static void Initialize()
		{
			CommandSystem.Register("ResetPlayerIllness", AccessLevel.GameMaster, ResetPlayer_OnCommand);
		}

		[Usage("ResetPlayerIllness")]
		[Description("Reset Player Illness Data.")]
		private static void ResetPlayer_OnCommand(CommandEventArgs e)
		{
			e.Mobile.Target = new GetResetTarget();
		}
	}

	internal class GetResetTarget : Target
	{
		public GetResetTarget() : base(-1, true, TargetFlags.None)
		{ }

		protected override void OnTarget(Mobile from, object target)
		{
			if (target is PlayerMobile player)
			{
				var reset = IllnessHandler.ResetPlayerIllness(player);

				if (reset)
				{
					player.SendMessage("Player illness data cleared!");
				}
				else
				{
					player.SendMessage("Player illness data is empty!");
				}
			}
			else
			{
				from.SendMessage("That can't be targeted by this tool!");
			}
		}
	}
}

using Server.Commands;

namespace Server.Engines.Sickness.Commands
{
	public class ResetSystem
	{
		public static void Initialize()
		{
			CommandSystem.Register("ResetSicknessSys", AccessLevel.Administrator, ResetSys_OnCommand);
		}

		[Usage("ResetSicknessSys")]
		[Description("Reset Sickness System Data.")]
		private static void ResetSys_OnCommand(CommandEventArgs e)
		{
			var reset = IllnessHandler.ResetAll();

			if (reset)
			{
				e.Mobile.SendMessage("All illness data cleared!");
			}
			else
			{
				e.Mobile.SendMessage("All illness data is empty!");
			}
		}
	}
}

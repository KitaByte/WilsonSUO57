using Server.Commands;

namespace Server.Engines.Sickness.Commands
{
	public class DebugControl
	{
		public static void Initialize()
		{
			CommandSystem.Register("DebugSick", AccessLevel.Administrator, DebugSys_OnCommand);
		}

		[Usage("DebugSick")]
		[Description("Debug Sickness System Toggle On/Off.")]
		private static void DebugSys_OnCommand(CommandEventArgs e)
		{
			var debug = !IllnessHandler.InDebug;

			IllnessHandler.InDebug = debug;

			e.Mobile.SendMessage(debug? 62:42, $"{e.Mobile.Name}, Debug On => {debug}");
		}
	}
}

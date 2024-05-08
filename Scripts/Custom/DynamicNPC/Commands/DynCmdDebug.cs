using Server.Commands;

namespace Server.Services.DynamicNPC.Commands
{
	internal class DynCmdDebug
	{
		public static void Initialize()
		{
			CommandSystem.Register("DebugDynVendor", AccessLevel.Administrator, DebugSys_OnCommand);
		}

		[Usage("DebugDynVendor")]
		[Description("Debug Dynamic NPC System: Toggle On/Off")]
		private static void DebugSys_OnCommand(CommandEventArgs e)
		{
			var debug = !DynamicSettings.InDebug;

			DynamicSettings.InDebug = debug;

			e.Mobile.SendMessage(debug ? 62 : 42, $"{e.Mobile.Name}, Debug On => {debug}");
		}
	}
}


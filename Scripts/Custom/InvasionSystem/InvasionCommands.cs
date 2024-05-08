using Server.Commands;

namespace Server.Custom.InvasionSystem
{
    internal class InvasionCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("ToggleInvasionSystem", AccessLevel.Administrator, new CommandEventHandler(ToggleInvasions_OnCommand));
            CommandSystem.Register("DebugInvasionSystem", AccessLevel.Administrator, new CommandEventHandler(DebugInvasions_OnCommand));
        }

        // ToggleInvasionSystem
        [Usage("ToggleInvasionSystem")]
        [Description("Invasion System: ON/OFF")]
        private static void ToggleInvasions_OnCommand(CommandEventArgs e)
        {
            InvasionSettings.ToggleInvasionActive();

            var text = InvasionSettings.InvasionSysEnabled ? "ENABLED" : "DISABLED";

            e.Mobile.SendMessage(53, $"Invasion System : {text}");
        }

        // DebugInvasionSystem
        [Usage("DebugInvasionSystem")]
        [Description("Debug Invasion System: ON/OFF")]
        private static void DebugInvasions_OnCommand(CommandEventArgs e)
        {
            InvasionSettings.InvasionSysDEBUG = !InvasionSettings.InvasionSysDEBUG;

            var text = InvasionSettings.InvasionSysDEBUG ? "ON" : "OFF";

            e.Mobile.SendMessage(53, $"Debug Invasion System : {text}");
        }
    }
}

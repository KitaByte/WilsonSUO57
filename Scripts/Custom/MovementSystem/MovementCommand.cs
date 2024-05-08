using Server.Commands;

namespace Server.Custom.MovementSystem
{
    internal class MovementCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ToggleMove", AccessLevel.Player, new CommandEventHandler(ToggleMove_OnCommand));
        }

        [Usage("ToggleMovementSystem")]
        [Description("Movement System: ON/OFF")]
        private static void ToggleMove_OnCommand(CommandEventArgs e)
        {
            if (MovementCore.MovePlayers.Contains(e.Mobile))
            {
                MovementCore.MovePlayers.Remove(e.Mobile);

                e.Mobile.SendMessage(53, "Movement System : OFF");

                if (e.Mobile.HasGump(typeof(MovementGump)))
                {
                    e.Mobile.CloseGump(typeof(MovementGump));
                }
            }
            else
            {
                MovementCore.MovePlayers.Add(e.Mobile);

                e.Mobile.SendMessage(53, "Movement System : ON");
            }
        }
    }
}

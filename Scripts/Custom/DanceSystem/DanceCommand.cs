using Server.Commands;
using Server.Mobiles;

namespace Server.Custom.DanceSystem
{
    public static class DanceCommand 
    {
        public static void Initialize()
        {
            CommandSystem.Register("Dance", AccessLevel.Player, new CommandEventHandler(Dance_OnCommand));
        }

        [Usage("Dance")]
        [Description("Makes player dance.")]
        private static void Dance_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm && DanceEngine.IsValidConditions(pm))
            {
                DanceEngine.AddPlayer(pm);
            }
        }
    }
}

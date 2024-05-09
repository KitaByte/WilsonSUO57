using Server.Commands;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.UOHotBar
{
    internal class HotBarCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("TestSpell", AccessLevel.Administrator, new CommandEventHandler(TestSpell_OnCommand));
        }

        [Usage("TestSpell")]
        [Description("Hot Bar - Test Spell")]
        private static void TestSpell_OnCommand(CommandEventArgs e)
        {
            BaseGump.SendGump(new HotBarIconGump(e.Mobile as PlayerMobile));

            e.Mobile.SendMessage(53, "Testing Spell!");
        }
    }
}

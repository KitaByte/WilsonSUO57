using Server.Commands;
using Server.Mobiles;
using Server.Gumps;
using Server.Spells;
using Server.Spells.Ninjitsu;
using Server.Items;
using Server.Spells.First;

namespace Server.Custom.UOHotBar
{
    internal class HotBarCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("TestSpell", AccessLevel.Administrator, new CommandEventHandler(TestSpell_OnCommand));
            CommandSystem.Register("TestMove", AccessLevel.Administrator, new CommandEventHandler(TestMove_OnCommand));
        }

        [Usage("TestSpell")]
        [Description("Hot Bar - Test Spell")]
        private static void TestSpell_OnCommand(CommandEventArgs e)
        {
            HotBarIcon icon = new HotBarIcon(SpellRegistry.NewSpell(new AnimalForm(e.Mobile, null).ID, e.Mobile, null));

            BaseGump.SendGump(new HotBarIconGump(e.Mobile as PlayerMobile, icon));

            SendIconMessage(e.Mobile, icon);
        }

        [Usage("TestMove")]
        [Description("Hot Bar - Test Move")]
        private static void TestMove_OnCommand(CommandEventArgs e)
        {
            HotBarIcon icon = new HotBarIcon(new DeathStrike());

            BaseGump.SendGump(new HotBarIconGump(e.Mobile as PlayerMobile, icon));

            SendIconMessage(e.Mobile, icon);
        }

        private static void SendIconMessage(Mobile from, HotBarIcon icon)
        {
            icon.SendDebugMessage(from);
        }
    }
}

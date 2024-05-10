using Server.Commands;
using Server.Mobiles;
using Server.Gumps;
using Server.Spells;
using Server.Spells.Ninjitsu;
using Server.Items;

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
            SpellScroll scroll = new CreateFoodScroll();

            Spell spell = SpellRegistry.NewSpell(scroll.SpellID, e.Mobile, null);

            BaseGump.SendGump(new HotBarIconGump(e.Mobile as PlayerMobile, new HotBarIcon(spell)));

            e.Mobile.SendMessage(53, "Testing Spell!");
        }

        [Usage("TestMove")]
        [Description("Hot Bar - Test Move")]
        private static void TestMove_OnCommand(CommandEventArgs e)
        {
            SpecialMove move = new DeathStrike();

            BaseGump.SendGump(new HotBarIconGump(e.Mobile as PlayerMobile, new HotBarIcon(move)));

            e.Mobile.SendMessage(53, "Testing Move!");
        }
    }
}

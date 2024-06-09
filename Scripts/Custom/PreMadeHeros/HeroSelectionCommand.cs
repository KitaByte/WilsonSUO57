using Server.Targeting;
using Server.Commands;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    public static class HeroSelectionCommand
    {
        public static void Initialize()
        {
            CommandHandlers.Register("SelectHero", AccessLevel.GameMaster, SelectHero_CommandHandler);
            CommandHandlers.Register("GiveHero", AccessLevel.GameMaster, GiveHero_CommandHandler);
        }

        [Usage("SelectHero")]
        [Description("Opens Hero Selection Menu!")]
        private static void SelectHero_CommandHandler(CommandEventArgs e)
        {
            HeroUtility.SendHeroSelectGump(e.Mobile as PlayerMobile, null);
        }

        [Usage("GiveHero")]
        [Description("Targeted Player - Opens Hero Selection Menu!")]
        private static void GiveHero_CommandHandler(CommandEventArgs e)
        {
            e.Mobile.Target = new HeroSelectTarget();
        }

        private class HeroSelectTarget : Target
        {
            public HeroSelectTarget() : base(20, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile pm && pm != from)
                {
                    HeroUtility.SendHeroSelectGump(pm, null);
                }
            }
        }
    }
}

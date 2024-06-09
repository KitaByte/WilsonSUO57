using System.Linq;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal static class HeroUtility
    {
        internal static void ConvertPlayer(PlayerMobile pm, IHero hero)
        {
            pm.RawStr = hero.Str;
            pm.RawInt = hero.Int;
            pm.RawDex = hero.Dex;

            for (int i = 0; i < pm.Skills.Count(); i++)
            {
                if (hero.SkillList.Any(info => info.Name == pm.Skills[i].SkillName))
                {
                    var skillInfo = hero.SkillList.Find(si => si.Name == pm.Skills[i].SkillName);

                    pm.Skills[i].Base = skillInfo.Val;
                }
                else
                {
                    pm.Skills[i].Base = 0;
                }
            }

            pm.SendMessage(63, $"You have been converted into a {hero.Title}!");
        }

        public static void SendHeroSelectGump(PlayerMobile pm, HeroDeed deed)
        {
            if (pm.HasGump(typeof(HeroSelectionGump)))
            {
                pm.CloseGump(typeof(HeroSelectionGump));
            }

            BaseGump.SendGump(new HeroSelectionGump(pm, deed));
        }
    }
}

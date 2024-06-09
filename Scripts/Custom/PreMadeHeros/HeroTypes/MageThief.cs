using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class MageThief : IHero
    {
        public string Title => "Mage/Thief";

        public int Str => 50;

        public int Int => 100;

        public int Dex => 75;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Magery, 100),
                (SkillName.EvalInt, 100),
                (SkillName.Meditation, 100),
                (SkillName.Stealing, 100),
                (SkillName.Snooping, 100),
                (SkillName.Hiding, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


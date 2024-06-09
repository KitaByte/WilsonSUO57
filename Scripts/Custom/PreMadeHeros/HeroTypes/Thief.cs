using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class Thief : IHero
    {
        public string Title => "Thief";

        public int Str => 80;

        public int Int => 40;

        public int Dex => 105;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Stealing, 100),
                (SkillName.Snooping, 100),
                (SkillName.Hiding, 100),
                (SkillName.Stealth, 100),
                (SkillName.Lockpicking, 100),
                (SkillName.DetectHidden, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


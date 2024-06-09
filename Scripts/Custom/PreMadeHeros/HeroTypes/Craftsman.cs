using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class Craftsman : IHero
    {
        public string Title => "Craftsman";

        public int Str => 80;

        public int Int => 60;

        public int Dex => 85;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Blacksmith, 100),
                (SkillName.Tailoring, 100),
                (SkillName.Carpentry, 100),
                (SkillName.Tinkering, 100),
                (SkillName.Mining, 100),
                (SkillName.Lumberjacking, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


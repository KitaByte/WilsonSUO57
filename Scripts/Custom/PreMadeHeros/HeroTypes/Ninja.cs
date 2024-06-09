using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class Ninja : IHero
    {
        public string Title => "Ninja";

        public int Str => 80;

        public int Int => 40;

        public int Dex => 105;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Ninjitsu, 100),
                (SkillName.Hiding, 100),
                (SkillName.Stealth, 100),
                (SkillName.Fencing, 100),
                (SkillName.Tactics, 100),
                (SkillName.Healing, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


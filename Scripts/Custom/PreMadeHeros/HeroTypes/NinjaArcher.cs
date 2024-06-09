using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class NinjaArcher : IHero
    {
        public string Title => "Ninja/Archer";

        public int Str => 70;

        public int Int => 40;

        public int Dex => 115;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Ninjitsu, 100),
                (SkillName.Hiding, 100),
                (SkillName.Stealth, 100),
                (SkillName.Archery, 100),
                (SkillName.Tactics, 100),
                (SkillName.Anatomy, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


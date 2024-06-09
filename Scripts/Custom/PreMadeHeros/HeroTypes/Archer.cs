using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class Archer : IHero
    {
        public string Title => "Archer";

        public int Str => 80;

        public int Int => 30;

        public int Dex => 115;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Archery, 100),
                (SkillName.Tactics, 100),
                (SkillName.Anatomy, 100),
                (SkillName.Healing, 100),
                (SkillName.Focus, 100),
                (SkillName.Hiding, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class Samurai : IHero
    {
        public string Title => "Samurai";

        public int Str => 90;

        public int Int => 30;

        public int Dex => 105;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Bushido, 100),
                (SkillName.Swords, 100),
                (SkillName.Tactics, 100),
                (SkillName.Anatomy, 100),
                (SkillName.Healing, 100),
                (SkillName.Parry, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class Paladin : IHero
    {
        public string Title => "Paladin";

        public int Str => 90;

        public int Int => 50;

        public int Dex => 85;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Chivalry, 100),
                (SkillName.Swords, 100),
                (SkillName.Tactics, 100),
                (SkillName.Healing, 100),
                (SkillName.Parry, 100),
                (SkillName.Anatomy, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


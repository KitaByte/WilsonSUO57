using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class BardWarrior : IHero
    {
        public string Title => "Bard/Warrior";

        public int Str => 90;

        public int Int => 40;

        public int Dex => 95;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Swords, 100),
                (SkillName.Tactics, 100),
                (SkillName.Anatomy, 100),
                (SkillName.Healing, 100),
                (SkillName.Provocation, 100),
                (SkillName.Musicianship, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


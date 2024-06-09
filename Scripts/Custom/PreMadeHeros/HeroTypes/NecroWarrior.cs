using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class NecroWarrior : IHero
    {
        public string Title => "Necro/Warrior";

        public int Str => 80;

        public int Int => 70;

        public int Dex => 75;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Necromancy, 100),
                (SkillName.SpiritSpeak, 100),
                (SkillName.Swords, 100),
                (SkillName.Tactics, 100),
                (SkillName.Anatomy, 100),
                (SkillName.Healing, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


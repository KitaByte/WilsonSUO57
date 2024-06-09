using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class WarriorMage : IHero
    {
        public string Title => "Warrior/Mage";

        public int Str => 80;

        public int Int => 70;

        public int Dex => 75;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Swords, 100),
                (SkillName.Tactics, 100),
                (SkillName.Magery, 100),
                (SkillName.EvalInt, 100),
                (SkillName.Meditation, 100),
                (SkillName.Healing, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


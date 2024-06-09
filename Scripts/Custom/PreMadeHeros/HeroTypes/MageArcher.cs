using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class MageArcher : IHero
    {
        public string Title => "Mage/Archer";

        public int Str => 60;

        public int Int => 85;

        public int Dex => 80;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Archery, 100),
                (SkillName.Magery, 100),
                (SkillName.EvalInt, 100),
                (SkillName.Meditation, 100),
                (SkillName.Anatomy, 100),
                (SkillName.Healing, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


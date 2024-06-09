using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class NecroMage : IHero
    {
        public string Title => "Necro/Mage";

        public int Str => 50;

        public int Int => 100;

        public int Dex => 75;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Necromancy, 100),
                (SkillName.SpiritSpeak, 100),
                (SkillName.Magery, 100),
                (SkillName.EvalInt, 100),
                (SkillName.Meditation, 100),
                (SkillName.MagicResist, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}



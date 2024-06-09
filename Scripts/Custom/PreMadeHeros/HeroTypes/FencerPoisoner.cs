using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class FencerPoisoner : IHero
    {
        public string Title => "Fencer/Poisoner";

        public int Str => 75;

        public int Int => 50;

        public int Dex => 100;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Fencing, 100),
                (SkillName.Poisoning, 100),
                (SkillName.Tactics, 100),
                (SkillName.Anatomy, 100),
                (SkillName.Healing, 100),
                (SkillName.Hiding, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


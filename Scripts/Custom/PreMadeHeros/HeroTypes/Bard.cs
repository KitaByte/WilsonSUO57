using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    internal class Bard : IHero
    {
        public string Title => "Bard";

        public int Str => 60;

        public int Int => 80;

        public int Dex => 85;

        public List<(SkillName Name, int Val)> SkillList => new List<(SkillName, int)>
         {
                (SkillName.Provocation, 100),
                (SkillName.Discordance, 100),
                (SkillName.Musicianship, 100),
                (SkillName.Peacemaking, 100),
                (SkillName.Magery, 100),
                (SkillName.EvalInt, 100),
                (SkillName.Meditation, 100)
         };

        public void ApplyHero(PlayerMobile pm)
        {
            HeroUtility.ConvertPlayer(pm, this);
        }
    }
}


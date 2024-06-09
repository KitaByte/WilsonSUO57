using System.Collections.Generic;
using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    public interface IHero
    {
        string Title { get; }

        int Str { get; }
        int Int { get; }
        int Dex { get; }

        List<(SkillName Name, int Val)> SkillList { get; }

        void ApplyHero(PlayerMobile pm);
    }
}

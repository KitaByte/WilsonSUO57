using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Spells.Eighth;

namespace Server.Custom.UOHotBar
{
    internal class HotBarIconGump : BaseGump
    {
        public HotBarIconGump(PlayerMobile user) : base(user, 50, 50, null)
        {
        }

        public override void AddGumpLayout()
        {
            Closable = true;
            Resizable = false;
            Dragable = true;

            HotBarIcon icon = new HotBarIcon(new ResurrectionSpell(User, new ResurrectionScroll()));

            int id = HotBarArt.GetSpellArt(icon.GetSpell());

            AddBackground(X, Y, 44, 44, id);
        }
    }
}

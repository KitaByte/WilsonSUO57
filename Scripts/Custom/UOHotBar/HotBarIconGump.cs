using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.UOHotBar
{
    internal class HotBarIconGump : BaseGump
    {
        private HotBarIcon _Icon;

        public HotBarIconGump(PlayerMobile user, HotBarIcon icon) : base(user, 50, 50, null)
        {
            _Icon = icon;
        }

        public override void AddGumpLayout()
        {
            Closable = true;
            Resizable = false;
            Dragable = true;

            int id;

            if (_Icon.GetSpell() != null)
            {
                id = _Icon.GetSpellIcon();
            }
            else
            {
                id = _Icon.GetMoveIcon();
            }

            AddBackground(X, Y, 64, 64, 83);

            AddButton(X + 10, Y + 10, id, id, 1, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                if (_Icon.GetSpell() != null)
                {
                    _Icon.CastSpell(User);
                }
                else
                {
                    _Icon.CastMove(User);
                }
            }

            Close();
        }
    }
}

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    internal class ToolInfo : BaseGump
    {
        private IToolInfo Info { get; set; }

        public ToolInfo(BoxSession session, IToolInfo info, int x, int y, BaseGump parent) : base(session.User, x, y, parent)
        {
            Info = info;
        }

        public override void AddGumpLayout()
        {
            var width = 350;

            var height = 350;

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            AddHtml(X + 25, Y + 25, 300, 300, Info.LoadInfo().ToString(), false, true);

            GumpCore.SetGumpClose(this, width);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (Parent is BaseGump gump)
            {
                gump.Children.Remove(this);
            }

            base.OnResponse(info);
        }
    }
}

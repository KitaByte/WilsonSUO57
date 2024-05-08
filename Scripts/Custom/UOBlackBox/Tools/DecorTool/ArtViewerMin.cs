using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    internal class ArtViewerMin : BaseGump
    {
        public BoxSession Session { get; set; }

        private ArtViewer Viewer;

        public ArtViewerMin(BoxSession session, ArtViewer artViewer) : base(session.User, 0, 0, null)
        {
            Session = session;

            Viewer = artViewer;
        }

        public override void AddGumpLayout()
        {
            var title = Viewer.ListName;
            var offset = title.Length * 6.7;

            var width = 50 + (int)offset;
            var height = 49;

            AddBackground(X, Y, width, height, GumpCore.SubBG);

            // Title
            AddLabel(X + 15, Y + 17, GumpCore.GoldText, title);

            // Max
            AddButton(X + width - 30, Y + 17, GumpCore.MaxBtn, GumpCore.MaxBtn, 1, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                User.SendGump(Viewer);
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }
    }
}

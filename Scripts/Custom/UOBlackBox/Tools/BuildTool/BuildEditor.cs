using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class BuildEditor : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        public BuildEditor(BoxSession session) : base(session.User, 0, 0, null)
        {
            Session = session;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Gump Editor : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "UO Black Box : Build Editor";

            var width = 250;
            var height = 100;

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        public override void OnResponse(RelayInfo info)
        {
            base.OnResponse(info);
        }
    }
}

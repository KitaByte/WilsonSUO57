using System.Text;

using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox
{
    public class BoxSupport : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        public BoxSupport(BoxSession session) : base(session.User, 210, 90, null)
        {
            User.SendMessage(52, $"Opening Support");

            Session = session;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Support : Instructions");
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
            var title = "UO Black Box : Support";

            var width = 250;
            var height = 80;

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

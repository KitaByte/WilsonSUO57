using System.Text;

using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox
{
    internal class UnDoTool : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        public UnDoTool(BoxSession session) : base(session.User, 0, 10, null)
        {
            Session = session;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("UnDo : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Left Arrow - Undo!");
            information.AppendLine("Red Button - Undo All!");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "UO Black Box : UnDo";

            var width = 300;
            var height = 60;

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // 16x16 Undo
            AddButton(X + GumpCore.GetCentered(width, title, true) - 25, Y + 21, GumpCore.PrevBtnUP, GumpCore.PrevBtnDown, 1, GumpButtonType.Reply, 0);

            // Delete All
            AddButton(X + width - 45, Y + 23, GumpCore.RedUp, GumpCore.RedDown, 2, GumpButtonType.Reply, 0);

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                if (info.ButtonID == 1)
                {
                    UndoManager.Undo(User.Name);

                    Session.UpdateBox("UnDo");
                }
                else if (info.ButtonID == 2)
                {
                    UndoManager.UndoAll(User.Name);

                    Session.UpdateBox("UnDo All");
                }
                else
                {
                    GumpCore.SendInfoGump(Session, this);
                }

                Refresh(true, false);
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }
    }
}

using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class RemoveTool : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        public RemoveTool(BoxSession session) : base(session.User, 0, 0, null)
        {
            User.SendMessage(52, $"Opening Removal Tool");

            Session = session;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Remove Tool : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Id Input - Set ItemId (Area)");
            information.AppendLine("Red Button - Delete Art");
            information.AppendLine("Green Button - Area Delete Art");
            information.AppendLine("Blue Button - Wipe Art");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "UO Black Box : Removal Tool";

            var width = 250;
            var height = 100;

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // TargetID
            AddTextEntry(X + GumpCore.GetCentered(width, "1234", true), Y + 42, 50, 25, GumpCore.WhtText, 0, "ItemID");

            // Delete
            AddButton(X + 65, Y + 65, GumpCore.RedUp, GumpCore.RedDown, 1, GumpButtonType.Reply, 0);

            // Area Delete
            AddButton(X + 120, Y + 65, GumpCore.GreenUp, GumpCore.GreenDown, 2, GumpButtonType.Reply, 0);

            // Wipe
            AddButton(X + 174, Y + 65, GumpCore.BlueUp, GumpCore.BlueDown, 3, GumpButtonType.Reply, 0);

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        public override void OnResponse(RelayInfo info)
        {
            bool isValid = true;

            if (info.ButtonID == 2)
            {
                if (int.TryParse(info.GetTextEntry(0).Text, out _))
                {
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
            }

            if (!isValid)
            {
                User.SendMessage(32, "Invalid ItemID Entered!");

                Refresh(true, false);
            }
            else
            {
                if (info.ButtonID > 0)
                {
                    switch (info.ButtonID)
                    {
                        case 1: BoxCore.RunBBCommand(User, "m Delete"); User.SendMessage(52, $"Delete Art"); break;
                        case 2: BoxCore.RunBBCommand(User, $"Area Delete itemid {info.GetTextEntry(0)?.Text}"); User.SendMessage(52, $"Area Delete Art"); break;
                        case 3: BoxCore.RunBBCommand(User, "Wipe"); User.SendMessage(52, $"Wipe Art"); break;
                        default:
                            {
                                GumpCore.SendInfoGump(Session, this);

                                break;
                            }
                    }

                    if (info.ButtonID < 4)
                    {
                        Session.UpdateBox("Removal");
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
}

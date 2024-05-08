using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class GumpElements : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        public GumpElements(BoxSession session) : base(session.User, 0, 0, null)
        {
            Session = session;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Gump Editor : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Used to pick element");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "Elements";

            var width = 150;
            var height = 400;

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // Alpha
            AddLabel(X + 40, Y + 47, GumpCore.GoldText, "Alpha");
            AddButton(X + 25, Y + 50, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 1, GumpButtonType.Reply, 0);

            // Background
            AddLabel(X + 40, Y + 72, GumpCore.GoldText, "Background");
            AddButton(X + 25, Y + 75, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 2, GumpButtonType.Reply, 0);

            // Button
            AddLabel(X + 40, Y + 97, GumpCore.GoldText, "Button");
            AddButton(X + 25, Y + 100, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 3, GumpButtonType.Reply, 0);

            // Check
            AddLabel(X + 40, Y + 122, GumpCore.GoldText, "Checkbox");
            AddButton(X + 25, Y + 125, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 4, GumpButtonType.Reply, 0);

            // Html
            AddLabel(X + 40, Y + 147, GumpCore.GoldText, "Html");
            AddButton(X + 25, Y + 150, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 5, GumpButtonType.Reply, 0);

            // Image
            AddLabel(X + 40, Y + 172, GumpCore.GoldText, "Image");
            AddButton(X + 25, Y + 175, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 6, GumpButtonType.Reply, 0);

            // Image Tiled
            AddLabel(X + 40, Y + 197, GumpCore.GoldText, "Image Tiled");
            AddButton(X + 25, Y + 200, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 7, GumpButtonType.Reply, 0);

            // Item
            AddLabel(X + 40, Y + 222, GumpCore.GoldText, "Item");
            AddButton(X + 25, Y + 225, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 8, GumpButtonType.Reply, 0);

            // Label
            AddLabel(X + 40, Y + 247, GumpCore.GoldText, "Label");
            AddButton(X + 25, Y + 250, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 9, GumpButtonType.Reply, 0);

            // Label Cropped
            AddLabel(X + 40, Y + 272, GumpCore.GoldText, "Label Cropped");
            AddButton(X + 25, Y + 275, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 10, GumpButtonType.Reply, 0);

            // Radio
            AddLabel(X + 40, Y + 297, GumpCore.GoldText, "Radio");
            AddButton(X + 25, Y + 300, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 11, GumpButtonType.Reply, 0);

            // Text Entry
            AddLabel(X + 40, Y + 322, GumpCore.GoldText, "Text Entry");
            AddButton(X + 25, Y + 325, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 12, GumpButtonType.Reply, 0);

            // Tool Tip
            AddLabel(X + 40, Y + 347, GumpCore.GoldText, "Tool Tip");
            AddButton(X + 25, Y + 350, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 13, GumpButtonType.Reply, 0);

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                if (User.HasGump(typeof(GumpTune)))
                {
                    User.CloseGump(typeof(GumpTune));
                }

                switch (info.ButtonID)
                {
                    case 1:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.Alpha)));

                            break;
                        }
                    case 2:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.Background)));

                            break;
                        }
                    case 3:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.Button)));

                            break;
                        }
                    case 4:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.Check)));

                            break;
                        }
                    case 5:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.Html)));

                            break;
                        }
                    case 6:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.Image)));

                            break;
                        }
                    case 7:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.ImageTiled)));

                            break;
                        }
                    case 8:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.Item)));

                            break;
                        }
                    case 9:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.Label)));

                            break;
                        }
                    case 10:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.LabelCropped)));

                            break;
                        }
                    case 11:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.Radio)));

                            break;
                        }
                    case 12:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.TextEntry)));

                            break;
                        }
                    case 13:
                        {
                            SendGump(new GumpTune(Session, new GumpPrams(Elements.ToolTip)));

                            break;
                        }
                    default:
                        {
                            GumpCore.SendInfoGump(Session, this);

                            Session.UpdateBox("Info Selection");

                            Refresh(true, false);

                            break;
                        }
                }
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }
    }
}

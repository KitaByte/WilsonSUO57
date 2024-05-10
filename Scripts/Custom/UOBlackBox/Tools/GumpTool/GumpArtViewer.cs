using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class GumpArtViewer : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        public int GumpId { get; set; }

        public GumpArtViewer(BoxSession session, int gump = 0) : base(session.User, 0, 0, null)
        {
            if (gump == 0)
            {
                User.SendMessage(52, $"Opening Gump Art");
            }

            Session = session;

            GumpId = gump;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Gump Art Viewer : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Use to search gump art");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "Gump Art Picker";

            if (GumpId < 0 || GumpId > 65534)
            {
                GumpId = 0;
            }

            var image = UOArtHook.AllGumps[GumpId];

            var iWidth = image?.Width > 0 ? image.Width : 0;
            var iHeight = image?.Height > 0 ? image.Height : 0;

            var width = iWidth + 50 > 200 ? iWidth + 50 : 200;
            var height = iHeight + 75 > 250 ? iHeight + 75 : 250;

            var wOffset = (width - 50 - iWidth) / 2;
            var hOffset = (height - 75 - iHeight) / 2;

            width += 40; // For ID TextEntry!

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // 16x16 Prev
            AddButton(X + GumpCore.GetCentered(width, title, true) - 25, Y + 21, GumpCore.PrevBtnUP, GumpCore.PrevBtnDown, 1, GumpButtonType.Reply, 0);

            // 16x16 Next
            AddButton(X + GumpCore.GetCentered(width, title, false) + 25, Y + 21, GumpCore.NextBtnUP, GumpCore.NextBtnDown, 2, GumpButtonType.Reply, 0);

            // JumpTo #
            AddTextEntry(X + GumpCore.GetCentered(width, title, false) + 45, Y + 21, 35, 16, GumpCore.WhtText, 0, GumpId.ToString());

            // Gump
            if (iWidth == 0 && iHeight == 0)
            {
                AddLabel(X + 70, Y + 110, GumpCore.WhtText, "Enpty Slot");
            }
            else
            {
                AddImage(X + 25 + wOffset, Y + 50 + hOffset, GumpId);
            }

            // Gump Info
            AddLabel(X + 30, Y + height - 25, GumpCore.GoldText, $"{GumpId} [{GumpId:X4}] - {iWidth}x{iHeight}");

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        public override void OnResponse(RelayInfo info)
        {
            int GoodID = 0;

            if (int.TryParse(info.TextEntries[0].Text, out int id))
            {
                if (id > 0 && id < 65534)
                {
                    GoodID = id;
                }
            }

            if (info.ButtonID > 0)
            {
                switch (info.ButtonID)
                {
                    case 1:
                        {
                            if (GumpId > 0)
                            {
                                SendGump(new GumpArtViewer(Session, GumpId - 1));
                            }
                            else
                            {
                                SendGump(new GumpArtViewer(Session, 65534));
                            }

                            break;
                        }
                    case 2:
                        {
                            if (GumpId < 65534)
                            {
                                if (GoodID != 0 && GoodID != GumpId)
                                {
                                    SendGump(new GumpArtViewer(Session, GoodID));
                                }
                                else
                                {
                                    SendGump(new GumpArtViewer(Session, GumpId + 1));
                                }
                            }
                            else
                            {
                                SendGump(new GumpArtViewer(Session));
                            }

                            break;
                        }
                    default:
                        {
                            GumpCore.SendInfoGump(Session, this);

                            Session.UpdateBox("Gump Selection");

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

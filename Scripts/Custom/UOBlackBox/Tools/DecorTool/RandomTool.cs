using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    internal class RandomTool : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        private string Item1 { get; set; }
        private int Hue1 { get; set; }
        private string Item2 { get; set; }
        private int Hue2 { get; set; }
        private string Item3 { get; set; }
        private int Hue3 { get; set; }
        private string Item4 { get; set; }
        private int Hue4 { get; set; }
        private string Item5 { get; set; }
        private int Hue5 { get; set; }
        private int Rand { get; set; }

        public RandomTool(BoxSession session) : base(session.User, 0, 0, null)
        {
            User.SendMessage(52, $"Opening Random Tool");

            Session = session;

            Item1 = "0x0";
            Item2 = "0x0";
            Item3 = "0x0";
            Item4 = "0x0";
            Item5 = "0x0";

            Rand = 5;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Random Tool : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("5 slots to store random art");
            information.AppendLine("Set id/hue with Pop out art!");
            information.AppendLine("");
            information.AppendLine("Top Button - Loads ID/Hue");
            information.AppendLine("Bottom Button - Tile ID/Hue * Rand");
            information.AppendLine("Rand Entry - Set random id(+)Rand");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "UO Black Box : Random Tool";

            var width = 300;
            var height = 150;

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // Item 1
            AddLabel(X + 45, Y + 50, Hue1 == 0 ? GumpCore.WhtText : Hue1, Item1);

            AddButton(X + 50, Y + 70, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 1, GumpButtonType.Reply, 0);

            AddButton(X + 50, Y + 90, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 6, GumpButtonType.Reply, 0);

            // Item 2
            AddLabel(X + 95, Y + 50, Hue2 == 0 ? GumpCore.WhtText : Hue2, Item2);

            AddButton(X + 100, Y + 70, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 2, GumpButtonType.Reply, 0);

            AddButton(X + 100, Y + 90, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 7, GumpButtonType.Reply, 0);

            // Item 3
            AddLabel(X + 145, Y + 50, Hue3 == 0 ? GumpCore.WhtText : Hue3, Item3);

            AddButton(X + 150, Y + 70, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 3, GumpButtonType.Reply, 0);

            AddButton(X + 150, Y + 90, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 8, GumpButtonType.Reply, 0);

            // Item 4
            AddLabel(X + 195, Y + 50, Hue4 == 0 ? GumpCore.WhtText : Hue4, Item4);

            AddButton(X + 200, Y + 70, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 4, GumpButtonType.Reply, 0);

            AddButton(X + 200, Y + 90, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 9, GumpButtonType.Reply, 0);

            // Item 5
            AddLabel(X + 245, Y + 50, Hue5 == 0 ? GumpCore.WhtText : Hue5, Item5);

            AddButton(X + 250, Y + 70, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 5, GumpButtonType.Reply, 0);

            AddButton(X + 250, Y + 90, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 10, GumpButtonType.Reply, 0);

            // Frequency
            AddLabel(X + 110, Y + 115, GumpCore.GoldText, "Rand :");

            AddTextEntry(X + 155, Y + 115, 25, 25, GumpCore.WhtText, 0, $"{Rand}");

            AddLabel(X + 175, Y + 115, GumpCore.GoldText, "/10");

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                if (info.ButtonID < 6)
                {
                    switch (info.ButtonID)
                    {
                        case 1:
                            {
                                Item1 = Session.ArtSelected.ToString();

                                Hue1 = Session.HueSelected;

                                break;
                            }
                        case 2:
                            {
                                Item2 = Session.ArtSelected.ToString();

                                Hue2 = Session.HueSelected;

                                break;
                            }
                        case 3:
                            {
                                Item3 = Session.ArtSelected.ToString();

                                Hue3 = Session.HueSelected;

                                break;
                            }
                        case 4:
                            {
                                Item4 = Session.ArtSelected.ToString();

                                Hue4 = Session.HueSelected;

                                break;
                            }
                        case 5:
                            {
                                Item5 = Session.ArtSelected.ToString();

                                Hue5 = Session.HueSelected;

                                break;
                            }
                    }
                }
                else
                {
                    if (info.ButtonID < 11)
                    {
                        int id = 0;

                        int fetchHue = 0;

                        if (int.TryParse(info.GetTextEntry(0).Text, out int rand))
                        {
                            Rand = rand;
                        }

                        switch (info.ButtonID)
                        {
                            case 6:
                                {
                                    if (int.TryParse(Item1, out int val))
                                    {
                                        id = val;
                                    }

                                    fetchHue = Hue1;

                                    break;
                                }
                            case 7:
                                {
                                    if (int.TryParse(Item2, out int val))
                                    {
                                        id = val;
                                    }

                                    fetchHue = Hue2;

                                    break;
                                }
                            case 8:
                                {
                                    if (int.TryParse(Item3, out int val))
                                    {
                                        id = val;
                                    }

                                    fetchHue = Hue3;

                                    break;
                                }
                            case 9:
                                {
                                    if (int.TryParse(Item4, out int val))
                                    {
                                        id = val;
                                    }

                                    fetchHue = Hue4;

                                    break;
                                }
                            case 10:
                                {
                                    if (int.TryParse(Item5, out int val))
                                    {
                                        id = val;
                                    }

                                    fetchHue = Hue5;

                                    break;
                                }
                        }

                        BoxCore.RunBBCommand(User, $"TileAVG BoxStatic {User.Name} {User.Map.Name} {id} {Rand} set Hue {fetchHue}");

                        User.SendMessage(52, $"Adding Static {id}");
                    }
                    else
                    {
                        GumpCore.SendInfoGump(Session, this);
                    }
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

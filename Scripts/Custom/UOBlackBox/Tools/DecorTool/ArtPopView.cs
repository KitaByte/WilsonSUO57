using System.Linq;
using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class ArtPopView : BaseGump, IGumpHue, IToolInfo
    {
        public BoxSession Session { get; set; }

        private ArtViewer ArtGump { get; set; }

        private ArtEntity Art { get; set; }

        public int Hue { get; set; }

        private int LastZ = 0;

        public ArtPopView(BoxSession session, ArtEntity art, ArtViewer viewer) : base(session.User, 0, 0, null)
        {
            Session = session;

            Art = art;

            Hue = art.Hue;

            ArtGump = viewer;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Pop Art Viewer : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Bottom Left - Hue Picker");
            information.AppendLine("Bottom Right - Replace Art");
            information.AppendLine("First Button - Add Art");
            information.AppendLine("Middle Button - Tile Add Art");
            information.AppendLine("Last Button - Area Add Art");
            information.AppendLine("Z Input- Set Z");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            Art.Hue = Hue;

            var totalWidth = Art.Width + 60;
            var totalHeight = Art.Height + 110;

            var noDrawX = (totalWidth / 2) - 22;
            var noDrawY = Art.Height - 44;

            // 23x23 btn
            var heightOffset = totalHeight - 60;

            AddBackground(X, Y, totalWidth, totalHeight, GumpCore.SubBG);

            // Info
            AddButton(X, Y - 1, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 123456, GumpButtonType.Reply, 0);

            // Trans
            if (ArtGump != null && ArtGump.IsTransOn)
                AddAlphaRegion(X + 10, Y + 10, totalWidth - 20, 20);

            // Title
            if (ArtGump != null)
            {
                AddLabel(X + 15, Y + 10, GumpCore.GoldText, ArtGump.IsHex ? $"{Art.Hex}" : $"{Art.ID}");
            }
            else
            {
                AddLabel(X + 15, Y + 10, GumpCore.GoldText, $"{Art.ID}*{Art.Hex.Split('x').Last()}");
            }

            // NoDraw
            if (ArtGump != null && ArtGump.IsNoDraw)
                AddItem(X + noDrawX, Y + noDrawY + 40, 1, 0x4001);

            // Art
            AddItem(X + 30, Y + 40, Art.ID, Hue);

            // Hue
            AddButton(X, Y + totalHeight - 11, GumpCore.GreenUp, GumpCore.GreenDown, 1, GumpButtonType.Reply, 0);

            // Add
            AddButton(X + 15, Y + heightOffset, GumpCore.AddBtn, GumpCore.AddBtn, 2, GumpButtonType.Reply, 0);

            // Tile
            AddButton(X + (totalWidth / 2) - 12, Y + heightOffset, GumpCore.MultiBtn, GumpCore.MultiBtn, 3, GumpButtonType.Reply, 0);

            // Area
            AddButton(X + totalWidth - 38, Y + heightOffset, GumpCore.TileBtn, GumpCore.TileBtn, 4, GumpButtonType.Reply, 0);

            // Replace
            AddButton(X + totalWidth - 11, Y + totalHeight - 11, GumpCore.BlueUp, GumpCore.BlueDown, 5, GumpButtonType.Reply, 0);

            // Z Mod Entry
            AddTextEntry(X + (totalWidth / 2) - 10, Y + totalHeight - 30, 20, 24, GumpCore.GoldText, 0, LastZ > 0 ? LastZ.ToString() : "Z");

            // Close
            AddButton(X + totalWidth - 11, Y - 1, GumpCore.RedUp, GumpCore.RedDown, 0, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                var result = 0;
                var aboveGround = false;

                if (info.GetTextEntry(0).Text != "Z")
                {
                    if (int.TryParse(info.GetTextEntry(0).Text, out int newResult))
                    {
                        result = newResult;
                    }

                    aboveGround = result >= User.Map.GetAverageZ(User.Location.X, User.Location.Y) && result > -100 && result < 100;

                    if (aboveGround)
                    {
                        LastZ = result;
                    }
                }

                switch (info.ButtonID)
                {
                    case 1: // Hue
                        {
                            User.SendHuePicker(new ColorPicker(Session, Art.ID, this));

                            User.SendMessage(52, "Select a hue!");

                            break;
                        }
                    case 2: // Add
                        {
                            BoxCore.RunBBCommand(User, $"m Add BoxStatic {User.Name} {User.Map.Name} {Art.ID} set Name \"{Art.Name}\" Hue {Hue}");

                            User.SendMessage(52, $"Adding Static {Art.ID}");

                            break;
                        }
                    case 3: // Tile
                        {
                            if (result != 0 && aboveGround)
                            {
                                BoxCore.RunBBCommand(User, $"TileZ {result} BoxStatic {User.Name} {User.Map.Name} {Art.ID} set Name \"{Art.Name}\" Hue {Hue}");
                            }
                            else
                            {
                                BoxCore.RunBBCommand(User, $"TileAVG BoxStatic {User.Name} {User.Map.Name} {Art.ID} set Name \"{Art.Name}\" Hue {Hue}");
                            }

                            User.SendMessage(52, $"Tile Static {Art.ID}");

                            break;
                        }
                    case 4: // Area
                        {
                            if (result != 0 && aboveGround)
                            {
                                BoxCore.RunBBCommand(User, $"Area set itemid {Art.ID} Name \"{Art.Name}\" Hue {Hue} Z {result}");
                            }
                            else
                            {
                                BoxCore.RunBBCommand(User, $"Area set itemid {Art.ID} Name \"{Art.Name}\" Hue {Hue}");
                            }

                            User.SendMessage(52, $"Area Set Static {Art.ID}");

                            break;
                        }
                    case 5: // Replace
                        {
                            User.SendMessage(52, "Target the art to replace!");

                            User.Target = new ReplaceArt(Art);

                            break;
                        }
                    default:
                        {
                            GumpCore.SendInfoGump(Session, this);

                            break;
                        }
                }

                if (info.ButtonID < 6)
                {
                    Session.UpdateBox("Art");
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

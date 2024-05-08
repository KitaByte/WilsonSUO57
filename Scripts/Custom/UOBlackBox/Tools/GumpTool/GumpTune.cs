using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class GumpTune : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        private GumpPrams Prams { get; set; }

        public GumpLayer Layer { get; set; }

        public GumpTune(BoxSession session, GumpPrams prams) : base(session.User, 0, 0, null)
        {
            Session = session;

            Prams = prams;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Gump Editor : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Used to set up element");
            information.AppendLine("");
            information.AppendLine("Blue Button - Gump Viewer");
            information.AppendLine("Gold Button - Add Element");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "Element Settings";

            var range = GetRange();

            var width = 200;
            var height = 75 + (25 * range);

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // Add Elements
            AddElements();

            // Gump Picker
            AddButton(X + 80, Y + 50 + (25 * range), GumpCore.RndBtnDown, GumpCore.RndBtnUp, 1, GumpButtonType.Reply, 0);

            // Add Element
            AddButton(X + 110, Y + 50 + (25 * range), GumpCore.RndBtnUp, GumpCore.RndBtnDown, 2, GumpButtonType.Reply, 0);

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        private int GetRange()
        {
            switch (Prams.Element)
            {
                case Elements.Alpha: return 4;
                case Elements.Background: return 5;
                case Elements.Button: return 5;
                case Elements.Check: return 6;
                case Elements.Html: return 7;
                case Elements.Image: return 4;
                case Elements.ImageTiled: return 5;
                case Elements.Item: return 4;
                case Elements.Label: return 4;
                case Elements.LabelCropped: return 6;
                case Elements.Radio: return 6;
                case Elements.TextEntry: return 7;
                case Elements.ToolTip: return 1;
            }

            return 0;
        }

        private void AddElements()
        {
            switch (Prams.Element)
            {
                case Elements.Alpha: AddAlphaSet();
                    break;
                case Elements.Background: AddBackgroundSet();
                    break;
                case Elements.Button: AddButtonSet();
                    break;
                case Elements.Check: AddCheckboxSet();
                    break;
                case Elements.Html: AddHtmlSet();
                    break;
                case Elements.Image: AddImageSet();
                    break;
                case Elements.ImageTiled: AddImageTiledSet();
                    break;
                case Elements.Item: AddItemSet();
                    break;
                case Elements.Label: AddLabelSet();
                    break;
                case Elements.LabelCropped: AddLabelCroppedSet();
                    break;
                case Elements.Radio: AddRadioSet();
                    break;
                case Elements.TextEntry: AddTextEntrySet();
                    break;
                case Elements.ToolTip: AddToolTipSet();
                    break;
            }
        }

        private void AddLocation()
        {
            // LocX
            AddLabel(X + 25, Y + 50, GumpCore.GoldText, "X");
            AddTextEntry(X + 40, Y + 50, 50, 25, GumpCore.WhtText, 1, Prams.LocX.ToString());

            // LocY
            AddLabel(X + 25, Y + 75, GumpCore.GoldText, "Y");
            AddTextEntry(X + 40, Y + 75, 50, 25, GumpCore.WhtText, 2, Prams.LocY.ToString());
        }

        private void AddSize()
        {
            // Width
            AddLabel(X + 25, Y + 100, GumpCore.GoldText, "Width");
            AddTextEntry(X + 65, Y + 100, 50, 25, GumpCore.WhtText, 3, Prams.Width.ToString());

            // Height
            AddLabel(X + 25, Y + 125, GumpCore.GoldText, "Height");
            AddTextEntry(X + 65, Y + 125, 50, 25, GumpCore.WhtText, 4, Prams.Height.ToString());
        }

        private void AddAlphaSet()
        {
            AddLocation();

            AddSize();
        }

        private void AddBackgroundSet()
        {
            AddLocation();

            AddSize();

            // Art
            AddLabel(X + 25, Y + 150, GumpCore.GoldText, "Art");
            AddTextEntry(X + 50, Y + 150, 50, 25, GumpCore.WhtText, 5, Prams.Art.ToString());
        }

        private void AddButtonSet()
        {
            AddLocation();

            // Art Up
            AddLabel(X + 25, Y + 100, GumpCore.GoldText, "Up");
            AddTextEntry(X + 45, Y + 100, 50, 25, GumpCore.WhtText, 6, Prams.ArtUp.ToString());

            // Art Down
            AddLabel(X + 25, Y + 125, GumpCore.GoldText, "Down");
            AddTextEntry(X + 60, Y + 125, 50, 25, GumpCore.WhtText, 7, Prams.ArtDown.ToString());

            // Id
            AddLabel(X + 25, Y + 150, GumpCore.GoldText, "ID");
            AddTextEntry(X + 45, Y + 150, 50, 25, GumpCore.WhtText, 9, Prams.ID.ToString());
        }

        private void AddCheckboxSet()
        {
            AddLocation();

            // Art Up
            AddLabel(X + 25, Y + 100, GumpCore.GoldText, "Up");
            AddTextEntry(X + 45, Y + 100, 50, 25, GumpCore.WhtText, 6, Prams.ArtUp.ToString());

            // Art Down
            AddLabel(X + 25, Y + 125, GumpCore.GoldText, "Down");
            AddTextEntry(X + 60, Y + 125, 50, 25, GumpCore.WhtText, 7, Prams.ArtDown.ToString());

            // Id
            AddLabel(X + 25, Y + 150, GumpCore.GoldText, "ID");
            AddTextEntry(X + 45, Y + 150, 50, 25, GumpCore.WhtText, 9, Prams.ID.ToString());

            // Intial State
            AddLabel(X + 25, Y + 175, GumpCore.GoldText, "State");
            AddTextEntry(X + 65, Y + 175, 75, 25, GumpCore.WhtText, 10, Prams.IntState.ToString());
        }

        private void AddHtmlSet()
        {
            AddLocation();

            AddSize();

            // Has Background
            AddLabel(X + 25, Y + 150, GumpCore.GoldText, "HasBack");
            AddTextEntry(X + 75, Y + 150, 75, 25, GumpCore.WhtText, 11, Prams.HasBack.ToString());

            // Has Scroll Bar
            AddLabel(X + 25, Y + 175, GumpCore.GoldText, "HasBar");
            AddTextEntry(X + 70, Y + 175, 75, 25, GumpCore.WhtText, 12, Prams.HasBar.ToString());

            // Text
            AddLabel(X + 25, Y + 200, GumpCore.GoldText, "Text");
            AddTextEntry(X + 55, Y + 200, 150, 25, GumpCore.WhtText, 13, Prams.Text.ToString());
        }

        private void AddImageSet()
        {
            AddLocation();

            // Art
            AddLabel(X + 25, Y + 100, GumpCore.GoldText, "Art");
            AddTextEntry(X + 50, Y + 100, 50, 25, GumpCore.WhtText, 5, Prams.Art.ToString());

            // Hue
            AddLabel(X + 25, Y + 125, GumpCore.GoldText, "Hue");
            AddTextEntry(X + 50, Y + 125, 50, 25, GumpCore.WhtText, 8, Prams.Hue.ToString());
        }

        private void AddImageTiledSet()
        {
            AddLocation();

            AddSize();

            // Art
            AddLabel(X + 25, Y + 150, GumpCore.GoldText, "Art");
            AddTextEntry(X + 50, Y + 150, 50, 25, GumpCore.WhtText, 5, Prams.Art.ToString());
        }

        private void AddItemSet()
        {
            AddLocation();

            // Art
            AddLabel(X + 25, Y + 100, GumpCore.GoldText, "Art");
            AddTextEntry(X + 50, Y + 100, 50, 25, GumpCore.WhtText, 5, Prams.Art.ToString());

            // Hue
            AddLabel(X + 25, Y + 125, GumpCore.GoldText, "Hue");
            AddTextEntry(X + 50, Y + 125, 50, 25, GumpCore.WhtText, 8, Prams.Hue.ToString());
        }

        private void AddLabelSet()
        {
            AddLocation();

            // Hue
            AddLabel(X + 25, Y + 100, GumpCore.GoldText, "Hue");
            AddTextEntry(X + 50, Y + 100, 50, 25, GumpCore.WhtText, 8, Prams.Hue.ToString());

            // Text
            AddLabel(X + 25, Y + 125, GumpCore.GoldText, "Text");
            AddTextEntry(X + 55, Y + 125, 150, 25, GumpCore.WhtText, 13, Prams.Text.ToString());
        }

        private void AddLabelCroppedSet()
        {
            AddLocation();

            AddSize();

            // Hue
            AddLabel(X + 25, Y + 150, GumpCore.GoldText, "Hue");
            AddTextEntry(X + 50, Y + 150, 50, 25, GumpCore.WhtText, 8, Prams.Hue.ToString());

            // Text
            AddLabel(X + 25, Y + 175, GumpCore.GoldText, "Text");
            AddTextEntry(X + 55, Y + 175, 150, 25, GumpCore.WhtText, 13, Prams.Text.ToString());
        }

        private void AddRadioSet()
        {
            AddLocation();

            // Art Up
            AddLabel(X + 25, Y + 100, GumpCore.GoldText, "Up");
            AddTextEntry(X + 45, Y + 100, 50, 25, GumpCore.WhtText, 6, Prams.ArtUp.ToString());

            // Art Down
            AddLabel(X + 25, Y + 125, GumpCore.GoldText, "Down");
            AddTextEntry(X + 60, Y + 125, 50, 25, GumpCore.WhtText, 7, Prams.ArtDown.ToString());

            // Id
            AddLabel(X + 25, Y + 150, GumpCore.GoldText, "ID");
            AddTextEntry(X + 45, Y + 150, 50, 25, GumpCore.WhtText, 9, Prams.ID.ToString());

            // Intial State
            AddLabel(X + 25, Y + 175, GumpCore.GoldText, "State");
            AddTextEntry(X + 65, Y + 175, 75, 25, GumpCore.WhtText, 10, Prams.IntState.ToString());
        }

        private void AddTextEntrySet()
        {
            AddLocation();

            AddSize();

            // Hue
            AddLabel(X + 25, Y + 150, GumpCore.GoldText, "Hue");
            AddTextEntry(X + 50, Y + 150, 50, 25, GumpCore.WhtText, 8, Prams.Hue.ToString());

            // Id
            AddLabel(X + 25, Y + 175, GumpCore.GoldText, "ID");
            AddTextEntry(X + 45, Y + 175, 50, 25, GumpCore.WhtText, 9, Prams.ID.ToString());

            // Text
            AddLabel(X + 25, Y + 200, GumpCore.GoldText, "Text");
            AddTextEntry(X + 55, Y + 200, 150, 25, GumpCore.WhtText, 13, Prams.Text.ToString());
        }

        private void AddToolTipSet()
        {
            // Text
            AddLabel(X + 25, Y + 50, GumpCore.GoldText, "Text");
            AddTextEntry(X + 55, Y + 50, 150, 25, GumpCore.WhtText, 13, Prams.Text.ToString());
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                Prams.LocX = int.TryParse(info.GetTextEntry(1)?.Text, out int locX) == false ? 0 : locX;
                Prams.LocY = int.TryParse(info.GetTextEntry(2)?.Text, out int locY) == false ? 0 : locY;
                Prams.Width = int.TryParse(info.GetTextEntry(3)?.Text, out int width) == false ? 0 : width;
                Prams.Height = int.TryParse(info.GetTextEntry(4)?.Text, out int height) == false ? 0 : height;
                Prams.Art = int.TryParse(info.GetTextEntry(5)?.Text, out int art) == false ? 0 : art;
                Prams.ArtUp = int.TryParse(info.GetTextEntry(6)?.Text, out int artUp) == false ? 0 : artUp;
                Prams.ArtDown = int.TryParse(info.GetTextEntry(7)?.Text, out int artDown) == false ? 0 : artDown;
                Prams.Hue = int.TryParse(info.GetTextEntry(8)?.Text, out int hue) == false ? 0 : hue;
                Prams.ID = int.TryParse(info.GetTextEntry(9)?.Text, out int id) == false ? 0 : id;
                Prams.IntState = bool.TryParse(info.GetTextEntry(10)?.Text, out bool intState) != false && intState;
                Prams.HasBack = bool.TryParse(info.GetTextEntry(11)?.Text, out bool hasBack) != false && hasBack;
                Prams.HasBar = bool.TryParse(info.GetTextEntry(12)?.Text, out bool hasBar) != false && hasBar;
                Prams.Text = info.GetTextEntry(13)?.Text;

                if (Prams.Element == Elements.Image)
                {
                    Prams.Width = UOArtHook.AllGumps[Prams.Art].Width + Prams.LocX;
                    Prams.Height = UOArtHook.AllGumps[Prams.Art].Height + Prams.LocY;
                }

                if (Prams.Element == Elements.Item)
                {
                    Prams.Width = UOArtHook.GetArtImage(Prams.Art).Width + Prams.LocX;
                    Prams.Height = UOArtHook.GetArtImage(Prams.Art).Height + Prams.LocY;
                }

                if (info.ButtonID == 1)
                {
                    if (User.HasGump(typeof(GumpArtViewer)))
                    {
                        User.CloseGump(typeof(GumpArtViewer));
                    }

                    if (Prams.Art != 0)
                    {
                        SendGump(new GumpArtViewer(Session, Prams.Art));
                    }
                    else
                    {
                        SendGump(new GumpArtViewer(Session, Prams.ArtUp));
                    }

                    Refresh(true, false);
                }

                if (info.ButtonID == 2)
                {
                    if (User.HasGump(typeof(GumpEditor)))
                    {
                        User.CloseGump(typeof(GumpEditor));
                    }

                    if (Layer == null)
                    {
                        var layer = new GumpLayer(Prams.Element)
                        {
                            Prams = Prams
                        };

                        Session.Layers.Add(layer);
                    }

                    SendGump(new GumpEditor(Session));
                }
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }
    }
}

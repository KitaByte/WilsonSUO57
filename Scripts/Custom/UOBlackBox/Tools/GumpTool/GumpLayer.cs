using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public enum Elements
    {
        Alpha,
        Background,
        Button,
        Check,
        Html,
        Image,
        ImageTiled,
        Item,
        Label,
        LabelCropped,
        Radio,
        TextEntry,
        ToolTip
    }

    public class GumpLayer
    {
        public Elements Element { get; set; }

        public GumpPrams Prams { get; set; }

        public GumpLayer(Elements element)
        {
            Element = element;
        }

        public void AddGumpElement(GumpEditor gump)
        {
            switch (Element)
            {
                case Elements.Alpha: AddBoxAlphaRegion(gump);
                    break;
                case Elements.Background: AddBoxBackground(gump);
                    break;
                case Elements.Button: AddBoxButton(gump);
                    break;
                case Elements.Check: AddBoxCheck(gump);
                    break;
                case Elements.Html: AddBoxHtml(gump);
                    break;
                case Elements.Image: AddBoxImage(gump);
                    break;
                case Elements.ImageTiled: AddBoxImageTiled(gump);
                    break;
                case Elements.Item: AddBoxItem(gump);
                    break;
                case Elements.Label: AddBoxLabel(gump);
                    break;
                case Elements.LabelCropped: AddBoxLabelCropped(gump);
                    break;
                case Elements.Radio: AddBoxRadio(gump);
                    break;
                case Elements.TextEntry: AddBoxTextEntry(gump);
                    break;
                case Elements.ToolTip: AddBoxToolTip(gump);
                    break;
            }
        }

        private const int XOS = 25;

        private const int YOS = 50;

        private void AddBoxAlphaRegion(GumpEditor gump)
        {
            gump.AddAlphaRegion(Prams.LocX + XOS, Prams.LocY + YOS, Prams.Width, Prams.Height);
        }

        private void AddBoxBackground(GumpEditor gump)
        {
            gump.AddBackground(Prams.LocX + XOS, Prams.LocY + YOS, Prams.Width, Prams.Height, Prams.Art);
        }

        private void AddBoxButton(GumpEditor gump)
        {
            gump.AddButton(Prams.LocX + XOS, Prams.LocY + YOS, Prams.ArtUp, Prams.ArtDown, Prams.ID, GumpButtonType.Reply, 0);
        }

        private void AddBoxCheck(GumpEditor gump)
        {
            gump.AddCheck(Prams.LocX + XOS, Prams.LocY + YOS, Prams.ArtUp, Prams.ArtDown, Prams.IntState, Prams.ID);
        }

        private void AddBoxHtml(GumpEditor gump)
        {
            gump.AddHtml(Prams.LocX + XOS, Prams.LocY + YOS, Prams.Width, Prams.Height, Prams.Text, Prams.HasBack, Prams.HasBar);
        }

        private void AddBoxImage(GumpEditor gump)
        {
            gump.AddImage(Prams.LocX + XOS, Prams.LocY + YOS, Prams.Art, Prams.Hue);
        }

        private void AddBoxImageTiled(GumpEditor gump)
        {
            gump.AddImageTiled(Prams.LocX + XOS, Prams.LocY + YOS, Prams.Width, Prams.Height, Prams.Art);
        }

        private void AddBoxItem(GumpEditor gump)
        {
            gump.AddItem(Prams.LocX + XOS, Prams.LocY + YOS, Prams.Art, Prams.Hue);
        }

        private void AddBoxLabel(GumpEditor gump)
        {
            gump.AddLabel(Prams.LocX + XOS, Prams.LocY + YOS, Prams.Hue, Prams.Text);
        }

        private void AddBoxLabelCropped(GumpEditor gump)
        {
            gump.AddLabelCropped(Prams.LocX + XOS, Prams.LocY + YOS, Prams.Width, Prams.Height, Prams.Hue, Prams.Text);
        }

        private void AddBoxRadio(GumpEditor gump)
        {
            gump.AddRadio(Prams.LocX + XOS, Prams.LocY + YOS, Prams.ArtUp, Prams.ArtDown, Prams.IntState, Prams.ID);
        }

        private void AddBoxTextEntry(GumpEditor gump)
        {
            gump.AddTextEntry(Prams.LocX + XOS, Prams.LocY + YOS, Prams.Width, Prams.Height, Prams.Hue, Prams.ID, Prams.Text);
        }

        private void AddBoxToolTip(GumpEditor gump)
        {
            gump.AddTooltip(Prams.Text);
        }
    }
}

using static Server.Services.UOBlackBox.Tools.GumpLayer;

namespace Server.Services.UOBlackBox.Tools
{
    public class GumpPrams
    {
        public Elements Element { get; set; }

        public int LocX { get; set; } = 0;

        public int LocY { get; set; } = 0;

        public int Width { get; set; } = 0;

        public int Height { get; set; } = 0;

        public int Art { get; set; } = 0;

        public int ArtUp { get; set; } = 0;

        public int ArtDown { get; set; } = 0;

        public int Hue { get; set; } = 0;

        public int ID { get; set; } = 0;

        public bool IntState { get; set; } = false;

        public bool HasBack { get; set; } = false;

        public bool HasBar { get; set; } = false;

        public string Text { get; set; } = "Enter Text";

        public GumpPrams(Elements element)
        {
            Element = element;
        }
    }
}

using System.Drawing;

using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox
{
    public class ArtEntity
    {
        public string Name { get; private set; }
        public int ID { get; private set; }
        public string Hex { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Hue { get; set; }
        private ArtPopView Gump { get; set; }

        public Bitmap Image { get; private set; }

        public ArtEntity() { }

        public ArtEntity(string name, int pos, int width, int height, Bitmap image)
        {
            ID = pos;
            Image = image;
            Name = name == "" ? ArtCore.SetName(this, pos) : name;
            Hex = $"0x{pos:X4}";
            Width = width;
            Height = height;
            Hue = 0;
        }

        public void SendGump(BoxSession session, ArtViewer gump)
        {
            CloseGump();

            Gump = new ArtPopView(session, this, gump);

            BaseGump.SendGump(Gump);
        }

        public void CloseGump()
        {
            if (Gump != null && Gump.Open)
            {
                Gump.Close();
            }
        }

        public void Save(GenericWriter writer)
        {
            writer.Write(Name);
            writer.Write(ID);
            writer.Write(Hex);
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(Hue);
        }

        public void Load(GenericReader reader)
        {
            Name = reader.ReadString();
            ID = reader.ReadInt();
            Hex = reader.ReadString();
            Width = reader.ReadInt();
            Height = reader.ReadInt();
            Hue = reader.ReadInt();
        }
    }
}

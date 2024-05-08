using System.IO;

namespace Server.Custom.UOStudio
{
    public class PropInfo
    {
        public int ID { get; set; }
        public int HUE { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public PropInfo()
        {
        }

        public PropInfo(GenericReader reader)
        {
            LoadPorp(reader);
        }

        public PropInfo(int id, int hue, int x, int y, int z)
        {
            ID = id;

            HUE = hue;

            X = x;

            Y = y;

            Z = z;
        }

        public bool IsSame(PropInfo info)
        {
            if (info.ID != ID) return false;
            if (info.HUE != HUE) return false;

            if (info.X != X) return false;
            if (info.Y != Y) return false;
            if (info.Z != Z) return false;

            return true;
        }

        public void SavePorp(GenericWriter writer)
        {
            writer.Write(ID);
            writer.Write(HUE);

            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public void Export(StreamWriter writer)
        {
            writer.WriteLine(ID);
            writer.WriteLine(HUE);

            writer.WriteLine(X);
            writer.WriteLine(Y);
            writer.WriteLine(Z);
        }

        public void LoadPorp(GenericReader reader)
        {
            ID = reader.ReadInt();
            HUE = reader.ReadInt();

            X = reader.ReadInt();
            Y = reader.ReadInt();
            Z = reader.ReadInt();
        }

        public void Import(StreamReader reader)
        {
            ID = int.Parse(reader.ReadLine());
            HUE = int.Parse(reader.ReadLine());

            X = int.Parse(reader.ReadLine());
            Y = int.Parse(reader.ReadLine());
            Z = int.Parse(reader.ReadLine());
        }
    }
}

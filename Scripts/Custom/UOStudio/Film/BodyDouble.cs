using System.IO;
using Server.Items;
using System.Collections;

namespace Server.Custom.UOStudio
{
    public class BodyDouble
    {
        public bool IsFemale { get; set; }

        public string ActorName { get; set; }

        public int BodyID { get; set; }

        public int BodyIDMod { get; set; }

        public int SkinHue { get; set; }

        public int HairColor { get; set; }

        public int HairStyle { get; set; }

        public int FaceHairColor { get; set; }

        public int FaceHairStyle { get; set; }

        public ArrayList Clothing { get; set; }

        public ArrayList ClothingHue { get; set; }

        public BodyDouble()
        {
            Clothing = new ArrayList();

            ClothingHue = new ArrayList();
        }

        public void CopyActor(Mobile actor)
        {
            IsFemale = actor.Female;

            if (string.IsNullOrEmpty(actor.NameMod))
            {
                ActorName = actor.Name;
            }
            else
            {
                ActorName = actor.NameMod;
            }

            if (actor.BodyMod == 0x0)
            {
                BodyID = actor.Body;
            }
            else
            {
                BodyID = actor.BodyMod;
            }

            BodyIDMod = actor.BodyMod;

            if (actor.HueMod == -1)
            {
                SkinHue = actor.Hue;
            }
            else
            {
                SkinHue = actor.HueMod;
            }

            HairColor = actor.HairHue;

            HairStyle = actor.HairItemID;

            if (!IsFemale)
            {
                FaceHairStyle = actor.FacialHairItemID;

                FaceHairColor = actor.FacialHairHue;
            }

            foreach (var item in actor.Items)
            {
                if (item is Backpack || item is BaseTalisman)
                {
                    // do nothing
                }
                else
                {
                    if (!actor.Backpack.Items.Contains(item))
                    {
                        Clothing.Add(item.GetType().FullName);

                        ClothingHue.Add(item.Hue);
                    }
                }
            }
        }

        internal void Save(GenericWriter writer)
        {
            writer.Write(IsFemale);

            writer.Write(ActorName);

            writer.Write(BodyID);

            writer.Write(BodyIDMod);

            writer.Write(SkinHue);

            writer.Write(HairColor);

            writer.Write(HairStyle);

            if (!IsFemale)
            {
                writer.Write(FaceHairColor);

                writer.Write(FaceHairStyle);
            }

            writer.Write(Clothing.Count);

            foreach (var clothing in Clothing)
            {
                writer.Write(clothing.ToString());
            }

            foreach (var hue in ClothingHue)
            {
                writer.Write((int)hue);
            }
        }

        internal void Export(StreamWriter writer)
        {
            writer.WriteLine(IsFemale);

            writer.WriteLine(ActorName);

            writer.WriteLine(BodyID);

            writer.WriteLine(BodyIDMod);

            writer.WriteLine(SkinHue);

            writer.WriteLine(HairColor);

            writer.WriteLine(HairStyle);

            if (!IsFemale)
            {
                writer.WriteLine(FaceHairColor);

                writer.WriteLine(FaceHairStyle);
            }

            writer.WriteLine(Clothing.Count);

            if (Clothing.Count > 0)
            {
                int count = 0;

                foreach (var clothing in Clothing)
                {
                    writer.WriteLine(clothing.ToString());

                    writer.WriteLine((int)ClothingHue[count]);

                    count++;
                }
            }
        }

        internal void Load(GenericReader reader)
        {
            IsFemale = reader.ReadBool();

            ActorName = reader.ReadString();

            BodyID = reader.ReadInt();

            BodyIDMod = reader.ReadInt();

            SkinHue = reader.ReadInt();

            HairColor = reader.ReadInt();

            HairStyle = reader.ReadInt();

            if (!IsFemale)
            {
                FaceHairColor = reader.ReadInt();

                FaceHairStyle = reader.ReadInt();
            }

            Clothing = new ArrayList();

            ClothingHue = new ArrayList();

            int count = reader.ReadInt();

            for (int i = 0; i < count; i++)
            {
                Clothing.Add(reader.ReadString());
            }

            for (int i = 0; i < count; i++)
            {
                ClothingHue.Add(reader.ReadInt());
            }
        }

        internal void Import(StreamReader reader)
        {
            IsFemale = bool.Parse(reader.ReadLine());

            ActorName = reader.ReadLine();

            BodyID = int.Parse(reader.ReadLine());

            BodyIDMod = int.Parse(reader.ReadLine());

            SkinHue = int.Parse(reader.ReadLine());

            HairColor = int.Parse(reader.ReadLine());

            HairStyle = int.Parse(reader.ReadLine());

            if (!IsFemale)
            {
                FaceHairColor = int.Parse(reader.ReadLine());

                FaceHairStyle = int.Parse(reader.ReadLine());
            }

            var countClothing = int.Parse(reader.ReadLine());

            for (int j = 0; j < countClothing; j++)
            {
                Clothing.Add(reader.ReadLine());

                ClothingHue.Add(int.Parse(reader.ReadLine()));
            }
        }
    }
}

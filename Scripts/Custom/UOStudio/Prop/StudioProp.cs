using System.IO;
using Server.Items;

namespace Server.Custom.UOStudio
{
    public class StudioProp : Static
    {
        [Constructable]
        public StudioProp() : this(0, 0)
        {
            Name = "Studio Prop";

            Movable = false;
        }

        [Constructable]
        public StudioProp(int id, int hue) : base(id)
        {
            Name = "Studio Prop";

            Hue = hue;

            Movable = false;
        }

        public StudioProp(Serial serial) : base(serial)
        {
        }

        public void Export(StreamWriter writer)
        {
            writer.WriteLine(ItemID);
            writer.WriteLine(Hue);

            writer.WriteLine(X);
            writer.WriteLine(Y);
            writer.WriteLine(Z);
        }

        public void Import(StreamReader reader)
        {
            ItemID = int.Parse(reader.ReadLine());
            Hue = int.Parse(reader.ReadLine());

            X = int.Parse(reader.ReadLine());
            Y = int.Parse(reader.ReadLine());
            Z = int.Parse(reader.ReadLine());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}

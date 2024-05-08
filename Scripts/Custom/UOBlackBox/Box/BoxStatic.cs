using Server.Items;

namespace Server.Services.UOBlackBox
{
    public class BoxStatic : Static
    {
        [CommandProperty(AccessLevel.Administrator)]
        public string Staff { get; set; }

        [CommandProperty(AccessLevel.Administrator)]
        public bool IsUndo { get; set; }

        [Constructable] // Single ID
        public BoxStatic(string name, Map map, int id) : base(id)
        {
            SetProps(name, map);
        }

        [Constructable] 
        public BoxStatic(string name, Map map, int id, int count) : base(id, count)
        {
            SetProps(name, map);
        }

        private void SetProps(string name, Map map)
        {
            Staff = name;

            Map = map;

            ObjectPropertyList.Enabled = false;
        }

        public BoxStatic(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(Staff);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Staff = reader.ReadString();
        }
    }
}

namespace Server.Custom
{
    public class DevItem : Item
    {
        [Constructable]
        public DevItem() : this(0x9A1A)
        {
        }

        public DevItem(int itemID) : base(itemID)
        {
            Name = "Dev Test Item";

            Weight = 1.0;
        }

        public DevItem(Serial serial) : base(serial)
        {
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

using Server.Items;

namespace Server.Custom.Misc
{
    internal class Ham : Static
    {
        private int totalMeat = 5;

        [Constructable]
        public Ham(int left) : this()
        {
            totalMeat = left;
        }

        [Constructable]
        public Ham() : base(0x09D3)
        {
            Name = "Hame";

            Weight = 1.0;

            Movable = true;
        }

        public Ham(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.AddToBackpack(this);

            base.OnDoubleClick(from);   
        }

        public override bool OnDragLift(Mobile from)
        {
            if (totalMeat > 0 && Parent == null)
            {
                ItemID = 0x99A0;

                Name = "Pulled Pork Sandwich";

                totalMeat--;

                if (totalMeat > 0)
                {
                    Ham ham = new Ham(totalMeat);

                    ham.MoveToWorld(Location, Map);

                    from.SendMessage(53, "You make a pulled pork snadwich from the ham!");
                }
                else
                {
                    from.SendMessage(53, "You make a pulled pork snadwich from the last of the ham!");
                }

                totalMeat = 0;
            }

            return true;   
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(totalMeat);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            totalMeat = reader.ReadInt();
        }
    }
}

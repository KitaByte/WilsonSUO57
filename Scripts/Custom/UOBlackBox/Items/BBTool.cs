namespace Server.Services.UOBlackBox.Items
{
    public class BBTool : Item
    {
        private BlackBox b_Box;

        [Constructable]
        public BBTool(BlackBox box, string name, int art) : base(art)
        {
            b_Box = box;

            Name = $"Black Box : {name}";

            Hue = 2500;
        }

        public BBTool(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel > BoxCore.StaffAccess && b_Box != null)
            {
                if (Parent != b_Box)
                {
                    b_Box.Items.Add(this);
                }

                OpenGump(from, b_Box);
            }
            else
            {
                Delete();
            }

            base.OnDoubleClick(from);
        }

        public virtual void OpenGump(Mobile from, BlackBox b_Box)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            if (Parent != b_Box)
            {
                Delete();
            }

            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            if (Parent is BlackBox bb)
            {
                b_Box = bb;
            }
        }
    }
}

using Server.Gumps;

namespace Server.Services.UOBlackBox.Items
{
    public class BBSupport : BBTool
    {
        [Constructable]
        public BBSupport(BlackBox box) : base(box, "Support", 0x0EEC)
        {
        }

        public BBSupport(Serial serial) : base(serial)
        {
        }

        public override void OpenGump(Mobile from, BlackBox box)
        {
            if (!from.HasGump(typeof(BoxSupport)))
            {
                BaseGump.SendGump(new BoxSupport(box.Session));
            }

            base.OpenGump(from, box);
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

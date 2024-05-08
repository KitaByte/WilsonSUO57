using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox.Items
{
    public class BBRandomTool : BBTool
    {
        [Constructable]
        public BBRandomTool(BlackBox box) : base(box, "Random Tool", 0x0FA7)
        {
        }

        public BBRandomTool(Serial serial) : base(serial)
        {
        }

        public override void OpenGump(Mobile from, BlackBox box)
        {

            if (!from.HasGump(typeof(RandomTool)))
            {
                BaseGump.SendGump(new RandomTool(box.Session));
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

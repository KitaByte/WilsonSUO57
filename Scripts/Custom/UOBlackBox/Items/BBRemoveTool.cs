using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox.Items
{
    public class BBRemoveTool : BBTool
    {
        [Constructable]
        public BBRemoveTool(BlackBox box) : base(box, "Remove Tool", 0x0F4B)
        {
        }

        public BBRemoveTool(Serial serial) : base(serial)
        {
        }

        public override void OpenGump(Mobile from, BlackBox box)
        {

            if (!from.HasGump(typeof(RemoveTool)))
            {
                BaseGump.SendGump(new RemoveTool(box.Session));
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

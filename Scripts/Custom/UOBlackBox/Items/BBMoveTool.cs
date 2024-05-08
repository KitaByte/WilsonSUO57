using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox.Items
{
    public class BBMoveTool : BBTool
    {
        [Constructable]
        public BBMoveTool(BlackBox box) : base(box, "Move Tool", 0x2F58)
        {
        }

        public BBMoveTool(Serial serial) : base(serial)
        {
        }

        public override void OpenGump(Mobile from, BlackBox box)
        {
            if (!from.HasGump(typeof(MoveTool)))
            {
                BaseGump.SendGump(new MoveTool(box.Session));
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

using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox.Items
{
    public class BBGumpEditor : BBTool
    {

        [Constructable]
        public BBGumpEditor(BlackBox box) : base(box, "Gump Editor", 0x0FC0)
        {
        }

        public BBGumpEditor(Serial serial) : base(serial)
        {
        }

        public override void OpenGump(Mobile from, BlackBox box)
        {
            if (!from.HasGump(typeof(GumpEditor)))
            {
                BaseGump.SendGump(new GumpEditor(box.Session));
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

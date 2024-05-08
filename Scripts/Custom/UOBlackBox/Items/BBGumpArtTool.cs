using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox.Items
{
    public class BBGumpArtTool : BBTool
    {
        [Constructable]
        public BBGumpArtTool(BlackBox box) : base(box, "Gump Art Picker", 0x0BD1)
        {
        }

        public BBGumpArtTool(Serial serial) : base(serial)
        {
        }

        public override void OpenGump(Mobile from, BlackBox box)
        {
            if (!from.HasGump(typeof(GumpArtViewer)))
            {
                BaseGump.SendGump(new GumpArtViewer(box.Session));
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

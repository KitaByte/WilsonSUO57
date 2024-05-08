using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox.Items
{
    public class BBSearchTool : BBTool
    {
        [Constructable]
        public BBSearchTool(BlackBox box) : base(box, "Search Tool", 0x0FBD)
        {
        }

        public BBSearchTool(Serial serial) : base(serial)
        {
        }

        public override void OpenGump(Mobile from, BlackBox box)
        {

            if (!from.HasGump(typeof(SearchTool)))
            {
                BaseGump.SendGump(new SearchTool(box.Session));
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

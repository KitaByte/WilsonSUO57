using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox.Items
{
    public class BBHeatMapTool : BBTool
    {
        [Constructable]
        public BBHeatMapTool(BlackBox box) : base(box, "Heat Map", 0x14EB)
        {
        }

        public BBHeatMapTool(Serial serial) : base(serial)
        {
        }

        public override void OpenGump(Mobile from, BlackBox box)
        {

            if (!from.HasGump(typeof(HeatMapTool)))
            {
                BaseGump.SendGump(new HeatMapTool(box.Session));
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

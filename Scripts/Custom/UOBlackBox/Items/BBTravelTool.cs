using Server.Gumps;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox.Items
{
    public class BBTravelTool : BBTool
    {
        [Constructable]
        public BBTravelTool(BlackBox box) : base(box, "Travel Ball", 0x0E2D)
        {
        }

        public BBTravelTool(Serial serial) : base(serial)
        {
        }

        public override void OpenGump(Mobile from, BlackBox box)
        {
            if (!from.HasGump(typeof(TravelTool)))
            {
                BaseGump.SendGump(new TravelTool(box.Session));
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

using Server.Mobiles;

namespace Server.Custom.PreMadeHeros
{
    public class HeroDeed : Item
    {
        [Constructable]
        public HeroDeed() : base(0x14F0)
        {
            Name = "Hero Deed";

            Hue = 2747;

            Weight = 1.0;

            if (!Core.AOS)
                LootType = LootType.Newbied;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (RootParent == from && from is PlayerMobile pm)
            {
                HeroUtility.SendHeroSelectGump(pm, this);
            }
            else
            {
                from.SendMessage(53, "This must be in your backpack!");
            }

            base.OnDoubleClick(from);
        }

        public HeroDeed(Serial serial) : base(serial)
        {
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

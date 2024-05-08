using Server.Mobiles;

namespace Server.Services.UOBattleCards.Mobiles
{
    public class SmokeDragon : Dragon
    {
        [Constructable]
        public SmokeDragon() : base()
        {
            Name = "Sareus The Smoke Dragon";

            Hue = Settings.EtherealHue;
        }

        public SmokeDragon(Serial serial) : base(serial)
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

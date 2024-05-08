namespace Server.Custom.InfusionSystem.Infusions
{
    internal class EarthInfusion : BaseInfusion
    {
        [Constructable]
        public EarthInfusion() : base(InfusionType.Earth)
        {
        }

        public EarthInfusion(Serial serial) : base(serial)
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

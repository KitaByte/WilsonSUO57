namespace Server.Custom.InfusionSystem.Infusions
{
    internal class WindInfusion : BaseInfusion
    {
        [Constructable]
        public WindInfusion() : base(InfusionType.Wind)
        {
        }

        public WindInfusion(Serial serial) : base(serial)
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

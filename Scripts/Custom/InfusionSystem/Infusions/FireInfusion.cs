namespace Server.Custom.InfusionSystem.Infusions
{
    internal class FireInfusion : BaseInfusion
    {
        [Constructable]
        public FireInfusion() : base(InfusionType.Fire)
        {
        }

        public FireInfusion(Serial serial) : base(serial)
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

namespace Server.Custom.InfusionSystem.Infusions
{
    internal class RotInfusion : BaseInfusion
    {
        [Constructable]
        public RotInfusion() : base(InfusionType.Rot)
        {
        }

        public RotInfusion(Serial serial) : base(serial)
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

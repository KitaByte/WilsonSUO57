namespace Server.Custom.InfusionSystem.Infusions
{
    internal class MagicInfusion : BaseInfusion
    {
        [Constructable]
        public MagicInfusion() : base(InfusionType.Magic)
        {
        }

        public MagicInfusion(Serial serial) : base(serial)
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

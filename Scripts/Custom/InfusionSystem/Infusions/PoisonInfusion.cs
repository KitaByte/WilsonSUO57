namespace Server.Custom.InfusionSystem.Infusions
{
    internal class PoisonInfusion : BaseInfusion
    {
        [Constructable]
        public PoisonInfusion() : base(InfusionType.Poison)
        {
        }

        public PoisonInfusion(Serial serial) : base(serial)
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

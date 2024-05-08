namespace Server.Custom.InfusionSystem.Infusions
{
    internal class DeathInfusion : BaseInfusion
    {
        [Constructable]
        public DeathInfusion() : base(InfusionType.Death)
        {
        }

        public DeathInfusion(Serial serial) : base(serial)
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

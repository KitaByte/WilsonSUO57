
namespace Server.Custom.InfusionSystem.Infusions
{
    public class ColdInfusion : BaseInfusion
    {
        [Constructable]
        public ColdInfusion() : base(InfusionType.Cold)
        {
        }

        public ColdInfusion(Serial serial) : base(serial)
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

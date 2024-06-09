using Server.Mobiles;

namespace Server.Custom.Design2Script
{
    public class CreatureD2S : BaseCreature
    {
        [Constructable]
        public CreatureD2S() : this(AIType.AI_Use_Default, FightMode.None, 0, 0)
        {
            Name = "Blank Creature";

            Body = 58;

            CantWalk = true;
        }

        public CreatureD2S(AIType ai, FightMode mode, int iRP, int iRF) : base(ai, mode, iRP, iRF)
        {
        }

        public CreatureD2S(AIType ai, FightMode mode, int iRP, int iRF, double dAS, double dPS) : base(ai, mode, iRP, iRF, dAS, dPS)
        {
        }

        public CreatureD2S(Serial serial) : base(serial)
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

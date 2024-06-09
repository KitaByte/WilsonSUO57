using Server.Items;
using Server.Mobiles;

namespace Server.Custom.WispPack
{
    [CorpseName("a electric wisp corpse")]
    public class EnergyWisp : BaseCreature
    {
        [Constructable]
        public EnergyWisp() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.3, 0.6)
        {
            Name = "an electric wisp";

            Hue = 2734;

            WispUtility.SetWispStats(this, ResistanceType.Energy);

            SetSpecialAbility(SpecialAbility.ConductiveBlast);

            AddItem(new LightSource());

            PackBones();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            TryZapping(defender);

            base.OnGaveMeleeAttack(defender);
        }

        public override void OnSpellCast(ISpell spell)
        {
            if (Combatant != null && Combatant is Mobile defender)
            {
                TryZapping(defender);
            }

            base.OnSpellCast(spell);
        }

        private void TryZapping(Mobile defender)
        {
            if (defender.InRange(Location, WispUtility.GetRange(Title)) && Utility.Random(100) > defender.EnergyResistance)
            {
                Effects.SendBoltEffect(defender, true);

                defender.Damage(WispUtility.GetResistanceDamage(defender.EnergyResistance, Title));

                defender.SendMessage(34, "You were zapped by the energy from the wisp!");
            }
        }

        public EnergyWisp(Serial serial) : base(serial)
        {
        }

        public override OppositionGroup OppositionGroup
        {
            get
            {
                return OppositionGroup.FeyAndUndead;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            _ = reader.ReadInt();
        }
    }
}



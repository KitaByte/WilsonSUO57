using Server.Items;
using Server.Mobiles;

namespace Server.Custom.WispPack
{
    [CorpseName("a chaos wisp corpse")]
    public class ChaosWisp : BaseCreature
    {
        [Constructable]
        public ChaosWisp() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.3, 0.6)
        {
            Name = "a chaos wisp";

            Hue = 2500;

            WispUtility.SetWispStats(this, ResistanceType.Physical);

            SetSpecialAbility(SpecialAbility.StealLife);

            AddItem(new DarkSource());

            PackBones();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            TryWeakening(defender);

            base.OnGaveMeleeAttack(defender);
        }

        public override void OnSpellCast(ISpell spell)
        {
            if (Combatant != null && Combatant is Mobile defender)
            {
                TryWeakening(defender);
            }

            base.OnSpellCast(spell);
        }

        private void TryWeakening(Mobile defender)
        {
            if (defender.InRange(Location, WispUtility.GetRange(Title)) && Utility.Random(100) > defender.PhysicalResistance)
            {
                Effects.SendTargetEffect(defender, Utility.RandomList(0x37B9, 0x37C4), 30);

                Effects.PlaySound(defender.Location, Map, Utility.RandomList(0x1E1, 0x1E6));

                defender.Damage(WispUtility.GetResistanceDamage(defender.PhysicalResistance, Title));

                defender.SendMessage(34, "You were weakened by the dark aura of the wisp!");
            }
        }

        public ChaosWisp(Serial serial) : base(serial)
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

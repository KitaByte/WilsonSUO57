using Server.Items;
using Server.Mobiles;

namespace Server.Custom.WispPack
{
    [CorpseName("a fire wisp corpse")]
    public class FireWisp : BaseCreature
    {
        [Constructable]
        public FireWisp() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.3, 0.6)
        {
            Name = "a fire wisp";

            Hue = 2753;

            WispUtility.SetWispStats(this, ResistanceType.Fire);

            SetSpecialAbility(SpecialAbility.AngryFire);

            AddItem(new LightSource());

            PackBones();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            TryBurning(defender);

            base.OnGaveMeleeAttack(defender);
        }

        public override void OnSpellCast(ISpell spell)
        {
            if (Combatant != null && Combatant is Mobile defender)
            {
                TryBurning(defender);
            }

            base.OnSpellCast(spell);
        }

        private void TryBurning(Mobile defender)
        {
            if (defender.InRange(Location, WispUtility.GetRange(Title)) && Utility.Random(100) > defender.FireResistance)
            {
                Effects.SendTargetEffect(defender, Utility.RandomList(0x36B0, 0x36BD), 30);

                Effects.PlayExplodeSound(defender.Location, Map);

                defender.Damage(WispUtility.GetResistanceDamage(defender.FireResistance, Title));

                defender.SendMessage(34, "You were burnt by the radiant heat of the wisp!");
            }
        }

        public FireWisp(Serial serial) : base(serial)
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

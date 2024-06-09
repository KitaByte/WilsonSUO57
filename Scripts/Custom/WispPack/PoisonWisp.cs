using Server.Items;
using Server.Mobiles;

namespace Server.Custom.WispPack
{
    [CorpseName("a poisoned wisp corpse")]
    public class PoisonWisp : BaseCreature
    {
        [Constructable]
        public PoisonWisp() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.3, 0.6)
        {
            Name = "a poisoned wisp";

            Hue = 2755;

            WispUtility.SetWispStats(this, ResistanceType.Poison);

            SetSpecialAbility(SpecialAbility.PoisonSpit);

            AddItem(new LightSource());

            PackBones();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            TryPoisoning(defender);

            base.OnGaveMeleeAttack(defender);
        }

        public override void OnSpellCast(ISpell spell)
        {
            if (Combatant != null && Combatant is Mobile defender)
            {
                TryPoisoning(defender);
            }

            base.OnSpellCast(spell);
        }

        private void TryPoisoning(Mobile defender)
        {
            if (defender.InRange(Location, WispUtility.GetRange(Title)) && Utility.Random(100) > defender.PoisonResistance)
            {
                Effects.SendTargetEffect(defender, 0x11A6, 30);

                Effects.PlaySound(defender.Location, Map, 0x205);

                defender.Damage(WispUtility.GetResistanceDamage(defender.PoisonResistance, Title));

                if (!defender.Poisoned)
                {
                    defender.ApplyPoison(this, Poison.Lesser);
                }

                defender.SendMessage(34, "You were poisoned by the poisoned wisp's aura!");
            }
        }

        public PoisonWisp(Serial serial) : base(serial)
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


using Server.Items;
using Server.Mobiles;

namespace Server.Custom.WispPack
{
    [CorpseName("a frozen wisp corpse")]
    public class ColdWisp : BaseCreature
    {
        [Constructable]
        public ColdWisp() : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.3, 0.6)
        {
            Name = "a frozen wisp";

            Hue = 2729;

            WispUtility.SetWispStats(this, ResistanceType.Cold);

            SetSpecialAbility(SpecialAbility.Webbing);

            AddItem(new LightSource());

            PackBones();
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            TryFreezing(defender);

            base.OnGaveMeleeAttack(defender);
        }

        public override void OnSpellCast(ISpell spell)
        {
            if (Combatant != null && Combatant is Mobile defender)
            {
                TryFreezing(defender);
            }

            base.OnSpellCast(spell);
        }

        private void TryFreezing(Mobile defender)
        {
            if (defender.InRange(Location, WispUtility.GetRange(Title)) && Utility.Random(100) > defender.ColdResistance)
            {
                Effects.SendTargetEffect(defender, Utility.RandomList(0xA7E3, 0xA7F1), 30);

                Effects.PlaySound(defender.Location, Map, Utility.RandomList(0x107, 0x108));

                defender.Damage(WispUtility.GetResistanceDamage(defender.ColdResistance, Title));

                defender.SendMessage(34, "You were frozen by the frigid air of the wisp!");
            }
        }

        public ColdWisp(Serial serial) : base(serial)
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


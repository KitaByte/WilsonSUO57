using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Sickness.Mobiles
{
    [CorpseName("a infected flesh golem corpse")]
    public class InfectedFleshGolem : InfectedMobile
    {
        [Constructable]
        public InfectedFleshGolem() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a infected flesh golem";
            Body = 304;
            BaseSoundID = 684;

            Hue = 1175;

            SetStr(176, 200);
            SetDex(51, 75);
            SetInt(46, 70);

            SetHits(106, 120);

            SetDamage(18, 22);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 25, 35);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 50.1, 75.0);
            SetSkill(SkillName.Tactics, 55.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 70.0);

            Fame = 1000;
            Karma = -1800;

            SetWeaponAbility(WeaponAbility.BleedAttack);
        }

        public InfectedFleshGolem(Serial serial) : base(serial)
        {
        }

        public override bool BleedImmune => true;

        public override int TreasureMapLevel => 1;

		// Sickness props
		public override bool IsActiveOnGaveMelee => true;
		public override int OnGaveMeleeChance => 50;

		public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
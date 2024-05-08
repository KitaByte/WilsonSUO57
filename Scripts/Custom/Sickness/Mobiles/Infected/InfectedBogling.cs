using Server.Mobiles;

namespace Server.Engines.Sickness.Mobiles
{
    [CorpseName("a infected plant corpse")]
    public class InfectedBogling : InfectedMobile
    {
        [Constructable]
        public InfectedBogling() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a infected bogling";
            Body = 779;
            BaseSoundID = 422;

			Hue = 1175;

            SetStr(96, 120);
            SetDex(91, 115);
            SetInt(21, 45);

            SetHits(58, 72);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.MagicResist, 75.1, 100.0);
            SetSkill(SkillName.Tactics, 55.1, 80.0);
            SetSkill(SkillName.Wrestling, 55.1, 75.0);

            Fame = 450;
            Karma = -450;
        }

        public InfectedBogling(Serial serial) : base(serial)
        {
        }

        public override int Hides => 6;

        public override int Meat => 1;

		// Sickness props
		public override bool IsActiveOnHits => true;
		public override int OnHitsChance => 10;

		public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
    }
}

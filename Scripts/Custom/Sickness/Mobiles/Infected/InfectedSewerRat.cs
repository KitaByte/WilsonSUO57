using Server.Mobiles;

namespace Server.Engines.Sickness.Mobiles
{
    [CorpseName("a infected sewer rat corpse")]
    public class InfectedSewerrat : InfectedMobile
    {
        [Constructable]
        public InfectedSewerrat() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a infected sewer rat";
            Body = 238;
            BaseSoundID = 0xCC;

            Hue = 1175;

            SetStr(9);
            SetDex(25);
            SetInt(6, 10);

            SetHits(6);
            SetMana(0);

            SetDamage(1, 2);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 5.0);
            SetSkill(SkillName.Tactics, 5.0);
            SetSkill(SkillName.Wrestling, 5.0);

            Fame = 300;
            Karma = -300;

            Tamable = false;
            ControlSlots = 1;
            MinTameSkill = -0.9;
        }

        public InfectedSewerrat(Serial serial) : base(serial)
        {
        }

        public override int Meat => 1;

        public override FoodType FavoriteFood => FoodType.Meat | FoodType.Eggs | FoodType.FruitsAndVegies;

		// Sickness props
		public override bool IsActiveOnDamage => true;
		public override int OnDamageChance => 75;

		public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
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
using Server.Mobiles;

namespace Server.Engines.Sickness.Mobiles
{
    [CorpseName("a infected pig corpse")]
    public class InfectedPig : InfectedMobile
    {
        [Constructable]
        public InfectedPig() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a infected pig";
            Body = 0xCB;
            BaseSoundID = 0xC4;

            Hue = 1175;

            SetStr(20);
            SetDex(20);
            SetInt(5);

            SetHits(12);
            SetMana(0);

            SetDamage(2, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);

            SetSkill(SkillName.MagicResist, 5.0);
            SetSkill(SkillName.Tactics, 5.0);
            SetSkill(SkillName.Wrestling, 5.0);

            Fame = 150;
            Karma = 0;

            Tamable = false;
            ControlSlots = 1;
            MinTameSkill = 11.1;
        }

        public InfectedPig(Serial serial) : base(serial)
        {
        }

        public override int Meat => 1;

        public override FoodType FavoriteFood => FoodType.FruitsAndVegies | FoodType.GrainsAndHay;

		// Sickness props
		public override bool IsActiveOnDeath => true;
		public override int OnDeathChance => 50;

		public override bool IsActiveOnCarve => true;
		public override int OnCarveChance => 50;

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

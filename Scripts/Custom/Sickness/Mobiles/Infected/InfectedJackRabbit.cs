using Server.Mobiles;

namespace Server.Engines.Sickness.Mobiles
{
    [CorpseName("a infected jack rabbit corpse")]
    public class InfectedJackRabbit : InfectedMobile
    {
        [Constructable]
        public InfectedJackRabbit() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a infected jack rabbit";
            Body = 0xCD;

            Hue = 1175;

            SetStr(15);
            SetDex(25);
            SetInt(5);

            SetHits(9);
            SetMana(0);

            SetDamage(1, 2);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 2, 5);

            SetSkill(SkillName.MagicResist, 5.0);
            SetSkill(SkillName.Tactics, 5.0);
            SetSkill(SkillName.Wrestling, 5.0);

            Fame = 150;
            Karma = 0;

            Tamable = false;
            ControlSlots = 1;
            MinTameSkill = -18.9;
        }

        public InfectedJackRabbit(Serial serial) : base(serial)
        {
        }

        public override int Meat => 1;

        public override int Hides => 1;

        public override FoodType FavoriteFood => FoodType.FruitsAndVegies;

		// Sickness props
		public override bool IsActiveOnDamage => true;
		public override int OnDamageChance => 10;

		public override bool IsActiveOnCarve => true;
		public override int OnCarveChance => 50;

		public override int GetAttackSound()
        {
            return 0xC9;
        }

        public override int GetHurtSound()
        {
            return 0xCA;
        }

        public override int GetDeathSound()
        {
            return 0xCB;
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

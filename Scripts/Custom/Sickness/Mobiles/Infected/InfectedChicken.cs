using Server.Mobiles;

namespace Server.Engines.Sickness.Mobiles
{
    [CorpseName("a infected chicken corpse")]
    public class InfectedChicken : InfectedMobile
    {
        [Constructable]
        public InfectedChicken() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a infected chicken";
            Body = 0xD0;
            BaseSoundID = 0x6E;

			Hue = 1175;

            SetStr(5);
            SetDex(15);
            SetInt(5);

            SetHits(3);
            SetMana(0);

            SetDamage(1);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 1, 5);

            SetSkill(SkillName.MagicResist, 4.0);
            SetSkill(SkillName.Tactics, 5.0);
            SetSkill(SkillName.Wrestling, 5.0);

            Fame = 150;
            Karma = 0;

            Tamable = false;
            ControlSlots = 1;
            MinTameSkill = -0.9;
        }

        public InfectedChicken(Serial serial) : base(serial)
        {
        }

        public override int Meat => 1;

        public override MeatType MeatType => MeatType.Bird;

        public override FoodType FavoriteFood => FoodType.GrainsAndHay;

        public override bool CanFly => true;

        public override int Feathers => 25;

		// Sickness props
		public override bool IsActiveOnDamage => true;
		public override int OnDamageChance => 10;

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

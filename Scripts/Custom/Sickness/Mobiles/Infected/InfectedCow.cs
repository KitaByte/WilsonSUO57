using System;
using Server.Mobiles;

namespace Server.Engines.Sickness.Mobiles
{
    [CorpseName("a infected cow corpse")]
    public class InfectedCow : InfectedMobile
    {
        private DateTime m_MilkedOn;
        private int m_Milk;

        [Constructable]
        public InfectedCow() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a infected cow";
            Body = Utility.RandomList(0xD8, 0xE7);
            BaseSoundID = 0x78;

            Hue = 1175;

            SetStr(30);
            SetDex(15);
            SetInt(5);

            SetHits(18);
            SetMana(0);

            SetDamage(1, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 15);

            SetSkill(SkillName.MagicResist, 5.5);
            SetSkill(SkillName.Tactics, 5.5);
            SetSkill(SkillName.Wrestling, 5.5);

            Fame = 300;
            Karma = 0;

            Tamable = false;
            ControlSlots = 1;
            MinTameSkill = 11.1;

            if (Utility.Random(1000) == 0) // 0.1% chance to have mad cows
                FightMode = FightMode.Closest;
        }

        public InfectedCow(Serial serial) : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime MilkedOn
        {
            get
            {
                return m_MilkedOn;
            }
            set
            {
                m_MilkedOn = value;
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Milk
        {
            get
            {
                return m_Milk;
            }
            set
            {
                m_Milk = value;
            }
        }

        public override int Meat => 8;

        public override int Hides => 12;

        public override FoodType FavoriteFood => FoodType.FruitsAndVegies | FoodType.GrainsAndHay;

		// Sickness props
		public override bool IsActiveOnDeath => true;
		public override int OnDeathChance => 10;

		public override bool IsActiveOnCarve => true;
		public override int OnCarveChance => 50;

		public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            int random = Utility.Random(100);

            if (random < 5)
                Tip();
            else if (random < 20)
                PlaySound(120);
            else if (random < 40)
                PlaySound(121);
        }

        public void Tip()
        {
            PlaySound(121);
            Animate(8, 0, 3, true, false, 0);
        }

        public bool TryMilk(Mobile from)
        {
            if (!from.InLOS(this) || !from.InRange(Location, 2))
                from.SendLocalizedMessage(1080400); // You can not milk the cow from this location.
            if (Controlled && ControlMaster != from)
                from.SendLocalizedMessage(1071182); // The cow nimbly escapes your attempts to milk it.
            if (m_Milk == 0 && m_MilkedOn + TimeSpan.FromDays(1) > DateTime.UtcNow)
                from.SendLocalizedMessage(1080198); // This cow can not be milked now. Please wait for some time.
            else
            {
                from.SendMessage("You find the milk is smelly and green, it is useless!");

                return false;
            }

            return false;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);

            writer.Write(m_MilkedOn);
            writer.Write(m_Milk);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();

            m_MilkedOn = reader.ReadDateTime();
            m_Milk = reader.ReadInt();
        }
    }
}
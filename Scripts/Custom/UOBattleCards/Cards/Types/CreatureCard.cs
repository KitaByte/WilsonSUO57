using System.Text;

using Server.Services.UOBattleCards.Core;

namespace Server.Services.UOBattleCards.Cards.Types
{
	public class CreatureCard : BaseCard
    {
        public CreatureCard(CardInfo info) : base()
        {
            Info = info;

            SetUp();
        }

        [Constructable]
        public CreatureCard() : base()
        {
            Info = new CardInfo
            {
                Creature = CreatureUtility.GetRandomCreature()
            };

            Info.Name = Info.Creature;

            SetUp();
        }

        [Constructable]
        public CreatureCard(string creature) : base()
        {
            creature = CreatureUtility.CleanName(creature);

            if (CreatureUtility.HasCreature(creature))
            {
                Info = new CardInfo
                {
                    Name = creature,
                    Creature = creature
                };
            }
            else
            {
                Info = new CardInfo
                {
                    Creature = CreatureUtility.GetRandomCreature()
                };

                Info.Name = Info.Creature;
            }

            StaffAction(creature);

            SetUp();
        }

        private void StaffAction(string command)
        {
            command = command.ToLower();

            if (command == "foil")
            {
                Info.IsFoil = true;
            }

            if (command == "maxfoil")
            {
                Info.IsFoil = true;

                Info.Experience = 4000000;
            }

            if (command == "maxexp")
            {
                Info.Experience = 4000000;
            }

            if (command == "damage")
            {
                Info.Damage = 50;
            }
        }

        private void SetUp()
        {
            Weight = 0;

            Name = Info.Name;

            Hue = Settings.BaseHue;

            UpdateCard();
        }

        public override void GetDescription()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Rarity {Info.GetRarity()} - Value {Info.GetValue()}");
            sb.AppendLine($"Level {Info.GetLevel()} - Slots {Info.GetSlots()}");
            sb.AppendLine($"Atk {Info.GetAttack()} - Def {Info.GetDefense()}");

            Info.Description = sb.ToString();
        }

        public CreatureCard(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}

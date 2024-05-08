using Server.Services.UOBattleCards.Core;

namespace Server.Services.UOBattleCards.Cards.Types
{
    public class SpellCard : BaseCard
    {
        private (string Info, int Mod) SupportData { get; set; }

        public SpellCard() : base()
        {
            Weight = 0;

            Info = new CardInfo
            {
                Creature = CreatureUtility.GetRandomCreature(),
                CardType = CardTypes.Spell,
                Hue = Settings.SpellCardHue,
                Name = "Spell Card"
            };

            SetCardMod();

            UpdateCard();

            Name = Info.Name;

            Hue = Info.Hue;

            Info.Description = SupportData.Info;
        }

        private void SetCardMod()
        {
            switch (Utility.Random(3))
            {
                case 0: SupportData = ("\n\rMinor Reflect", 100); break;
                case 1: SupportData = ("\n\rReflect", 50); break;
                case 2: SupportData = ("\n\rMajor Reflect", 10); break;
                default: SupportData = ("\n\rMinor Reflect", 100); break;
            }
        }

        public int TryGetDamage()
        {
            return CreatureUtility.GetInfo(Info.Creature).C_Mana / SupportData.Mod;
        }

        public SpellCard(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(SupportData.Info);
            writer.Write(SupportData.Mod);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            SupportData = (reader.ReadString(), reader.ReadInt());
        }
    }
}

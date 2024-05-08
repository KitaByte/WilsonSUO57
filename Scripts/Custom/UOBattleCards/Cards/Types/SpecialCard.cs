using Server.Services.UOBattleCards.Core;

namespace Server.Services.UOBattleCards.Cards.Types
{
    public class SpecialCard : BaseCard
    {
        private (string Info, int Mod) SupportData { get; set; }

        public SpecialCard() : base()
        {
            Weight = 0;

            Info = new CardInfo
            {
                Creature = CreatureUtility.GetRandomCreature(),
                CardType = CardTypes.Special,
                Hue = Settings.SpecialCardHue,
                Name = "Special Card"
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
                case 0: SupportData = ("\n\rMinor Damage", 100); break;
                case 1: SupportData = ("\n\rMajor Damage", 25); break;
                case 2: SupportData = ("\n\rKnockout", 2); break;
                default: SupportData = ("\n\rMinor Damage", 100); break;
            }
        }

        public int TryGetDamage()
        {
            var info = CreatureUtility.GetInfo(Info.Creature);

            var combo = info.C_Hits + info.C_Mana + info.C_Stam;

            return combo / SupportData.Mod;
        }

        public SpecialCard(Serial serial) : base(serial)
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

using Server.Services.UOBattleCards.Core;

namespace Server.Services.UOBattleCards.Cards.Types
{
    public class TrapCard : BaseCard
    {
        private (string Info, int Mod) SupportData { get; set; }

        public TrapCard() : base()
        {
            Weight = 0;

            Info = new CardInfo
            {
                Creature = CreatureUtility.GetRandomCreature(),
                CardType = CardTypes.Trap,
                Hue = Settings.TrapCardHue,
                Name = "Trap Card"
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
                case 0: SupportData = ("\n\rMinor Block", 100); break;
                case 1: SupportData = ("\n\rBlock", 50); break;
                case 2: SupportData = ("\n\rMajor Block", 10); break;
                default: SupportData = ("\n\rMinor Block", 100); break;
            }
        }

        public int TryGetDamage()
        {
            var info = CreatureUtility.GetInfo(Info.Creature);

            var combo = info.C_Hits + info.C_Stam;

            return combo / SupportData.Mod;
        }

        public TrapCard(Serial serial) : base(serial)
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

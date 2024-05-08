using Server.Services.UOBattleCards.Core;

namespace Server.Services.UOBattleCards.Cards.Types
{
    public class SkillCard : BaseCard
    {
        private (string Info, int Mod) SupportData { get; set; }

        public SkillCard() : base()
        {
            Weight = 0;

            Info = new CardInfo
            {
                Creature = CreatureUtility.GetRandomCreature(),
                CardType = CardTypes.Skill,
                Hue = Settings.SkillCardHue,
                Name = "Skill Card"
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
                case 0: SupportData = ("\n\rMinor Dodge", 100); break;
                case 1: SupportData = ("\n\rDodge", 50); break;
                case 2: SupportData = ("\n\rMajor Dodge", 10); break;
                default: SupportData = ("\n\rMinor Dodge", 100); break;
            }
        }

        public int TryGetDamage()
        {
            return CreatureUtility.GetInfo(Info.Creature).C_Stam / SupportData.Mod;
        }

        public SkillCard(Serial serial) : base(serial)
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

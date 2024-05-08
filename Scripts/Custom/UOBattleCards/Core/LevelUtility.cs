using Server.Services.UOBattleCards.Cards.Types;

namespace Server.Services.UOBattleCards.Core
{
	public static class LevelUtility
    {
        private const int FixedXPLevel = Settings.CardExperienceMod;

        public static void SendActionXP(CreatureCard card, int amount)
        {
            var currentLevel = card.Info.GetLevel();

            if (currentLevel < 10)
            {
                if (card.Info.Experience > -1)
                {
                    card.Info.Experience += Utility.RandomMinMax(currentLevel, currentLevel + amount);
                }

                if (card.Info.Experience > FixedXPLevel * currentLevel)
                {
                    card.Info.Experience = 0;
                }
            }
            else
            {
                if (card.Info.Experience > 0)
                {
                    card.Info.Owner.SendMessage(52, "I'm done, stop training!");

                    card.Info.Experience = -1;
                }
            }
        }
    }
}

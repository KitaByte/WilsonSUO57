using Server.Items;
using Server.Services.UOBattleCards.Cards;

namespace Server.Services.UOBattleCards
{
    public static class Settings
    {
		// Creature Set Up

		public const bool RebuildCreatureCards = false;

		public const bool IsGoodCardsAllowed = true;

		public const int NameLengthMax = 25;

		// Card Set Up
		// Card Size = 225 x 326

		public const int CardItemId = 39956; // ID - 39956 (Large) 3607 (Small)

		public const int CardDisplaySound = 0x1E2;

		public const int CardRevealSound = 0x5B5;

		public const int CardRarityValue = 50; // Multiplier (50 gold * Rarity)

		public const int FoilChance = 10; // 10 out of 10000 chance

		public const double CardDropRate = 0.1; // 10% of total creature types

		public const int GemDropRate = 5; // 5 out of 100 chance		

		public const int CardExperienceMod = 10000; // per level : exp grows * Level

		// Card Hues

		public const int BaseHue = 2500;

		public const int FoilHue = 2734;

		public const int SkillCardHue = 1967;

		public const int SpecialCardHue = 2750;

		public const int SpellCardHue = 1968;

		public const int TrapCardHue = 1972;

		// Card Art

		public const int CardFace = 65500;

		public const int CardBack = 65501;

		public const int CardFoil = 65499; // Foil Range (65495 - 65499)

		public const int EtherealHue = 0x4000;

		public const int TitleGemUp = 5838;

		public const int TitleGemDown = 5840;

		public const int EmptySlotUp = 65531;

		public const int EmptySlotDown = 65512;

		// Full Card Gem Up (State)
		public static int GetGemUp(GemType gemType)
        {
            switch (gemType)
            {
                case GemType.None: return EmptySlotUp;
                case GemType.Diamond: return 65513;
                case GemType.Amethyst: return 65515;
                case GemType.StarSapphire: return 65517;
                case GemType.Emerald: return 65519;
                case GemType.Ruby: return 65521;
                case GemType.Sapphire: return 65523;
                case GemType.Tourmaline: return 65525;
                case GemType.Amber: return 65527;
                case GemType.Citrine: return 65529;
                default: return EmptySlotDown;
            }
        }

		// Full Card Gem Down (State)
		public static int GetGemDown(GemType gemType)
        {
            switch (gemType)
            {
                case GemType.None: return EmptySlotUp;
                case GemType.Diamond: return 65514;
                case GemType.Amethyst: return 65516;
                case GemType.StarSapphire: return 65518;
                case GemType.Emerald: return 65520;
                case GemType.Ruby: return 65522;
                case GemType.Sapphire: return 65524;
                case GemType.Tourmaline: return 65526;
                case GemType.Amber: return 65528;
                case GemType.Citrine: return 65530;
                default: return EmptySlotDown;
            }
        }

		// Arena Biome Art
		public static int GetBiomeArt(CardBiomes biome)
        {
            switch (biome)
            {
                case CardBiomes.City:       return 65502;
                case CardBiomes.Desert:     return 65503;
                case CardBiomes.Dungeons:   return 65504;
                case CardBiomes.Forest:     return 65505;
                case CardBiomes.Ocean:      return 65506;
                case CardBiomes.Plains:     return 65507;
                case CardBiomes.Snow:       return 65508;
                case CardBiomes.Swamp:      return 65509;
                case CardBiomes.Lava:       return 65510;
				case CardBiomes.Library:	return 65511;
            }

            return -1;
        }

		// Font Color 
		// Use HTML : https://www.rapidtables.com/web/color/html-color-codes.html
		public static string GetFontColor(bool isFoil)
        {
            return isFoil ? "FFDF00" : "FFFFFF";
        }

		public static string Shadow => "000000";

		// Card Blood Art
		public static int GetRandomBlood()
        {
            return Utility.RandomList(7570, 7571, 7572, 7573, 7574);
		}

		public static int HighlightHue = 2050;

		public static int HighlightFoilHue = 1072 + Utility.Random(20);

		//Gump Text
		public const int HeaderHue = 2720;

		public const int SubjectHue = 2499;

		public const double TextOffset = 3.1;

		// Battle Match Art
		// Match Size = 574 x 485
		public static int MatchArt = 65494;
	}
}

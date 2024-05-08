using Server.Items;
using Server.Services.UOBattleCards.Cards.Types;

namespace Server.Services.UOBattleCards.Cards
{
    public enum CardTypes
    {
        Creature = 0,
        Skill = 1,
        Spell = 2,
        Special = 3,
        Trap = 4
    }

    public enum CardBiomes
    {
        City = 0,
        Desert = 1,
        Dungeons = 2,
        Forest = 3,
        Ocean = 4,
        Plains = 5,
        Snow = 6,
        Swamp = 7,
        Lava = 8,
		Library = 9
    }

    public static class CardUtility
    {
		public static void PlayGlowEffect(Mobile player, bool cardLoc, Point3D Location, Map map)
        {
            if (cardLoc)
            {
                Effects.SendLocationEffect(new Point3D(Location.X + 1, Location.Y + 1, Location.Z), map, 0x37C4, 15, Utility.RandomBrightHue(), 0);

                Effects.PlaySound(Location, map, Settings.CardDisplaySound);
            }
            else
            {
                Effects.SendLocationEffect(player.Location, player.Map, 0x37C4, 15, Utility.RandomBrightHue(), 0);

                player.PlaySound(Settings.CardRevealSound);
            }
        }

        public static CardBiomes GetCardBiome(Mobile from, CardInfo card, bool IsWet)
        {
			if (card.Card == null || InteriorDecorator.InHouse(from))
			{
				return CardBiomes.Library;
			}

            if (IsWet) { return CardBiomes.Ocean; }

            if (from.Region != null)
            {
                switch (from.Region)
                {
                    case Regions.DungeonRegion _: return CardBiomes.Dungeons;
                    case Regions.TownRegion _: return CardBiomes.City;
                    case Regions.Underwater _: return CardBiomes.Ocean;
                }
            }

            var landTile = from.Map.Tiles.GetLandTile(from.X, from.Y);

            var tile = TileData.LandTable[landTile.ID & 0x3FFF];

            switch (tile.Name.ToLower())
            {
                case "cobblestone": { return CardBiomes.City; }
                case "sand stone": { return CardBiomes.City; }
                case "wooden floor": { return CardBiomes.City; }
                case "flagstone": { return CardBiomes.City; }
                case "marble": { return CardBiomes.City; }
                case "planks": { return CardBiomes.City; }
                case "bricks": { return CardBiomes.City; }
                case "tile": { return CardBiomes.City; }
                case "stone": { return CardBiomes.City; }
                case "sand": { return CardBiomes.Desert; }
                case "cave": { return CardBiomes.Dungeons; }
                case "forest": { return CardBiomes.Forest; }
                case "tree": { return CardBiomes.Forest; }
                case "jungle": { return CardBiomes.Forest; }
                case "grass": { return CardBiomes.Plains; }
                case "dirt": { return CardBiomes.Plains; }
                case "furrows": { return CardBiomes.Plains; }
                case "snow": { return CardBiomes.Snow; }
                case "swamp": { return CardBiomes.Swamp; }
                case "acid": { return CardBiomes.Swamp; }
                case "lava": { return CardBiomes.Lava; }
                case "rock": { return CardBiomes.Lava; }
                default: return CardBiomes.Library;
            }
        }

        public static BaseCard GetSupportCard()
        {
            var cardType = (CardTypes)Utility.RandomList(1, 2, 3, 4);

            BaseCard card = null;

            switch (cardType)
            {
                case CardTypes.Skill:
                    {
                        card = new SkillCard();

                        break;
                    }
                case CardTypes.Spell:
                    {
                        card = new SpellCard();

                        break;
                    }
                case CardTypes.Special:
                    {
                        card = new SpecialCard();

                        break;
                    }
                case CardTypes.Trap:
                    {
                        card = new TrapCard();

                        break;
                    }
            }

            card?.Info.UpdateIsRevealed();

            return card;
        }
    }
}

using Server.Gumps;
using Server.Mobiles;
using Server.Services.UOBattleCards.Cards;
using Server.Services.UOBattleCards.Core;

namespace Server.Services.UOBattleCards.Gumps
{
    public static class GumpUtility
    {
		public const string Footer = "UO Battle Cards © 2023 by Kita & RedRooster";

		public const int HeadHue = Settings.HeaderHue;

		public const int SubHue = Settings.SubjectHue;

        private const double CharOffset = Settings.TextOffset;

		public static int Center(int midPoint, string chars)
        {
            return midPoint - (int)(chars.Length * CharOffset);
		}

		public static int GetCardHighLightHue(bool isFoil)
		{
			if (isFoil)
			{
				return Settings.HighlightFoilHue;
			}
			else
			{
				return Settings.HighlightHue;
			}
		}

		public static void AddHighlight(CreatureInfo card, int width, int height, int highLight, BaseGump gump, bool isFoil)
		{
			var offset = isFoil ? 2 : 1;

			// Cardinal Highlight Offsets
			gump.AddItem(width - offset, height, card.C_ItemID, highLight);
			gump.AddItem(width + offset, height, card.C_ItemID, highLight);
			gump.AddItem(width, height - offset, card.C_ItemID, highLight);
			gump.AddItem(width, height + offset, card.C_ItemID, highLight);

			// Diagonal Highlight Offsets
			gump.AddItem(width - offset, height - offset, card.C_ItemID, highLight);
			gump.AddItem(width - offset, height + offset, card.C_ItemID, highLight);
			gump.AddItem(width + offset, height - offset, card.C_ItemID, highLight);
			gump.AddItem(width + offset, height + offset, card.C_ItemID, highLight);
		}

		public static void AddBlood(int x, int y, CardInfo card, BaseGump gump)
		{
			if (card.Damage > 0)
			{
				var mod = card.GetDefense() / 10;

				if (mod != 0)
				{
					mod = (card.Damage / mod) / 2;
				}

				mod = mod > 0 ? mod : 1;

				for (var i = 0; i < mod; i++)
				{
					gump.AddItem(x + Utility.Random(150) + 23, y + Utility.Random(150) + 53, Settings.GetRandomBlood());
				}

				// Damage #
				gump.AddImage(x + 93, y + 50, 1646, SubHue);

				gump.AddLabel(x + Center(111, card.Damage.ToString()), y + 67, 1460, card.Damage.ToString());
			}
		}

		public static void AddCardFont(BaseGump gump, int x, int y, string cardName, string cardInfo, string txtHue = "FFFFFF", string sdwHue = "000000")
		{
			// Title
			gump.AddHtml(x + 29, y + 14, 168, 25, $"<basefont color=#{sdwHue}><Center>{cardName}</Center>", false, false);

			gump.AddHtml(x + 30, y + 15, 168, 25, $"<basefont color=#{txtHue}><Center>{cardName}</Center>", false, false);

			// Info
			gump.AddHtml(x + 29, y + 244, 167, 75, $"<basefont color=#{sdwHue}><Center>{cardInfo}</Center>", false, false);

			gump.AddHtml(x + 30, y + 245, 167, 75, $"<basefont color=#{txtHue}><Center>{cardInfo}</Center>", false, false);
		}

		public static void LoadCardGump(MatchInfo info, PlayerMobile user, CardInfo card, BaseGump gump, int x, int y)
        {
            if (info == null)
            {
                return;
            }

            LoadCardGump(user, card, gump, x, y);
        }

		public static void LoadCardGump(PlayerMobile user, CardInfo card, BaseGump gump, int x, int y)
        {
            if (user == null)
            {
                return;
            }

            CreatureInfo creature = null;

            if (card.Card != null)
            {
                creature = CreatureUtility.GetInfo(card.Creature);

                if (creature == null)
                {
                    return;
                }

                try
                {
                    user.PlaySound(creature.C_IdleSound);
                }
                catch
                {
                    // do nothing, bad sound!
                }
            }

            // Foil BG
            if (card.IsFoil)
            {
                gump.AddImage(x, y, Settings.CardFoil);
            }

            // Biome BG
            var hue = card.IsFoil ? Settings.EtherealHue : card.CardType == CardTypes.Creature ? SubHue : 1174;

            var biome = CardUtility.GetCardBiome(user, card, creature.C_IsWater);

            gump.AddImage(x, y, Settings.GetBiomeArt(biome), hue);

            // Damaged? add blood!
            AddBlood(x, y, card, gump);

            // Setup
            var cardName = card.Name;

            var cardInfo = card.Description;

            // Get Art Data
            var widthMod = (x + 112) - (creature.C_Width / 2);

            var heightMod = (y + 145) - (creature.C_Height / 2);

            // Highlight
            var highLight = GetCardHighLightHue(card.IsFoil);

            AddHighlight(creature, widthMod, heightMod, highLight, gump, card.IsFoil);

            // Creature Art
            gump.AddItem(widthMod, heightMod, creature.C_ItemID, creature.C_Hue);

            // Add Card Face
            gump.AddImage(x, y, Settings.CardFace, card.IsFoil ? Settings.EtherealHue : card.CardType == CardTypes.Creature ? 0 : card.Card.Hue);

            // Title Button
            if (!MatchUtility.InMatch(user) && card.Card?.RootParent != user && card.Card != null)
            {
                var up = card.IsDisplayed ? Settings.TitleGemDown : Settings.TitleGemUp;

                var down = card.IsDisplayed ? Settings.TitleGemUp : Settings.TitleGemDown;

                gump.AddButton(x + 15, y + 15, up, down, 1, GumpButtonType.Reply, 0);
            }
            else
            {
                gump.AddImage(x + 15, y + 15, Settings.TitleGemUp);
            }

            // Add Title/Info
            var txtHue = Settings.GetFontColor(card.IsFoil);

            var shadowHue = Settings.Shadow;

            AddCardFont(gump, x, y, cardName, cardInfo, txtHue, shadowHue);

            // Slot Button
            if (!MatchUtility.InMatch(user) && card.Card != null)
            {
                // Str
                if (card.HasGem(StatCode.Str))
                {
                    gump.AddButton(x + 40, y + 200, Settings.GetGemUp(card.StrSlotGem), Settings.GetGemDown(card.StrSlotGem), 2, GumpButtonType.Reply, 0);
                }
                else
                {
                    gump.AddImage(x + 40, y + 200, Settings.EmptySlotUp);
                }

                // Int
                if (card.HasGem(StatCode.Int))
                {
                    gump.AddButton(x + 101, y + 200, Settings.GetGemUp(card.IntSlotGem), Settings.GetGemDown(card.IntSlotGem), 3, GumpButtonType.Reply, 0);
                }
                else
                {
                    gump.AddImage(x + 101, y + 200, Settings.EmptySlotUp);
                }

                // Dex
                if (card.HasGem(StatCode.Dex))
                {
                    gump.AddButton(x + 160, y + 200, Settings.GetGemUp(card.DexSlotGem), Settings.GetGemDown(card.DexSlotGem), 4, GumpButtonType.Reply, 0);
                }
                else
                {
                    gump.AddImage(x + 160, y + 200, Settings.EmptySlotUp);
                }
            }
            else
            {
                if (card.CardType == CardTypes.Creature)
                {
                    // Str
                    if (card.HasGem(StatCode.Str))
                    {
                        gump.AddImage(x + 40, y + 200, Settings.GetGemUp(card.StrSlotGem));
                    }
                    else
                    {
                        gump.AddImage(x + 40, y + 200, Settings.EmptySlotUp);
                    }

                    // Int
                    if (card.HasGem(StatCode.Int))
                    {
                        gump.AddImage(x + 101, y + 200, Settings.GetGemUp(card.IntSlotGem));
                    }
                    else
                    {
                        gump.AddImage(x + 101, y + 200, Settings.EmptySlotUp);
                    }

                    // Dex
                    if (card.HasGem(StatCode.Dex))
                    {
                        gump.AddImage(x + 160, y + 200, Settings.GetGemUp(card.DexSlotGem));
                    }
                    else
                    {
                        gump.AddImage(x + 160, y + 200, Settings.EmptySlotUp);
                    }
                }
            }
        }
    }
}

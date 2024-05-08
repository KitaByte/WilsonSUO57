using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Services.UOBattleCards.Cards;
using Server.Services.UOBattleCards.Core;

namespace Server.Services.UOBattleCards.Gumps
{
	public class BaseCardGump : BaseGump
    {
        private CardInfo Info { get; set; }

        public override bool CloseOnMapChange => true;

        public BaseCardGump(PlayerMobile user, CardInfo info, int x = 0, int y = 0) : base(user, x, y)
        {
            Info = info;
        }

        public BaseCardGump(PlayerMobile user, CardInfo info, int x, int y, BaseGump parent = null) : base(user, x, y, parent)
        {
            Info = info;
        }

        public override void AddGumpLayout()
        {
            // Card Size = 225 x 326 => 168 x 168 (Inner) : 61 (Header) : 29 (Margins) : 97 (Footer)

            CardLayout(User, Info, X, Y);
        }

        public void CardLayout(PlayerMobile user, CardInfo card, int X, int Y)
        {
            if (card != null)
            {
                if (card.IsRevealed)
                {
                    switch (card.CardType)
                    {
                        case CardTypes.Creature: LoadCreatureCard(card, user, X, Y); break;
                        case CardTypes.Skill: LoadSupportCard(card, user, X, Y, CardTypes.Skill); break;
                        case CardTypes.Special: LoadSupportCard(card, user, X, Y, CardTypes.Special); break;
                        case CardTypes.Spell: LoadSupportCard(card, user, X, Y, CardTypes.Spell); break;
                        case CardTypes.Trap: LoadSupportCard(card, user, X, Y, CardTypes.Trap); break;
                    }
                }
                else
                {
                    // Add Card Back
                    if (MatchUtility.InMatch(user))
                    {
                        AddImage(X, Y, Settings.CardBack);
                    }
                    else
                    {
                        AddButton(X, Y, Settings.CardBack, Settings.CardBack, 0, GumpButtonType.Reply, 0);
                    }
                }
            }
        }

        private void LoadCreatureCard(CardInfo card, PlayerMobile user, int x, int y)
        {
            GumpUtility.LoadCardGump(user, card, this, x, y);
        }

        private void LoadSupportCard(CardInfo skillCard, PlayerMobile user, int x, int y, CardTypes skillType)
        {
            // Biome BG
            AddImage(x, y, Settings.GetBiomeArt(CardUtility.GetCardBiome(user, skillCard, false)));

            // Setup
            var cardName = skillCard.Name;

            var cardInfo = skillCard.Description;

            // Add Card Face
            AddImage(x, y, Settings.CardFace, skillCard.Hue - 1);

            // Add Title/Info
            GumpUtility.AddCardFont(this, x, y, cardName, cardInfo);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (MatchUtility.InMatch(User))
            {
                return;
            }

            if (Info.CardType == CardTypes.Creature)
            {
                switch (info.ButtonID)
                {
                    case 0:
                        {
                            if (!Info.IsRevealed)
                            {
                                Info.UpdateIsRevealed();

                                Refresh(true, false);
                            }

                            break;
                        }
                    case 1: // Display
                        {
                            Info.SetDisplay();

                            Refresh(true, false);

                            break;
                        }
                    case 2: // Str Gem Slot
                        {
                            if (Info.StrSlotGem != GemType.None)
                            {
                                User.SendMessage("Removing Str Gem");

                                Info.RemoveGem(StatCode.Str);
                            }

                            Refresh(true, false);

                            break;
                        }
                    case 3: // Int Gem Slot
                        {
                            if (Info.IntSlotGem != GemType.None)
                            {
                                User.SendMessage("Removing Int Gem");

                                Info.RemoveGem(StatCode.Int);
                            }

                            Refresh(true, false);

                            break;
                        }
                    case 4: // Dex Gem Slot
                        {
                            if (Info.DexSlotGem != GemType.None)
                            {
                                User.SendMessage("Removing Dex Gem");

                                Info.RemoveGem(StatCode.Dex);
                            }

                            Refresh(true, false);

                            break;
                        }
                }
            }
            else
            {
                if (info.ButtonID == 0)
                {
                    if (!Info.IsRevealed)
                    {
                        Info.UpdateIsRevealed();

                        Refresh(true, false);
                    }
                }
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;

using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Services.UOBattleCards.Cards;
using Server.Services.UOBattleCards.Core;
using Server.Services.UOBattleCards.Items;

namespace Server.Services.UOBattleCards.Gumps
{
	public class CollectionGump : BaseGump
    {
        private CollectionBook Book { get; set; }

        private bool IsHomePage { get; set; }

        private int CurrentCard { get; set; }

        private int CurrentRarity { get; set; }

        private Dictionary<int, List<CardStore>> CardsByRarity { get; set; }

        private const int CardWidth = 225;

        private const int CardHeight = 326;

        public override bool CloseOnMapChange => true;

        public CollectionGump(PlayerMobile user, CollectionBook book, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)
        {
            Book = book;

            if (parent == null)
            {
                IsHomePage = true;
            }
        }

        public override void AddGumpLayout()
        {
            // Card Size = 225 x 326

            cardInfos = new List<CardStore>();

            LoadCards();

            AddBackground(X, Y, 980, 905, 39925);

            if (IsHomePage)
            {
                AddHomePage();
            }
            else
            {
                AddCardPage();
            }

            AddLabel(X + GumpUtility.Center(490, GumpUtility.Footer), Y + 875, GumpUtility.SubHue, GumpUtility.Footer); // Footer
        }

        private void LoadCards()
        {
            if (CardsByRarity == null)
            {
                CardsByRarity = new Dictionary<int, List<CardStore>>();
            }
            else
            {
                CardsByRarity.Clear();
            }

            Book.CheckStorage();

            if (Book.CardStorage.Count > 0)
            {
                foreach (var cards in Book.CardStorage)
                {
                    CardsByRarity.TryGetValue(cards.Rarity, out List<CardStore> currentList);

                    currentList = currentList ?? new List<CardStore>();

                    currentList?.Add(cards);

                    CardsByRarity[cards.Rarity] = currentList.OrderByDescending(c => c.IsFoil).ThenBy(c => c.Name).ToList();
                }
            }
        }

        private void AddHomePage()
        {
            // home page layout

            AddLabel(X + GumpUtility.Center(490, $"UO Battle Cards : {Book.CardCount()}"), Y + 20, GumpUtility.HeadHue, $"UO Battle Cards : {Book.CardCount()}");

            AddButton(X + 480, Y + 50, Settings.EmptySlotUp, Settings.EmptySlotDown, 1, GumpButtonType.Reply, 0);

            AddImage(X + 203, Y + 80, Settings.MatchArt);

            AddImage(X + 155, Y + 20, 10440, GumpUtility.HeadHue);

            AddImage(X + 745, Y + 20, 10441, GumpUtility.HeadHue);

            // Rarity Quick Selection

            for (int i = 1; i < 11; i++)
            {
                AddRarityButton(X + 25, X + 40, X + 49, 96, i);
            }

            // Card Quest

            AddBackground(X + 203, Y + 720, 574, 135, 40000);

            var title = "Card Collection Quest";

            AddLabel(X + GumpUtility.Center(490, title), Y + 730, GumpUtility.HeadHue, title);

            var chance = CreatureUtility.FameChance(CreatureUtility.GetInfo(Book.NextCard).C_Fame);

            var msg = $"Add this card to your collection for a {chance}% chance at a random reward card!";

            AddLabel(X + GumpUtility.Center(490,msg), Y + 770, GumpUtility.SubHue, msg);

            AddLabel(X + GumpUtility.Center(490, Book.NextCard), Y + 810, GumpUtility.HeadHue, Book.NextCard);
        }

        private void AddRarityButton(int x, int xx, int xxx, int mod, int rarity)
        {
            AddLabel(x + (rarity - 1) * mod, Y + 610, GumpUtility.SubHue, $"Rarity {rarity}");

            AddButton(xx + (rarity - 1) * mod, Y + 635, Settings.GetGemUp((GemType)rarity), Settings.GetGemDown((GemType)rarity), rarity + 5, GumpButtonType.Reply, 0);

            var book = Book.CardRarityCount(rarity).ToString();

            AddLabel(xxx + GumpUtility.Center((rarity - 1) * mod, book), Y + 655, GumpUtility.HeadHue, book);
        }

        private void AddCardPage()
        {
            // Rarity

            AddImage(X + 435, Y + 20, 2062); // BG Bar

            AddButton(X + 425, Y + 20, 5840, 5839, 2, GumpButtonType.Reply, 0); // Prev

            AddLabel(X + GumpUtility.Center(490, $"Rarity {CurrentRarity}"), Y + 20, GumpUtility.HeadHue, $"Rarity {CurrentRarity}");

            AddButton(X + 535, Y + 20, 5838, 5837, 3, GumpButtonType.Reply, 0); // Next

            CardsByRarity.TryGetValue(CurrentRarity, out List<CardStore> store);

            if (store == null)
            {
                store = new List<CardStore>();
            }

            // Top Row

            AddCardArt(store, X + 25, Y + 50);

            AddCardArt(store, X + CardWidth + 35, Y + 50);

            AddCardArt(store, X + (CardWidth * 2) + 45, Y + 50);

            AddCardArt(store, X + (CardWidth * 3) + 55, Y + 50);

            // Bottom Row

            AddCardArt(store, X + 25, Y + CardHeight + 120);

            AddCardArt(store, X + CardWidth + 35, Y + CardHeight + 120);

            AddCardArt(store, X + (CardWidth * 2) + 45, Y + CardHeight + 120);

            AddCardArt(store, X + (CardWidth * 3) + 55, Y + CardHeight + 120);

            // Page

            AddImage(X + 435, Y + 825, 2062); // BG Bar

            if (CurrentCard > 8) // Prev
            {
                AddButton(X + 425, Y + 825, 5840, 5839, 4, GumpButtonType.Reply, 0);
            }

            int page = CurrentCard == 0 ? 1 : CurrentCard / 8;

            int pageMax = store.Count == 0 ? 1 : store.Count / 8 + 1;

            AddLabel(X + GumpUtility.Center(490, $"Page {page}/{pageMax}"), Y + 825, GumpUtility.HeadHue, $"Page {page}/{pageMax}");

            if (CurrentCard < store.Count) // Next
            {
                AddButton(X + 535, Y + 825, 5838, 5837, 5, GumpButtonType.Reply, 0);
            }

            AddButton(X + 482, Y + 850, 2118, 2117, 16, GumpButtonType.Reply, 0); // Home
        }

        private void AddCardArt(List<CardStore> store, int x, int y)
        {
            if (CurrentCard > -1 && CurrentCard < store?.Count && store[CurrentCard]?.Cards?.Count > -1)
            {
                if (store[CurrentCard]?.GetCurrentCard() != null)
                {
                    AddCardGump(store[CurrentCard].GetCurrentCard(), x, y, store[CurrentCard]);
                }
                else
                {
                    AddImage(x, y, Settings.CardBack);
                }
            }
            else
            {
                AddImage(x, y, Settings.CardBack);
            }

            if (CurrentCard == 0)
            {
                CurrentCard = 1;
            }
            else
            {
                CurrentCard++;
            }
        }

        private List<CardStore> cardInfos;

        public void AddCardGump(CardInfo card, int x, int y, CardStore store)
        {
            GumpUtility.LoadCardGump(User, card, this, x, y);

            // card button
            AddImage(x + 49, y + 343, 2054);

            int buttonMod;

            if (cardInfos.Count < 9)
            {
                buttonMod = cardInfos.Count * 3;

                store.currentSlot = cardInfos.Count;
            }
            else
            {
                buttonMod = store.currentSlot;
            }

            var cardMod = $"{store.currentPos + 1}/{store.Cards.Count}";

            AddLabel(x + GumpUtility.Center(113, cardMod), y + 325, GumpUtility.SubHue, cardMod); // Total

            AddButton(x + 47, y + 340, 5840, 5839, 17 + buttonMod, GumpButtonType.Reply, 0); //Prev

            AddButton(x + 102, y + 343, 2095, 2094, 18 + buttonMod, GumpButtonType.Reply, 0); //Remove

            AddButton(x + 153, y + 340, 5838, 5837, 19 + buttonMod, GumpButtonType.Reply, 0); //Next

            cardInfos.Add(store);
        }

        public override void OnResponse(RelayInfo info)
        {
            IsHomePage = false;

            var pos = 0;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        Close();

                        break;
                    }
                case 1:
                    {
                        CurrentCard = 0; CurrentRarity = 1;

                        Refresh(true, false);

                        break;
                    } 
                case 2:
                    {
                        CurrentCard = 0; CurrentRarity--;

                        if (CurrentRarity < 1)
                        {
                            CurrentRarity = 0;

                            IsHomePage = true;
                        }

                        Refresh(true, false);

                        break;
                    }
                case 3:
                    {
                        CurrentCard = 0; CurrentRarity++;

                        if (CurrentRarity > 10)
                        {
                            CurrentRarity = 0;

                            IsHomePage = true;
                        }

                        Refresh(true, false);

                        break;
                    }
                case 4:
                    {
                        CurrentCard = CurrentCard - 16;

                        if (CurrentCard < 0)
                        {
                            CurrentCard = 0;
                        }

                        Refresh(true, false);

                        break;
                    }
                case 5:
                    {
                        Refresh(true, false);

                        break;
                    }
                case 6:  // Rarity 1
                case 7:  // Rarity 2
                case 8:  // Rarity 3
                case 9:  // Rarity 4
                case 10: // Rarity 5
                case 11: // Rarity 6
                case 12: // Rarity 7
                case 13: // Rarity 8
                case 14: // Rarity 9
                case 15: // Rarity 10
                    {
                        CurrentRarity = info.ButtonID - 5;

                        Refresh(true, false);

                        break;
                    }
                case 16: // Rarity 10
                    {
                        CurrentCard = 0; CurrentRarity = 0; IsHomePage = true;

                        Refresh(true, false);

                        break;
                    }
                case 17: pos = 0; goto case 38;
                case 18: pos = 0; goto case 39;
                case 19: pos = 0; goto case 40;
                case 20: pos = 1; goto case 38;
                case 21: pos = 1; goto case 39;
                case 22: pos = 1; goto case 40;
                case 23: pos = 2; goto case 38;
                case 24: pos = 2; goto case 39;
                case 25: pos = 2; goto case 40;
                case 26: pos = 3; goto case 38;
                case 27: pos = 3; goto case 39;
                case 28: pos = 3; goto case 40;
                case 29: pos = 4; goto case 38;
                case 30: pos = 4; goto case 39;
                case 31: pos = 4; goto case 40;
                case 32: pos = 5; goto case 38;
                case 33: pos = 5; goto case 39;
                case 34: pos = 5; goto case 40;
                case 35: pos = 6; goto case 38;
                case 36: pos = 6; goto case 39;
                case 37: pos = 6; goto case 40;
                case 38: 
                    {
                        if (info.ButtonID == 38)
                        {
                            pos = 7;
                        }

                        cardInfos[pos].GetPrevCard();

                        CurrentCard = CurrentCard - 8;

                        Refresh(true, false);

                        break;
                    }
                case 39:
                    {
                        if (info.ButtonID == 39)
                        {
                            pos = 7;
                        }

                        var card = cardInfos[pos].GetCurrentCard();

                        if (card != null)
                        {
                            Book.RemoveCard(card);

                            cardInfos[pos].currentPos = 0;
                        }

                        CurrentCard = CurrentCard - 8;

                        Refresh(true, false);

                        break;
                    }
                case 40:
                    {
                        if (info.ButtonID == 40)
                        {
                            pos = 7;
                        }

                        cardInfos[pos].GetNextCard();

                        CurrentCard = CurrentCard - 8;

                        Refresh(true, false);

                        break;
                    }
                default:
                    {
                        Close();

                        break;
                    }
            }
        }

        public override void OnClosed()
        {
            Book.Gump = null;

            base.OnClosed();
        }
    }
}

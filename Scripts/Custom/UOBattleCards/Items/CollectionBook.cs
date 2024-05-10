using System.Collections.Generic;

using Server.Gumps;
using Server.Mobiles;
using Server.Services.UOBattleCards.Cards;
using Server.Services.UOBattleCards.Cards.Types;
using Server.Services.UOBattleCards.Core;
using Server.Services.UOBattleCards.Gumps;

namespace Server.Services.UOBattleCards.Items
{
	public class CollectionBook : Item
    {
        private PlayerMobile Owner { get; set; }

        public List<CardStore> CardStorage { get; set; }

        public BaseGump Gump { get; set; }

        public override int ItemID => 0x1C11;

        public string NextCard { get; set; }

        [Constructable]
        public CollectionBook()
        {
            Name = "UO Battle Card Book";

            Hue = 2500;

            Weight = 1.0;

            LootType = LootType.Blessed;

            NextCard = CreatureUtility.GetRandomCreature();

            CheckStorage();
        }

        public void CheckStorage()
        {
            if (CardStorage == null)
            {
                CardStorage = new List<CardStore>();
            }
        }

        public CollectionBook(Serial serial) : base(serial)
        {
        }

        public override bool VerifyMove(Mobile from)
        {
			Owner = CoreUtility.AntiTheftCheck(from, Owner, this);

            if (from == Owner || from.AccessLevel != AccessLevel.Player)
            {
                if (from.HasGump(typeof(CollectionGump)))
                {
                    from.CloseGump(typeof(CollectionGump));
                }
            }

            Gump = null;

            return base.VerifyMove(from);   
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (Owner == null && from is PlayerMobile pm)
            {
                Owner = pm;
            }

            if (Gump == null)
            {
                Gump = new CollectionGump(Owner, this);

                BaseGump.SendGump(Gump);
            }
        }

        public int CardCount()
        {
            var total = 0;

            if (CardStorage?.Count > 0)
            {
                foreach (var store in CardStorage)
                {
                    if (store.Cards?.Count > 0)
                    {
                        total += store.Cards.Count;
                    }
                }
            }

            return total;
        }

        internal int CardRarityCount(int rarity)
        {
            var total = 0;

            if (CardStorage?.Count > 0)
            {
                foreach (var store in CardStorage)
                {
                    if (store.Cards?.Count > 0)
                    {
                        if (store.Cards[0].GetRarity() == rarity)
                        {
                            total += store.Cards.Count;
                        }
                    }
                }
            }

            return total;
        }

        public void AddCard(CardInfo card)
        {
            CheckStorage();

            if (!card.IsRevealed)
            {
                card.UpdateIsRevealed();
            }

            var store = CardStorage.Find(i => i.Name == card.Name && i.Rarity == card.GetRarity() && i.IsFoil == card.IsFoil);

            if (store != null)
            {
                if (store.Cards.Count + 1 > 998)
                {
                    Owner.SendMessage(32, "Only allowed 999 of one card type in book!");

                    Owner.AddToBackpack(card.Card);

                    return;
                }
                else
                {
                    card.Card?.Delete();

                    card.Card = null;

                    store.Cards.Add(card);
                }
            }
            else
            {
                card.Card?.Delete();

                card.Card = null;

                var cardStore = new CardStore(card.Name, card.GetRarity(), card.IsFoil);

                cardStore.Cards.Add(card);

                CardStorage.Add(cardStore);
            }

            if (card.Creature == NextCard && Utility.Random(100) < CreatureUtility.FameChance(CreatureUtility.GetInfo(card.Creature).C_Fame))
            {
                var giftCard = new CreatureCard(CreatureUtility.GetRandomCreature());

                giftCard.Info.Owner = Owner;

                Owner.AddToBackpack(giftCard);
            }

            NextCard = CreatureUtility.GetRandomCreature();
        }

        public void RemoveCard(CardInfo card)
        {
            CheckStorage();

            var store = CardStorage.Find(i => i.Name == card.Name && i.Rarity == card.GetRarity() && i.IsFoil == card.IsFoil);

            if (store != null)
            {
                if (card != null)
                {
                    var newCard = new CreatureCard(card);

                    newCard.Info.Card = newCard;

                    Owner.AddToBackpack(newCard);

                    store.Cards.Remove(card);

                    if (store.Cards.Count == 0)
                    {
                        CardStorage.Remove(store);
                    }
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(Owner);

            var totalCards = CardCount();

            writer.Write(totalCards);

            if (totalCards > 0)
            {
                foreach (var store in CardStorage)
                {
                    if (store.Cards?.Count > 0)
                    {
                        foreach (var card in store.Cards)
                        {
                            card.CardSerialize(writer);
                        }
                    }
                }
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Owner = reader.ReadMobile() as PlayerMobile;

            CardStorage = new List<CardStore>();

            var cardCount = reader.ReadInt();

            if (cardCount > 0)
            {
                for (var i = 0; i < cardCount; i++)
                {
                    var card = new CardInfo();

                    card.CardDeserialize(reader);

                    AddCard(card);
                }
            }

            NextCard = CreatureUtility.GetRandomCreature();
        }
    }

	public class CardStore
    {
		public string Name { get; set; }

		public int Rarity { get; set; }

		public bool IsFoil { get; set; }

		public List<CardInfo> Cards { get; set; }

        public CardStore(string name, int rarity, bool isFoil = false)
        {
            Name = name;

            Rarity = rarity;

            IsFoil = IsFoil;

            Cards = new List<CardInfo>();
        }

		public int currentSlot = 0;

		public int currentPos = 0;

        public CardInfo GetCurrentCard()
        {
            if (Cards.Count > currentPos && Cards[currentPos] != null)
            {
                return Cards[currentPos];
            }

            currentPos = 0;

            return null;
        }

        public int GetNextCard()
        {
            if (Cards.Count > currentPos + 1 && Cards[currentPos] != null)
            {
                currentPos++;

                if (currentPos > Cards.Count - 1)
                {
                    currentPos = 0;
                }

                return currentPos;
            }

            return currentPos;
        }

        public int GetPrevCard()
        {
            if (currentPos > -1 && Cards[currentPos] != null)
            {
                currentPos--;

                if (currentPos < 0)
                {
                    currentPos = Cards.Count - 1;
                }

                return currentPos;
            }

            return currentPos;
        }
    }
}

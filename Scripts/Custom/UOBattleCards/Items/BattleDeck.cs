using System;
using System.Linq;
using System.Collections.Generic;

using Server.Services.UOBattleCards.Cards.Types;
using Server.Services.UOBattleCards.Cards;
using Server.Services.UOBattleCards.Gumps;
using Server.Services.UOBattleCards.Core;
using Server.ContextMenus;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;

namespace Server.Services.UOBattleCards.Items
{
	public class BattleDeck : Backpack
	{
		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner { get; set; }

		public List<BaseCard> CardDeck { get; set; }

		public List<BaseCard> AIDeck { get; set; }

		public override bool DisplayWeight => false;

		public override double DefaultWeight => 0;

		public override int DefaultGumpID => 0x266B;

		[Constructable]
		public BattleDeck()
		{
			ItemID = Utility.RandomList( 0x12AB, 0x12AC);

			Name = "Battle Deck";

			Hue = 2500;

			Weight = 0;

			MaxItems = 21; // 20 creatures & 1 Leaderboard

			AddItem(new CardLeaderIcon());
		}

		public BattleDeck(Serial serial) : base(serial)
		{
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
            if (Owner != null && Owner == from && Owner == RootParent && !MatchUtility.InMatch(from))
            {
                if (Items.Count == MaxItems)
                {
                    Owner.SendMessage(52, "Opening Match Setup");

                    Owner.CloseAllGumps();

                    BaseGump.SendGump(new MatchMakeGump(Owner as PlayerMobile, this));
                }
                else
                {
                    Owner?.SendMessage(32, "Deck must have 20 card before competing");
                }
            }
		}

		public override bool VerifyMove(Mobile from)
		{
			Owner = CoreUtility.AntiTheftCheck(from, Owner, this);

			return base.VerifyMove(from);
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (RootParent == from)
			{
				if (MatchUtility.InMatch(from, true))
					return;
				
				if (Owner == from)
				{
					from.SendMessage(52, "Deck Info");
					from.SendMessage(42, $"{Items.Count - 1} Creature Cards");
					from.SendMessage(62, $"{GetDeckValue()} Deck Value");

                    if (Items.Count == MaxItems)
                    {
                        from.SendMessage(52, "Deck ready for matches, single click deck to start match!");
                    }

					base.OnDoubleClick(from);
				}
				else
				{
					CoreUtility.AntiTheftCheck(from, Owner, this);
				}
			}
		}

		public override void Open(Mobile from)
		{
			if (from.AccessLevel > AccessLevel.Player && RootParent != from)
			{
				from.SendMessage(52, "You peek into the deck!");
			}

			if (!MatchUtility.InMatch(from, true))
			{
				from.SendMessage(52, "Opening your Battle Deck!");

				base.Open(from);
			}
		}

		public override void AddItem(Item item)
		{
			if (item is CardLeaderIcon && Items.FindAll(i => i is CardLeaderIcon).Count == 0)
			{
				base.AddItem(item);

				item.X = 150;

				item.Y = 75;

				return;
			}

			if (item is CreatureCard cc && Owner != null && cc.Info.IsRevealed)
			{
				if (Items.Count < MaxItems && cc.Info.Damage == 0)
				{
					base.AddItem(cc);

					if (Items.Count > 0)
					{
						UpdateItemPosition();
					}

					return;
				}

				Owner.SendMessage(32, cc.Info.Damage == 0 ? "You already have 20 max creature cards!" : "No damaged cards allowed in deck!");

				Owner.AddToBackpack(item);
			}
			else
			{
				Owner?.SendMessage(32, "That is not a revealed battle card!");

				Owner?.AddToBackpack(item);
			}
		}


		private void UpdateItemPosition()
		{
			if (Items[0] is CardLeaderIcon)
			{
				// All good do nothing!
			}
			else
			{
				var getIcon = Items.Find(l => l is CardLeaderIcon);

				if (getIcon != null)
				{
					Items.Remove(getIcon);

					Items.Insert(0, getIcon);
				}
				else
				{
					Items.Insert(0, new CardLeaderIcon());
				}
			}

			var itemWidth = 5;

			for (var i = 1; i < Items.Count(); i++)
			{
				if (Items[i] is CreatureCard)
				{
					Items[i].X = 20 + (i * itemWidth); 
					Items[i].Y = 85; 
				}
			}
        }

        public bool ValidateDeck(MatchMakeInfo info)
        {
            var validDeck = ValidateFoil(info.FoilAllowed);

            if (validDeck)
                validDeck = ValidateRarity(info.MaxRarity);

            if (validDeck)
                validDeck = ValidateLevel(info.MaxLevel);

            if (validDeck)
                validDeck = ValidateValue(info.ValueCap);

            return validDeck;
        }

        private bool ValidateFoil(bool isAllowed)
        {
            var isValid = true;

            var cards = Items.FindAll(item => item is CreatureCard).Cast<CreatureCard>();

            if (cards != null && cards.Count() > 0)
            {
                foreach (var card in cards)
                {
                    if (card.Info.IsFoil && !isAllowed)
                    {
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private bool ValidateRarity(int rarity)
        {
            var isValid = true;

            var cards = Items.FindAll(item => item is CreatureCard).Cast<CreatureCard>();

            if (cards != null && cards.Count() > 0)
            {
                foreach (var card in cards)
                {
                    if (card.Info.GetRarity() > rarity)
                    {
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private bool ValidateLevel(int level)
        {
            var isValid = true;

            var cards = Items.FindAll(item => item is CreatureCard).Cast<CreatureCard>();

            if (cards != null && cards.Count() > 0)
            {
                foreach (var card in cards)
                {
                    if (card.Info.GetLevel() > level)
                    {
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        private bool ValidateValue(int value)
        {
            var isValid = true;

            if (value != 0 && GetDeckValue() > value)
            {
                isValid = false;
            }

            return isValid;
        }

        public int GetDeckValue()
		{
			var cards = Items.FindAll(item => item is CreatureCard).Cast<CreatureCard>();

			var total = 0;

            if (cards != null && cards.Count() > 0)
            {
                foreach (var card in cards)
                {
                    total += card.Info.GetValue();
                }
            }

			return total;
		}

		public void ShuffleDeck(bool isAI)
		{
			if (isAI)
			{
                var aiCards = new List<BaseCard>();

                foreach (var item in Items)
                {
                    if (item is BaseCard bc)
                    {
                        var newCreatureCopy = new CreatureCard(bc.Info.Creature);

                        newCreatureCopy.Info.IsRevealed = true;

                        aiCards.Add(newCreatureCopy);
                    }
                }

				for (var i = 0; i < 12; i++)
				{
                    aiCards.Add(CardUtility.GetSupportCard());
				}

				var suffledCards = aiCards.OrderBy(x => Guid.NewGuid());

				AIDeck = new List<BaseCard>();

				foreach (var aiCard in suffledCards)
				{
					AIDeck.Add(aiCard);
				}
			}
			else
			{
				var cards = new List<BaseCard>();

                foreach (var item in Items)
                {
                    if (item is BaseCard bc)
                    {
                        cards.Add(bc);
                    }
                }

                for (var i = 0; i < 12; i++)
				{
					cards.Add(CardUtility.GetSupportCard());
				}

				var suffledCards = cards.OrderBy(x => Guid.NewGuid());

				CardDeck = new List<BaseCard>();

				foreach (var card in suffledCards)
				{
					CardDeck.Add(card);
				}
			}
		}

		public BaseCard DrawCard(int position, bool isAI)
		{
			position--;

			if (isAI)
			{
				if (AIDeck.Count == 32 && position > -1 && position < 32)
				{
					return AIDeck[position];
				}
			}
			else
			{
				if (CardDeck.Count == 32 && position > -1 && position < 32)
				{
					return CardDeck[position];
				}
			}

			return null;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);

			writer.Write(Owner);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();

			Owner = reader.ReadMobile();
		}
	}
}

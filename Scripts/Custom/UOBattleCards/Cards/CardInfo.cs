using Server.Items;
using Server.Mobiles;
using Server.Services.BattleCard.Card;
using Server.Services.UOBattleCard.Items;
using Server.Services.UOBattleCards.Core;
using Server.Services.UOBattleCards.Gumps;
using Server.Services.UOBattleCards.Items;

namespace Server.Services.UOBattleCards.Cards
{
    public class CardInfo
    {
		// Card Owner
		public PlayerMobile Owner { get; set; }

		// Card Info
		public BaseCard Card { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int Hue { get; set; }

		// Creature Info
		public string Creature { get; set; }

        // Gump Handle
        private BaseCardGump CardGump { get; set; }

		public BaseCardGump GetCardGump(int x = 0, int y = 0)
        {
            CardGump = new BaseCardGump(Owner, this, x, y);

            return CardGump;
        }

		public bool HasCardGump()
        {
            return CardGump != null;
        }

		public void CloseCardGump()
        {
            CardGump?.Close();
        }

		// Card Info
		public bool IsRevealed { get; set; }

		public void UpdateIsRevealed()
        {
            if (!IsRevealed && Owner != null)
            {
                IsRevealed = true;

                if (CardType == CardTypes.Creature)
                {
                    IsFoil = IsFoil != false || Utility.Random(10000) <= Settings.FoilChance;

                    Hue = IsFoil ? Settings.FoilHue : Settings.BaseHue;

                    Card.Hue = Hue;
                }
                else
                {
                    IsFoil = false;
                }

                if (!MatchUtility.InMatch(Owner))
                {
                    CardUtility.PlayGlowEffect(Owner, false, Owner.Location, Owner.Map);
                }
            }
        }

        private Item DisplayCard { get; set; }

		public bool IsDisplayed { get; private set; }

		public void SetDisplay()
        {
			if (Owner == null) { return; }

            if (Card.RootParent == Owner) { Owner.SendMessage(32, "Can only be displayed outside your backpack!"); return; }

            if (!InteriorDecorator.InHouse(Owner)) { Owner.SendMessage(32, "Can only be displayed inside your house!"); return; }

            if (IsDisplayed)
            {
                Card.Movable = true;

                Card.Visible = true;

                DisplayCard?.Delete();

                DisplayCard = null;

                Owner.SendMessage("Card is not being displayed!");

                IsDisplayed = false;
            }
            else
            {
                Card.Movable = false;

                Card.Visible = false;

                var creatureInfo = CreatureUtility.GetInfo(Creature);

                DisplayCard = new CardDisplay(this, creatureInfo.C_ItemID)
                {
                    Name = creatureInfo.C_Name,
                    Hue = creatureInfo.C_Hue,
					Movable = false
				};

                DisplayCard.MoveToWorld(Card.Location, Card.Map);

                Owner.SendMessage("Card is being displayed!");

                IsDisplayed = true;
            }

            CardUtility.PlayGlowEffect(Owner, true, Card.Location, Card.Map);
        }

		public CardTypes CardType { get; set; }

		public bool IsFoil { get; set; }

		public int Experience { get; set; }

		public int Damage { get; set; }

		public int GetRarity()
        {
            var creatureInfo = CreatureUtility.GetInfo(Creature);

            if (creatureInfo != null)
            {
                var rarity = creatureInfo.C_Fame / 1000 % 5;

                rarity += creatureInfo.C_Karma / 1000 % 5;

                rarity = AddRarityMod(rarity);

                if (rarity == 0)
                {
                    rarity = 1;
                }
                else if (rarity > 10)
                {
                    rarity = 10;
                }

                return rarity;
            }

            return 1;
        }

		public int GetValue()
        {
            var baseValue = GetRawStats() / 3;

            var rarityBonus = GetRarity() * Settings.CardRarityValue;

            var levelBonus = GetLevel() * rarityBonus;

            return (baseValue + levelBonus) * (IsFoil ? 2 : 1);
        }

		public int GetLevel()
        {
            if (Experience < 10000)     { return 1; }
            if (Experience < 30000)     { return 2; }
            if (Experience < 70000)     { return 3; }
            if (Experience < 150000)    { return 4; }
            if (Experience < 310000)    { return 5; }
            if (Experience < 630000)    { return 6; }
            if (Experience < 1300000)   { return 7; }
            if (Experience < 2350000)   { return 8; }
            if (Experience < 3499999)   { return 9; }
            if (Experience > 3500000)   { return 10; }

            return 1;
        }

		public int GetSlots()
        {
            return GetLevel() / 3;
        }

		public int GetAttack()
        {
            var statBonus = GetRawStats() / 1000 % 10;

            var creatureInfo = CreatureUtility.GetInfo(Creature);

            if (creatureInfo != null)
            {
                return creatureInfo.C_Damage + (GetLevel() * GetRarity()) + (int)statBonus;
            }

            return 1;
        }

		public int GetDefense()
        {
            var statBonus = GetRawStats() / 1000 % 10;

            var creatureInfo = CreatureUtility.GetInfo(Creature);

            if (creatureInfo != null)
            {
                if ((creatureInfo.C_Hits + (GetLevel() * GetRarity()) + (int)statBonus) - Damage > 0)
                {
                    return (creatureInfo.C_Hits + (GetLevel() * GetRarity()) + (int)statBonus) - Damage;
                }
                else
                {
                    return 0;
                }
            }

            return 0;
        }

		// Gem Slots
		public GemType StrSlotGem { get; set; } = GemType.None;
		public int StrSlotMod { get; set; } = 0;
		public GemType IntSlotGem { get; set; } = GemType.None;
		public int IntSlotMod { get; set; } = 0;
		public GemType DexSlotGem { get; set; } = GemType.None;
		public int DexSlotMod { get; set; } = 0;

        private int AddRarityMod(int mod)
        {
            var stats = GetRawStats() / 3;

            if (stats > 49 && stats < 150) { mod++; }
            if (stats > 149 && stats < 250) { mod += 2; }
            if (stats > 249 && stats < 500) { mod += 3; }
            if (stats > 499 && stats < 2500) { mod += 4; }
            if (stats > 2499 && stats < 5000) { mod += 5; }
            if (stats > 4999) { mod += 6; }

            return mod;
        }

        private int GetRawStats()
        {
            var creatureInfo = CreatureUtility.GetInfo(Creature);

            var gemMod = StrSlotMod + IntSlotMod + DexSlotMod;
        
            if (creatureInfo != null)
            {
                return creatureInfo.C_Hits + creatureInfo.C_Stam + creatureInfo.C_Mana + gemMod;
            }

            return 0;
        }

        public int GemCount()
        {
            var count = 0;

            if (StrSlotGem != GemType.None) count++;
            if (DexSlotGem != GemType.None) count++;
            if (IntSlotGem != GemType.None) count++;

            return count;
        }

        public bool HasGem(StatCode stat)
        {
            switch (stat)
            {
                case StatCode.Str: if (StrSlotGem != GemType.None) return true; break;
                case StatCode.Dex: if (DexSlotGem != GemType.None) return true; break;
                case StatCode.Int: if (IntSlotGem != GemType.None) return true; break;
            }

            return false;
        }

        public GemType GetGem(StatCode stat, out int mod)
        {
            switch (stat)
            {
                case StatCode.Str: if (StrSlotGem != GemType.None) { mod = StrSlotMod; return StrSlotGem; } break;
                case StatCode.Dex: if (DexSlotGem != GemType.None) { mod = DexSlotMod; return DexSlotGem; } break;
                case StatCode.Int: if (IntSlotGem != GemType.None) { mod = IntSlotMod; return IntSlotGem; } break;
            }

            mod = 0;

            return GemType.None;
        }

        public void AddGem(CardGem gem)
        {
            switch (gem.Info.Stat)
            {
                case StatCode.Str: { StrSlotGem = gem.Info.Gem; StrSlotMod = gem.Info.Mod; } break;
                case StatCode.Dex: { DexSlotGem = gem.Info.Gem; DexSlotMod = gem.Info.Mod; } break;
                case StatCode.Int: { IntSlotGem = gem.Info.Gem; IntSlotMod = gem.Info.Mod; } break;
            }

            gem.Delete();
        }

        public void RemoveGem(StatCode stat)
        {
            var gem = new GemInfo
            {
                Stat = stat
            };

            switch (stat)
            {
                case StatCode.Str: gem.Gem = StrSlotGem; gem.Mod = StrSlotMod; StrSlotGem = GemType.None; StrSlotMod = 0; break;
                case StatCode.Dex: gem.Gem = DexSlotGem; gem.Mod = DexSlotMod; DexSlotGem = GemType.None; DexSlotMod = 0; break;
                case StatCode.Int: gem.Gem = IntSlotGem; gem.Mod = IntSlotMod; IntSlotGem = GemType.None; IntSlotMod = 0; break;
            }

            Owner.AddToBackpack(new CardGem(gem));
        }

        public void CardSerialize(GenericWriter writer)
        {
            writer.Write(Owner);
            writer.Write(Card);
            writer.Write(Name);
            writer.Write(Description);
            writer.Write(Hue);
            writer.Write(Creature);
            writer.Write(IsRevealed);
            writer.Write(IsDisplayed);
            writer.Write((int)CardType);
            writer.Write(IsFoil);
            writer.Write(Experience);
            writer.Write(Damage);
            writer.Write((int)StrSlotGem);
            writer.Write(StrSlotMod);
            writer.Write((int)IntSlotGem);
            writer.Write(IntSlotMod);
            writer.Write((int)DexSlotGem);
            writer.Write(DexSlotMod);
        }

        public void CardDeserialize(GenericReader reader)
        {
            Owner = reader.ReadMobile() as PlayerMobile;
            Card = reader.ReadItem() as BaseCard;
            Name = reader.ReadString();
            Description = reader.ReadString();
            Hue = reader.ReadInt();
            Creature = reader.ReadString();
            IsRevealed = reader.ReadBool();
            IsDisplayed = reader.ReadBool();
            CardType = (CardTypes)reader.ReadInt();
            IsFoil = reader.ReadBool();
            Experience = reader.ReadInt();
            Damage = reader.ReadInt();
            StrSlotGem = (GemType)reader.ReadInt();
            StrSlotMod = reader.ReadInt();
            IntSlotGem = (GemType)reader.ReadInt();
            IntSlotMod = reader.ReadInt();
            DexSlotGem = (GemType)reader.ReadInt();
            DexSlotMod = reader.ReadInt();
        }
    }
}

using System.Collections.Generic;

using Server.Items;
using Server.Mobiles;
using Server.Services.UOBattleCards.Cards.Types;
using Server.Services.UOBattleCards.Items;

namespace Server.Services.BattleCard
{
	public class BattleDeckVendor : BaseVendor
	{
		private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
		protected override List<SBInfo> SBInfos => m_SBInfos;

		[Constructable]
		public BattleDeckVendor() : base("the battle card supplier")
		{
		}

		public override VendorShoeType ShoeType => Female ? VendorShoeType.Sandals : VendorShoeType.ThighBoots;

		public override void InitOutfit()
		{
			if (Backpack == null)
			{
				SetWearable(new Backpack());
			}

			switch (ShoeType)
			{
				case VendorShoeType.Sandals:	SetWearable(new Sandals(), 2500, 1);	break;

				case VendorShoeType.ThighBoots: SetWearable(new Shoes(), 2500, 1);		break;
			}

			Utility.AssignRandomHair(this, 2500);

			if (Body == 0x191)
			{
				FacialHairItemID = 0;
			}
			else
			{
				Utility.AssignRandomFacialHair(this, 2500);
			}

			if (Body == 0x191)
			{
				switch (Utility.RandomBool())
				{
					case true:	SetWearable(new FloweredDress(), 2500, 1);	break;

					case false: SetWearable(new FancyDress(), 2500, 1);		break;
				}
			}
			else
			{
				switch (Utility.RandomBool())
				{
					case true:	SetWearable(new FancyShirt(), 2500, 1);		break;

					case false: SetWearable(new FormalShirt(), 2500, 1);	break;
				}

				switch (Utility.RandomBool())
				{
					case true:	SetWearable(new LongPants(), 2500, 1);		break;

					case false: SetWearable(new ShortPants(), 2500, 1);	    break;
				}
			}

			SetWearable(new Spellbook(), 2500);

			SetWearable(new FeatheredHat(), 2500);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBBattleCards());
		}

		public BattleDeckVendor(Serial serial) : base(serial)
		{
		}

        private List<PlayerMobile> RecentPlayers = new List<PlayerMobile>();

        public override bool HandlesOnSpeech(Mobile from)
        {
            return base.HandlesOnSpeech(from);  
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            if (e.Mobile is PlayerMobile pm)
            {
                if (e.Speech.ToLower() == "heal")
                {
                    var totDamage = GetDamagedCards(pm);

                    if (totDamage > 0)
                    {
                        Say($"{pm.Name}, I can heal all your cards in your backpack for {totDamage} gold!");

                        pm.SendMessage(62, "Reply with 'Accept Heal' if you wish to proceed!");
                    }
                    else
                    {
                        Say($"{pm.Name}, You have no cards that are in need of healing!");
                    }
                }
                else if (e.Speech.ToLower() == "accept heal")
                {
                    var totDamage = GetDamagedCards(pm);

                    if (totDamage > 0)
                    {
                        var gold = e.Mobile.Backpack.FindItemByType(typeof(Gold));

                        if (gold?.Amount > totDamage)
                        {
                            Say($"Cards healed for {totDamage} gold!");

                            gold.Amount -= totDamage;

                            gold.GetLiftSound(pm);

                            HealCards(pm);

                            Say($"{pm.Name}, Thanks for your business!");
                        }
                        else
                        {
							if (pm.AccessLevel > AccessLevel.Counselor)
							{
								Say($"Cards healed for staff is free of charge!");

								HealCards(pm);

								Say($"{pm.Name}, Thanks for your business!");
							}
							else
							{
								Say($"{pm.Name}, you do not have enough gold for my service!");

								pm.SendMessage(52, $"You need {totDamage} gold!");
							}
                        }
                    }
                }
                else
                {
                    if (RecentPlayers == null)
                    {
                        RecentPlayers = new List<PlayerMobile>();
                    }

                    if (!RecentPlayers.Contains(pm))
                    {
                        Say($"{pm.Name}, I can 'Heal' cards too, just ask!");

                        RecentPlayers.Add(pm);
                    }
                }
            }
        }

        private int GetDamagedCards(PlayerMobile pm)
        {
            var cards = pm.Backpack.Items.FindAll(c => c is CreatureCard);

            var damage = 0;

            for (var i = 0; i < cards?.Count; i++)
            {
                if (cards[i] is CreatureCard cc)
                {
                    if (cc.Info.GetDefense() <= damage)
                    {
                        damage += cc.Info.Damage * 2;
                    }
                    else
                    {
                        damage += cc.Info.Damage;
                    }
                }
            }

            return damage;
        }

        private void HealCards(PlayerMobile pm)
        {
            var cards = pm.Backpack.Items.FindAll(c => c is CreatureCard);

            for (var i = 0; i < cards?.Count; i++)
            {
                if (cards[i] is CreatureCard cc)
                {
                    cc.Info.Damage = 0;
                }
            }
        }

        public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			reader.ReadInt();
		}
	}

	public class SBBattleCards : SBInfo
	{
		private readonly List<GenericBuyInfo> m_BuyInfo = new InternalBuyInfo();


		private readonly IShopSellInfo m_SellInfo = new InternalSellInfo();

		public override IShopSellInfo SellInfo => m_SellInfo;

		public override List<GenericBuyInfo> BuyInfo => m_BuyInfo;

		public class InternalBuyInfo : List<GenericBuyInfo>
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(BattleDeck), 500, 20, 0x12AB, 2500));

				Add(new GenericBuyInfo(typeof(CollectionBook), 2500, 20, 0x1C11, 2500));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
            {
                var totCards = 0;

                foreach (var item in World.Items.Values)
                {
					if (item is CreatureCard)
					{
						totCards++;
					}
				}

				if (totCards == 0)
					totCards = 100;

				if (totCards > 10000)
                {
                    totCards = 10000;
                }
                else
                {
                    totCards = 100 - (totCards / 100) + Utility.Random(150);
                }

                Add(typeof(CreatureCard), totCards);
            }
		}
	}
}

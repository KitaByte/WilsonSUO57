using Server.Services.UOBattleCards.Cards;

namespace Server.Services.BattleCard.Card
{
	public class CardDisplay : Item
	{
		public CardInfo CardHandle { get; set; }

		public CardDisplay()
		{
		}

		public CardDisplay(CardInfo card, int itemID) : base(itemID)
		{
			CardHandle = card;
		}

		public CardDisplay(Serial serial) : base(serial)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from == CardHandle.Owner)
			{
				CardHandle.SetDisplay();
			}
			else
			{
				from.SendMessage(42, $"{CardHandle.Name}");
				from.SendMessage(52, $"Rarity : {CardHandle.GetRarity()}");
				from.SendMessage(52, $"Value : {CardHandle.GetValue()}");
				from.SendMessage(52, $"Level : {CardHandle.GetLevel()}");
				from.SendMessage(52, $"Attck : {CardHandle.GetAttack()}");
				from.SendMessage(52, $"Defense : {CardHandle.GetDefense()}");
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

            CardHandle.CardSerialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

            CardHandle = new CardInfo();

            CardHandle.CardDeserialize(reader);
		}
	}
}

using Server.Services.UOBattleCards.Items;
using Server.ContextMenus;
using Server.Targeting;

namespace Server.Services.UOBattleCards.Cards
{
    public class CardEntry : ContextMenuEntry
    {
        public CardEntry() : base(1114788) // Add 1114788
        {
        }

        public override void OnClick()
        {
            if (Owner.From.CheckAlive())
            {
                if (Owner.Target is BaseCard card)
				{
					Owner.From.Target = new CardTarget(card);
				}
            }
        }

        private class CardTarget : Target
        {
            private readonly BaseCard Card;

            public CardTarget(BaseCard card) : base(0, false, TargetFlags.None)
            {
                Card = card;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is CollectionBook book)
                {
                    if (Card.CheckItemUse(from))
                    {
                        book.AddCard(Card.Info);

                        from.SendMessage(62, $"{Card.Name} added to {book.Name}");

                        if (Card.Info.HasCardGump())
                        {
                            Card.Info.CloseCardGump();
                        }
                    }
                }
                else
                {
                    from.SendMessage(32, $"{targeted.GetType().Name} is not a UO Battle Card Book!");
                }
            }
        }
    }
}

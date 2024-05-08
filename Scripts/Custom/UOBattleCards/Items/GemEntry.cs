using Server.ContextMenus;
using Server.Gumps;
using Server.Services.UOBattleCards.Cards.Types;
using Server.Targeting;

namespace Server.Services.UOBattleCard.Items
{
	public class GemEntry : ContextMenuEntry
	{
		public GemEntry() : base(1079168) //Slot 1079168
		{
		}

		public override void OnClick()
		{
			if (Owner.From.CheckAlive())
			{
				if (Owner.Target is CardGem gem)
					Owner.From.Target = new GemTarget(gem);
			}
		}

		private class GemTarget : Target
		{
			private readonly CardGem m_Gem;

			public GemTarget(CardGem gem) : base(3, false, TargetFlags.None)
			{
				m_Gem = gem;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is CreatureCard card && card.Info.IsRevealed)
				{
					if (from.CheckAlive() && !m_Gem.Deleted && m_Gem.Movable && m_Gem.CheckItemUse(from))
					{
						if (card.Info.GetSlots() > card.Info.GemCount())
						{
							if (!card.Info.HasGem(m_Gem.Info.Stat))
							{
								from.SendMessage(62, $"{m_Gem.Name} slotted!");

								card.Info.AddGem(m_Gem);

                                if (card.Info.HasCardGump())
                                {
                                    card.Info.CloseCardGump();
                                }

                                BaseGump.SendGump(card.Info.GetCardGump());
							}
							else
							{
								from.SendMessage(32, "That has a Gem, remove it first!");
							}
						}
						else
						{
							from.SendMessage(32, "You have no free slots!");
						}
					}
				}
				else
				{
					from.SendMessage(32, "That is not a revealed battle card!");
				}
			}
		}
	}
}

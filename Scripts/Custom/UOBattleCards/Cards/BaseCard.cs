using Server.Gumps;
using Server.Mobiles;
using Server.Services.UOBattleCards.Core;
using System.Collections.Generic;
using Server.ContextMenus;

namespace Server.Services.UOBattleCards.Cards
{
    public class BaseCard : Item
    {
        public CardInfo Info { get; set; }

        public override bool DisplayWeight => false;

        public override double DefaultWeight => 0;

        public override int ItemID => Settings.CardItemId;

        public BaseCard() : base()
        {
        }

        public BaseCard(Serial serial) : base(serial)
        {
        }

        public override bool VerifyMove(Mobile from)
        {
			if (Info != null)
			{
				Info.Owner = CoreUtility.AntiTheftCheck(from, Info.Owner, this);

				Info.CloseCardGump();
			}
			else
			{
				Delete();
			}

            return base.VerifyMove(from);
        }

        public virtual void UpdateCard()
        {
            Info.Card = this;

            GetDescription();
        }

        public virtual void GetDescription()
        {
        }

        public override void OnSecureTrade(Mobile from, Mobile to, Mobile newOwner, bool accepted)
        {
            if (accepted)
            {
                Info.Owner = newOwner as PlayerMobile;
            }

            base.OnSecureTrade(from, to, newOwner, accepted);
        }

        public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, list);

            if (RootParent == from && from.Alive && Movable)
                list.Add(new CardEntry());
        }

        public override void OnDoubleClick(Mobile from)
        {
			if (Info == null)
				Delete();

            if (Info.CardType != CardTypes.Creature)
                return;

            if (MatchUtility.InMatch(from, true))
                return;

            if (Info.Owner == from || from.AccessLevel != AccessLevel.Player)
            {
                BaseGump.SendGump(Info.GetCardGump());

                base.OnDoubleClick(from);
            }
            else
            {
				Info.Owner = CoreUtility.AntiTheftCheck(from, Info.Owner, this);
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            Info.CardSerialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Info = new CardInfo();

            Info.CardDeserialize(reader);
        }
    }
}

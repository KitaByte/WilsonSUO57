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
            if (from is PlayerMobile pm)
            {
                if (Info != null)
                {
                    Info.Owner = CoreUtility.AntiTheftCheck(pm, Info.Owner, this);

                    Info.CloseCardGump();
                }
                else
                {
                    Delete();
                }
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

            if (from is PlayerMobile pm)
            {
                if (Info.Owner == pm || pm.AccessLevel > AccessLevel.Player)
                {
                    BaseGump.SendGump(Info.GetCardGump());

                    base.OnDoubleClick(pm);
                }
                else
                {
                    Info.Owner = CoreUtility.AntiTheftCheck(pm, Info.Owner, this);
                }
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

            if (Info.Owner == null || Info.Owner is PlayerMobile)
            {
            }
            else
            {
                Delete();
            }
        }
    }
}

using Server.Gumps;
using Server.Mobiles;
using Server.Services.UOBattleCards.Gumps;

namespace Server.Services.UOBattleCards.Items
{
	public class CardLeaderIcon : Item
	{
		public override bool DisplayWeight => false;

		public override double DefaultWeight => 0;

		[Constructable]
		public CardLeaderIcon() : base(0xA299)
		{
			Name = "Battle Card - Leaders";

			Movable = false;

			Hue = 1177;

			Weight = 0;
		}

		public override void OnDoubleClick(Mobile from)
		{
            if (!from.HasGump(typeof(LeaderBoardGump)) && from is PlayerMobile pm)
            {
                if (Parent is BattleDeck deck)
                {
                    BaseGump.SendGump(new LeaderBoardGump(pm, deck));
                }
            }
		}

		public CardLeaderIcon(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}

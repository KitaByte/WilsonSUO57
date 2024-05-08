using System.Collections.Generic;

using Server.Items;
using Server.ContextMenus;
using Server.Services.UOBattleCards.Items;

namespace Server.Services.UOBattleCard.Items
{
	public class CardGem : Item
	{
        public GemInfo Info { get; set; }

		public override bool DisplayWeight => false;

		public override double DefaultWeight => 0;

		public override string DefaultName => "Stat Gem";

		[Constructable]
		public CardGem() : base(Utility.RandomList(0x4078, 0x4079))
		{
            Info = new GemInfo
            {
                Stat = (StatCode)Utility.RandomMinMax(0, 2),
                Gem = (GemType)Utility.RandomMinMax(1, 9),
                Mod = Utility.RandomList(Utility.Random(25), Utility.Random(50), Utility.Random(75), Utility.Random(100))
            };

            UpdateGem();
        }

        public CardGem(GemInfo info) : base(Utility.RandomList(0x4078, 0x4079))
        {
            Info = info;

            UpdateGem();
        }

        private void UpdateGem()
        {
            Name = Info.Name;

            Hue = Info.Hue;
        }

        public CardGem(Serial serial) : base(serial)
		{
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);

			if (RootParent == from && from.Alive && Movable)
				list.Add(new GemEntry());
		}

		public override void OnDoubleClick(Mobile from)
		{
			base.OnDoubleClick(from);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

            Info.GemSerialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

            Info = new GemInfo();

            Info.GemDeserialize(reader);

            UpdateGem();
        }
	}
}

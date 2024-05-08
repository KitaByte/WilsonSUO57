using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class HealingPoultice : BaseCureItem
	{
		[Constructable]
		public HealingPoultice() : base(0x2DB5, new HealingPoulticeType())
		{
			Hue = 1195;
		}

		public HealingPoultice(Serial serial) : base(serial)
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

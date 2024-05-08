using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class HealingUnguent : BaseCureItem
	{
		[Constructable]
		public HealingUnguent() : base(0x24E2, new HealingUnguentType())
		{
			Hue = 1166;
		}

		public HealingUnguent(Serial serial) : base(serial)
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

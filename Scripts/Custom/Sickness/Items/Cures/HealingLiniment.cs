using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class HealingLiniment : BaseCureItem
	{
		[Constructable]
		public HealingLiniment() : base(0x0F03, new HealingLinimentType())
		{
			Hue = 1164;
		}

		public HealingLiniment(Serial serial) : base(serial)
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

using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class HealingBalm : BaseCureItem
	{
		[Constructable]
		public HealingBalm() : base(0x1006, new HealingBalmType())
		{
			Hue = 1172;
		}

		public HealingBalm(Serial serial) : base(serial)
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

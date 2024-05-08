using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class HealingSalve : BaseCureItem
	{
		[Constructable]
		public HealingSalve() : base(0x1848, new HealingSalveType())
		{
			Hue = 1168;
		}

		public HealingSalve(Serial serial) : base(serial)
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

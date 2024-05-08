using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class ElixirOfLife : BaseCureItem
	{
		[Constructable]
		public ElixirOfLife() : base(0x1842, new ElixirOfLifeType())
		{
			Hue = 1152;
		}

		public ElixirOfLife(Serial serial) : base(serial)
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

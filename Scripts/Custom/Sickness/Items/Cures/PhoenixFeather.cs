using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class PhoenixFeather : BaseCureItem
	{
		[Constructable]
		public PhoenixFeather() : base(0x0DFB, new PhoenixFeatherType())
		{
			Hue = 1161;
		}

		public PhoenixFeather(Serial serial) : base(serial)
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

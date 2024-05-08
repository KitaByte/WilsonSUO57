using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class ArcaneElixir : BaseCureItem
	{
		[Constructable]
		public ArcaneElixir() : base(0x0E24, new ArcaneElixirType())
		{
			Hue = 1163;
		}

		public ArcaneElixir(Serial serial) : base(serial)
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

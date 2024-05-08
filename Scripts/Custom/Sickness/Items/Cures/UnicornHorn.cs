using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class UnicornHorn : BaseCureItem
	{
		[Constructable]
		public UnicornHorn() : base(0x2DB7, new UnicornHornType())
		{
			Hue = 1177;
		}

		public UnicornHorn(Serial serial) : base(serial)
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

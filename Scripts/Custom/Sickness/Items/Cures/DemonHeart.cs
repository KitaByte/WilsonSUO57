using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class DemonHeart : BaseCureItem
	{
		[Constructable]
		public DemonHeart() : base(0x4A9D, new DemonHeartType())
		{
			Hue = 1166;
		}

		public DemonHeart(Serial serial) : base(serial)
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

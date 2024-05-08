using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class HolyWater : BaseCureItem
	{
		[Constructable]
		public HolyWater() : base(0x09B3, new HolyWaterType())
		{
			Hue = 1154;
		}

		public HolyWater(Serial serial) : base(serial)
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

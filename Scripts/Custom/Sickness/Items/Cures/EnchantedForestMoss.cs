using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class EnchantedForestMoss : BaseCureItem
	{
		[Constructable]
		public EnchantedForestMoss() : base(0x0F7B, new EnchantedForestMossType())
		{
			Hue = 1193;
		}

		public EnchantedForestMoss(Serial serial) : base(serial)
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

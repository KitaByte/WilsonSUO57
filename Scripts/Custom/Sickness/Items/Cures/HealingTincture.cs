using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class HealingTincture : BaseCureItem
	{
		[Constructable]
		public HealingTincture() : base(0x0E22, new HealingTinctureType())
		{
			Hue = 1157;
		}

		public HealingTincture(Serial serial) : base(serial)
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

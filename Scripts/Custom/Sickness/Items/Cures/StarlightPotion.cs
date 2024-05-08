using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class StarlightPotion : BaseCureItem
	{
		[Constructable]
		public StarlightPotion() : base(0x0F09, new StarlightPotionType())
		{
			Hue = 1186;
		}

		public StarlightPotion(Serial serial) : base(serial)
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

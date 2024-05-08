using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class DragonfirePotion : BaseCureItem
	{
		[Constructable]
		public DragonfirePotion() : base(0x0F00, new DragonfirePotionType())
		{
			Hue = 1174;
		}

		public DragonfirePotion(Serial serial) : base(serial)
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

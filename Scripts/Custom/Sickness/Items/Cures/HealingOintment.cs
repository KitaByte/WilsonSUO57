using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class HealingOintment : BaseCureItem
	{
		[Constructable]
		public HealingOintment() : base(0x2DB1, new HealingOintmentType())
		{
			Hue = 1156;
		}

		public HealingOintment(Serial serial) : base(serial)
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

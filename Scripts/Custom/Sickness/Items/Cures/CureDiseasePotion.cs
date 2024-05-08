using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class CureDiseasePotion : BaseCureItem
	{
		[Constructable]
		public CureDiseasePotion() : base(0x0E2C, new CureDiseasePotionType())
		{
			Hue = 1170;
		}

		public CureDiseasePotion(Serial serial) : base(serial)
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

using Server.Engines.Sickness.RemedyTypes;

namespace Server.Engines.Sickness.Items
{
	internal class CureCursePotion : BaseCureItem
	{
		[Constructable]
		public CureCursePotion() : base(0x0F09, new CureCursePotionType())
		{
			Hue = 1175;
		}

		public CureCursePotion(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class EmbroideryNeedlesOfEnchantment : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EmbroideryNeedlesOfEnchantment(string name) : base(0xF52)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Tailor;

			Name = "Embroidery Needles of Enchantment";

			Hue = Utility.RandomMetalHue();
		}

		public EmbroideryNeedlesOfEnchantment(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}

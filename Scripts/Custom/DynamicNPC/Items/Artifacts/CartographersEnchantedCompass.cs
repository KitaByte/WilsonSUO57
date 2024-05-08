namespace Server.Services.DynamicNPC.Items
{
	internal class CartographersEnchantedCompass : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public CartographersEnchantedCompass(string name) : base(0x1CB)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Mapmaker;

			Name = "Cartographer's Enchanted Compass";

			Hue = Utility.RandomMetalHue();
		}

		public CartographersEnchantedCompass(Serial serial) : base(serial)
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

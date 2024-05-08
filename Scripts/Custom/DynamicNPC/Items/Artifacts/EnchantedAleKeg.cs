namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedAleKeg : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedAleKeg(string name) : base(0xE7F)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Innkeeper;

			Name = "Enchanted Ale Keg";

			Hue = Utility.RandomMetalHue();
		}

		public EnchantedAleKeg(Serial serial) : base(serial)
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

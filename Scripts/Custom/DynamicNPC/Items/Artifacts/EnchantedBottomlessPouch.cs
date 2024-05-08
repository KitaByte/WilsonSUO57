namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedBottomlessPouch : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedBottomlessPouch(string name) : base(0xE79)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Provisioner;

			Name = "Enchanted Bottomless Pouch";

			Hue = Utility.RandomNondyedHue();
		}

		public EnchantedBottomlessPouch(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedPickaxe : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedPickaxe(string name) : base(0xE86)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Miner;

			Name = "Enchanted Pickaxe";

			Hue = Utility.RandomBrightHue();
		}

		public EnchantedPickaxe(Serial serial) : base(serial)
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

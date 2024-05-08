namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedHealingStaff : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedHealingStaff(string name) : base(0x905)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Veterinarian;

			Name = "Enchanted Healing Staff";

			Hue = Utility.RandomBirdHue();
		}

		public EnchantedHealingStaff(Serial serial) : base(serial)
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

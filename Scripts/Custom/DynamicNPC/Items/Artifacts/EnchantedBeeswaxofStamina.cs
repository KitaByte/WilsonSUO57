namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedBeeswaxofStamina : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedBeeswaxofStamina(string name) : base(0x1422)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Beekeeper;

			Name = "Enchanted Beeswax of Stamina";

			Hue = Utility.RandomYellowHue();
		}

		public EnchantedBeeswaxofStamina(Serial serial) : base(serial)
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

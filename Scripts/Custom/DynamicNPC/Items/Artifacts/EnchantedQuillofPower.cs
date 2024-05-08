namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedQuillofPower : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedQuillofPower(string name) : base(0x2FB7)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Mage;

			Name = "Enchanted Quill of Power";

			Hue = Utility.RandomGreenHue();
		}

		public EnchantedQuillofPower(Serial serial) : base(serial)
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

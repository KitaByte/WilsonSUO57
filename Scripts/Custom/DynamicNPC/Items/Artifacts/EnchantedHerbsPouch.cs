namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedHerbsPouch : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedHerbsPouch(string name) : base(0xE79)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Herbalist;

			Name = "Enchanted Herbs Pouch";

			Hue = Utility.RandomGreenHue();
		}

		public EnchantedHerbsPouch(Serial serial) : base(serial)
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

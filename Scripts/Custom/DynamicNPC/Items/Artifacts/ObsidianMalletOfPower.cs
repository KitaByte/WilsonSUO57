namespace Server.Services.DynamicNPC.Items
{
	internal class ObsidianMalletOfPower : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public ObsidianMalletOfPower(string name) : base(0x12B3)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.StoneCrafter;

			Name = "Obsidian Mallet of Power";

			Hue = 1175;
		}

		public ObsidianMalletOfPower(Serial serial) : base(serial)
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

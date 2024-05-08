namespace Server.Services.DynamicNPC.Items
{
	internal class DreamforgedAnvil : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public DreamforgedAnvil(string name) : base(0xFB1)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Carpenter;

			Name = "Dreamforged Anvil";

			Hue = Utility.RandomBrightHue();
		}

		public DreamforgedAnvil(Serial serial) : base(serial)
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

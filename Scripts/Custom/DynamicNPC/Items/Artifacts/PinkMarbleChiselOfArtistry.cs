namespace Server.Services.DynamicNPC.Items
{
	internal class PinkMarbleChiselOfArtistry : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public PinkMarbleChiselOfArtistry(string name) : base(0x1027)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.StoneCrafter;

			Name = "Pink Marble Chisel of Artistry";

			Hue = Utility.RandomMetalHue();
		}

		public PinkMarbleChiselOfArtistry(Serial serial) : base(serial)
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

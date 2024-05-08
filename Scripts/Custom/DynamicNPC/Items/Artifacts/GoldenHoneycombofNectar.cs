namespace Server.Services.DynamicNPC.Items
{
	internal class GoldenHoneycombofNectar : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public GoldenHoneycombofNectar(string name) : base(0x9ec)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Beekeeper;

			Name = "Golden Honeycomb of Nectar";

			Hue = Utility.RandomYellowHue();
		}

		public GoldenHoneycombofNectar(Serial serial) : base(serial)
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

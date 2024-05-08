namespace Server.Services.DynamicNPC.Items
{
	internal class BottomsUpApronOfService : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public BottomsUpApronOfService(string name) : base(0x153b)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Waiter;

			Name = "Bottoms Up Apron of Service";

			Hue = Utility.RandomMetalHue();
		}

		public BottomsUpApronOfService(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class BountifulBaitBucket : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public BountifulBaitBucket(string name) : base(0x2004)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Fisher;

			Name = "Bountiful Bait Bucket";

			Hue = Utility.RandomMetalHue();
		}

		public BountifulBaitBucket(Serial serial) : base(serial)
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

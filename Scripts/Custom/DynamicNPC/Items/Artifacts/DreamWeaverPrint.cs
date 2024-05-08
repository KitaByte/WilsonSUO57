namespace Server.Services.DynamicNPC.Items
{
	internal class DreamWeaverPrint : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public DreamWeaverPrint(string name) : base(0x14F0)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Architect;

			Name = "Dream Weaver Print";

			Hue = Utility.RandomBirdHue();
		}

		public DreamWeaverPrint(Serial serial) : base(serial)
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

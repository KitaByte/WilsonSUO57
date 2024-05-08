namespace Server.Services.DynamicNPC.Items
{
	internal class PhoenixQuillOfImmortality : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public PhoenixQuillOfImmortality(string name) : base(0xFBF)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Scribe;

			Name = "Phoenix Quill of Immortality";

			Hue = Utility.RandomBrightHue();
		}

		public PhoenixQuillOfImmortality(Serial serial) : base(serial)
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

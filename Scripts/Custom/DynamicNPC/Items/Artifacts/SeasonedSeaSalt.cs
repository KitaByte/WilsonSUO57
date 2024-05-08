namespace Server.Services.DynamicNPC.Items
{
	internal class SeasonedSeaSalt : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SeasonedSeaSalt(string name) : base(0x4C09)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Provisioner;

			Name = "Seasoned Sea Salt";

			Hue = Utility.RandomBlueHue();
		}

		public SeasonedSeaSalt(Serial serial) : base(serial)
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

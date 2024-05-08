namespace Server.Services.DynamicNPC.Items
{
	internal class SunfireSkillet : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SunfireSkillet(string name) : base(0x97F)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Cook;

			Name = "Sunfire Skillet";

			Hue = Utility.RandomYellowHue();
		}

		public SunfireSkillet(Serial serial) : base(serial)
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

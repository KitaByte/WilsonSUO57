namespace Server.Services.DynamicNPC.Items
{
	internal class PhoenixfireSteak : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public PhoenixfireSteak(string name) : base(0x097A)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Butcher;

			Name = "Phoenixfire Steak";

			Hue = Utility.RandomRedHue();
		}

		public PhoenixfireSteak(Serial serial) : base(serial)
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

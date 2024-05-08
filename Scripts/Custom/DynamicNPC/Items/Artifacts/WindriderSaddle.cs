namespace Server.Services.DynamicNPC.Items
{
	internal class WindriderSaddle : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public WindriderSaddle(string name) : base(0xF38)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Rancher;

			Name = "Windrider Saddle";

			Hue = Utility.RandomBlueHue();
		}

		public WindriderSaddle(Serial serial) : base(serial)
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

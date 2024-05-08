namespace Server.Services.DynamicNPC.Items
{
	internal class WaterSpoutHammerOfStrength : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public WaterSpoutHammerOfStrength(string name) : base(0x13E4)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Shipwright;

			Name = "Water Spout Hammer of Strength";

			Hue = Utility.RandomBlueHue();
		}

		public WaterSpoutHammerOfStrength(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class LanternofUndergroundIllumination : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public LanternofUndergroundIllumination(string name) : base(0xA25)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Miner;

			Name = "Lantern of Underground Illumination";

			Hue = Utility.RandomMetalHue();
		}

		public LanternofUndergroundIllumination(Serial serial) : base(serial)
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

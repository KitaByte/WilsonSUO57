namespace Server.Services.DynamicNPC.Items
{
	internal class SpinningWheelOfProsperity : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SpinningWheelOfProsperity(string name) : base(0x46FE)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Weaver;

			Name = "Spinning Wheel of Prosperity";

			Hue = Utility.RandomMetalHue();
		}

		public SpinningWheelOfProsperity(Serial serial) : base(serial)
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

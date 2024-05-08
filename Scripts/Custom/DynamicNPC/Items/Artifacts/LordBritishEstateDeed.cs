namespace Server.Services.DynamicNPC.Items
{
	internal class LordBritishEstateDeed : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public LordBritishEstateDeed(string name) : base(0x14F0)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.RealEstateBroker;

			Name = "Lord British Estate Deed";

			Hue = Utility.RandomMetalHue();
		}

		public LordBritishEstateDeed(Serial serial) : base(serial)
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

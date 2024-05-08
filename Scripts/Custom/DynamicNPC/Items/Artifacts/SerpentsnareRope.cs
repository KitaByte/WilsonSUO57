namespace Server.Services.DynamicNPC.Items
{
	internal class SerpentsnareRope : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SerpentsnareRope(string name) : base(0x14F8)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Rancher;

			Name = "Serpentsnare Rope";

			Hue = Utility.RandomGreenHue();
		}

		public SerpentsnareRope(Serial serial) : base(serial)
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

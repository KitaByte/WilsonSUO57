namespace Server.Services.DynamicNPC.Items
{
	internal class StormbornSailingVessel : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public StormbornSailingVessel(string name) : base(0x14F4)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.BoatPainter;

			Name = "Stormborn Sailing Vessel";

			Hue = Utility.RandomBlueHue();
		}

		public StormbornSailingVessel(Serial serial) : base(serial)
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

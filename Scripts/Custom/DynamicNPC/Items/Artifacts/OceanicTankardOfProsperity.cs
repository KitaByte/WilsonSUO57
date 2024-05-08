namespace Server.Services.DynamicNPC.Items
{
	internal class OceanicTankardOfProsperity : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public OceanicTankardOfProsperity(string name) : base(0x9CA)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.SeaMarketTavernKeeper;

			Name = "Oceanic Tankard of Prosperity";

			Hue = Utility.RandomMetalHue();
		}

		public OceanicTankardOfProsperity(Serial serial) : base(serial)
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

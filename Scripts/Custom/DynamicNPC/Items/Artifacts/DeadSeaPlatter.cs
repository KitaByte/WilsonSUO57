namespace Server.Services.DynamicNPC.Items
{
	internal class DeadSeaPlatter : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public DeadSeaPlatter(string name) : base(0x2840)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.SeaMarketTavernKeeper;

			Name = "Dead Sea Platter";

			Hue = Utility.RandomBlueHue();
		}

		public DeadSeaPlatter(Serial serial) : base(serial)
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

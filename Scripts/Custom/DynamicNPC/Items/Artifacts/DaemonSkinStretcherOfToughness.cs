namespace Server.Services.DynamicNPC.Items
{
	internal class DaemonSkinStretcherOfToughness : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public DaemonSkinStretcherOfToughness(string name) : base(0x106B)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Tanner;

			Name = "Daemon Skin Stretcher of Toughness";

			Hue = Utility.RandomRedHue();
		}

		public DaemonSkinStretcherOfToughness(Serial serial) : base(serial)
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

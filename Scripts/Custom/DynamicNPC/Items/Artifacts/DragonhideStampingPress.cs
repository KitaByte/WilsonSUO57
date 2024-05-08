namespace Server.Services.DynamicNPC.Items
{
	internal class DragonhideStampingPress : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public DragonhideStampingPress(string name) : base(0x106B)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.LeatherWorker;

			Name = "Dragonhide Stamping Press";

			Hue = Utility.RandomMetalHue();
		}

		public DragonhideStampingPress(Serial serial) : base(serial)
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

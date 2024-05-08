namespace Server.Services.DynamicNPC.Items
{
	internal class JewelSettingPliersofPrecision : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public JewelSettingPliersofPrecision(string name) : base(0xFBB)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Jeweler;

			Name = "Jewel-setting Pliers of Precision";

			Hue = Utility.RandomMetalHue();
		}

		public JewelSettingPliersofPrecision(Serial serial) : base(serial)
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

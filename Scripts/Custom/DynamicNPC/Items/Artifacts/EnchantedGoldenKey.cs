namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedGoldenKey : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedGoldenKey(string name) : base(0x100F)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.RealEstateBroker;

			Name = "Enchanted Golden Key";

			Hue = Utility.RandomYellowHue();
		}

		public EnchantedGoldenKey(Serial serial) : base(serial)
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

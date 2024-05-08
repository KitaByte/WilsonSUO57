namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedGemCuttingWheel : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedGemCuttingWheel(string name) : base(0x46FE)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Jeweler;

			Name = "Enchanted Gem-cutting Wheel";

			Hue = Utility.RandomMetalHue();
		}

		public EnchantedGemCuttingWheel(Serial serial) : base(serial)
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

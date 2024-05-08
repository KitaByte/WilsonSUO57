namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedOakBarrel : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedOakBarrel(string name) : base(0xE77)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.TavernKeeper;

			Name = "Enchanted Oak Barrel";

			Hue = Utility.RandomMetalHue();
		}

		public EnchantedOakBarrel(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedAleofIntoxication : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedAleofIntoxication(string name) : base(0x995)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Barkeeper;

			Name = "Enchanted Ale of Intoxication";

			Hue = Utility.RandomMetalHue();
		}

		public EnchantedAleofIntoxication(Serial serial) : base(serial)
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

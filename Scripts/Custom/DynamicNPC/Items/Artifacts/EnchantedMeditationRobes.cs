namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedMeditationRobes : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedMeditationRobes(string name) : base(0x1F03)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Monk;

			Name = "Enchanted Meditation Robes";

			Hue = Utility.RandomNondyedHue();
		}

		public EnchantedMeditationRobes(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedLuteofMelodies : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedLuteofMelodies(string name) : base(0xEB3)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Bard;

			Name = "Enchanted Lute of Melodies";

			Hue = Utility.RandomYellowHue();
		}

		public EnchantedLuteofMelodies(Serial serial) : base(serial)
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

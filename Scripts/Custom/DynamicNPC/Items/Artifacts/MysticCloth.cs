namespace Server.Services.DynamicNPC.Items
{
	internal class MysticCloth : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public MysticCloth(string name) : base(0x1766)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Tailor;

			Name = "Mystic Cloth";

			Hue = Utility.RandomMetalHue();
		}

		public MysticCloth(Serial serial) : base(serial)
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

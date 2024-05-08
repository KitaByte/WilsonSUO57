namespace Server.Services.DynamicNPC.Items
{
	internal class GoldenWrenchSetOfIngenuity : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public GoldenWrenchSetOfIngenuity(string name) : base(0x1EBA)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Tinker;

			Name = "Golden Wrench Set of Ingenuity";

			Hue = Utility.RandomYellowHue();
		}

		public GoldenWrenchSetOfIngenuity(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class GoldenPeltLinedCape : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public GoldenPeltLinedCape(string name) : base(0x2309)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Furtrader;

			Name = "Golden Pelt-lined Cape";

			Hue = 1174;
		}

		public GoldenPeltLinedCape(Serial serial) : base(serial)
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

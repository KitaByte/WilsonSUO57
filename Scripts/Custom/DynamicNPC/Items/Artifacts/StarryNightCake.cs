namespace Server.Services.DynamicNPC.Items
{
	internal class StarryNightCake : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public StarryNightCake(string name) : base(0x9E9)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Baker;

			Name = "Starry Night Cake";

			Hue = Utility.RandomBlueHue();
		}

		public StarryNightCake(Serial serial) : base(serial)
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

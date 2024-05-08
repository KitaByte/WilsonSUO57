namespace Server.Services.DynamicNPC.Items
{
	internal class SilverThreadLoomOfCreativity : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SilverThreadLoomOfCreativity(string name) : base(0xFA0)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Weaver;

			Name = "Silver Thread Loom of Creativity";

			Hue = Utility.RandomMetalHue();
		}

		public SilverThreadLoomOfCreativity(Serial serial) : base(serial)
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

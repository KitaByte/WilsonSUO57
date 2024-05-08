namespace Server.Services.DynamicNPC.Items
{
	internal class GlassblowersMagicPaddle : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public GlassblowersMagicPaddle(string name) : base(0xF39)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Glassblower;

			Name = "Glassblower's Magic Paddle";

			Hue = Utility.RandomMetalHue();
		}

		public GlassblowersMagicPaddle(Serial serial) : base(serial)
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

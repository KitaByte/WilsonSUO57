namespace Server.Services.DynamicNPC.Items
{
	internal class MoonlitPlow : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public MoonlitPlow(string name) : base(0x1944)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Farmer;

			Name = "Moonlit Plow";

			Hue = 1153;
		}

		public MoonlitPlow(Serial serial) : base(serial)
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

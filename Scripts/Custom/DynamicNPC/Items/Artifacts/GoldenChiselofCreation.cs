namespace Server.Services.DynamicNPC.Items
{
	internal class GoldenChiselofCreation : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public GoldenChiselofCreation(string name) : base(0x1026)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.GolemCrafter;

			Name = "Golden Chisel of Creation";

			Hue = 1174;
		}

		public GoldenChiselofCreation(Serial serial) : base(serial)
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

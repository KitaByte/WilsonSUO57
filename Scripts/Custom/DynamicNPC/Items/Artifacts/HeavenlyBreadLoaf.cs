namespace Server.Services.DynamicNPC.Items
{
	internal class HeavenlyBreadLoaf : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public HeavenlyBreadLoaf(string name) : base(0x98C)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Baker;

			Name = "Heavenly Bread Loaf";

			Hue = 1153;
		}

		public HeavenlyBreadLoaf(Serial serial) : base(serial)
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

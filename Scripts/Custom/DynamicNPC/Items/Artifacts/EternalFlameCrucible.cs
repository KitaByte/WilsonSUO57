namespace Server.Services.DynamicNPC.Items
{
	internal class EternalFlameCrucible : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EternalFlameCrucible(string name) : base(0xFB1)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Glassblower;

			Name = "Eternal Flame Crucible";

			Hue = Utility.RandomRedHue();
		}

		public EternalFlameCrucible(Serial serial) : base(serial)
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

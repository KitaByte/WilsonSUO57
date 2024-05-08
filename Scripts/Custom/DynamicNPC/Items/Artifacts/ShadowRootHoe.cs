namespace Server.Services.DynamicNPC.Items
{
	internal class ShadowRootHoe : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public ShadowRootHoe(string name) : base(0xE86)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Gardener;

			Name = "Shadow Root Hoe";

			Hue = 1175;
		}

		public ShadowRootHoe(Serial serial) : base(serial)
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

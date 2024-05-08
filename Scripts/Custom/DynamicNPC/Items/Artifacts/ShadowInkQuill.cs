namespace Server.Services.DynamicNPC.Items
{
	internal class ShadowInkQuill : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public ShadowInkQuill(string name) : base(0x2D61)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Mapmaker;

			Name = "Shadow Ink Quill";

			Hue = 1175;
		}

		public ShadowInkQuill(Serial serial) : base(serial)
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

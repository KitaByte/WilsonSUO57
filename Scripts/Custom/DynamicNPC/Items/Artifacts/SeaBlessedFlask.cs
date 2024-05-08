namespace Server.Services.DynamicNPC.Items
{
	internal class SeaBlessedFlask : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SeaBlessedFlask(string name) : base(0x1841)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.DocksAlchemist;

			Name = "Sea-Blessed Flask";

			Hue = Utility.RandomBlueHue();
		}

		public SeaBlessedFlask(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}

namespace Server.Services.DynamicNPC.Items
{
	internal class DragonfireCrossbow : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public DragonfireCrossbow(string name) : base(0x13FD)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Bowyer;

			Name = "Dragonfire Crossbow";

			Hue = Utility.RandomRedHue();
		}

		public DragonfireCrossbow(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class FeatherlightQuiver : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public FeatherlightQuiver(string name) : base(0x2FB7)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Ranger;

			Name = "Featherlight Quiver";

			Hue = Utility.RandomMetalHue();
		}

		public FeatherlightQuiver(Serial serial) : base(serial)
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

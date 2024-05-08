namespace Server.Services.DynamicNPC.Items
{
	internal class MysticwoodLongbow : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public MysticwoodLongbow(string name) : base(0x2D1E)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Bowyer;

			Name = "Mysticwood Longbow";

			Hue = Utility.RandomGreenHue();
		}

		public MysticwoodLongbow(Serial serial) : base(serial)
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

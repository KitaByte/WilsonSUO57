namespace Server.Services.DynamicNPC.Items
{
	internal class PhoenixCauldron : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public PhoenixCauldron(string name) : base(0x11C7)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Cook;

			Name = "Phoenix Cauldron";

			Hue = Utility.RandomRedHue();
		}

		public PhoenixCauldron(Serial serial) : base(serial)
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

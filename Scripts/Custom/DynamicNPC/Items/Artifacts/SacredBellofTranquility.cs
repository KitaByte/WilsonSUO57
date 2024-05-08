namespace Server.Services.DynamicNPC.Items
{
	internal class SacredBellofTranquility : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SacredBellofTranquility(string name) : base(0x1C12)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Monk;

			Name = "Sacred Bell of Tranquility";

			Hue = Utility.RandomMetalHue();
		}

		public SacredBellofTranquility(Serial serial) : base(serial)
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

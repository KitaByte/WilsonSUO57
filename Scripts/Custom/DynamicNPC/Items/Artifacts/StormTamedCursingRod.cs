namespace Server.Services.DynamicNPC.Items
{
	internal class StormTamedCursingRod : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public StormTamedCursingRod(string name) : base(0x0DC0)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Fisher;

			Name = "Storm-Tamed Blessing Rod";

			Hue = Utility.RandomMetalHue();
		}

		public StormTamedCursingRod(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class MithrilHammer : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public MithrilHammer(string name) : base(0x102A)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.GolemCrafter;

			Name = "Mithril Hammer";

			Hue = 1153;
		}

		public MithrilHammer(Serial serial) : base(serial)
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

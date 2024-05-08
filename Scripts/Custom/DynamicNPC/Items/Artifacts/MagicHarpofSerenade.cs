namespace Server.Services.DynamicNPC.Items
{
	internal class MagicHarpofSerenade : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public MagicHarpofSerenade(string name) : base(0xEB1)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Bard;

			Name = "Magic Harp of Serenade";

			Hue = Utility.RandomBlueHue();
		}

		public MagicHarpofSerenade(Serial serial) : base(serial)
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

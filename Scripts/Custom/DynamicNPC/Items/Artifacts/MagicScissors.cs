namespace Server.Services.DynamicNPC.Items
{
	internal class MagicScissors : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public MagicScissors(string name) : base(0xF9F)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.HairStylist;

			Name = "Magic Scissors";

			Hue = Utility.RandomBrightHue();
		}

		public MagicScissors(Serial serial) : base(serial)
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

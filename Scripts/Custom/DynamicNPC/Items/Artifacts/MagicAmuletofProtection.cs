namespace Server.Services.DynamicNPC.Items
{
	internal class MagicAmuletofProtection : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public MagicAmuletofProtection(string name) : base(0x1F08)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Mystic;

			Name = "Magic Amulet of Protection";

			Hue = Utility.RandomMetalHue();
		}

		public MagicAmuletofProtection(Serial serial) : base(serial)
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

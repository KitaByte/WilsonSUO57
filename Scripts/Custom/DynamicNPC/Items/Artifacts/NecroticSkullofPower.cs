namespace Server.Services.DynamicNPC.Items
{
	internal class NecroticSkullofPower : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public NecroticSkullofPower(string name) : base(0x1853)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Necromancer;

			Name = "Necrotic Skull of Power";

			Hue = Utility.RandomRedHue();
		}

		public NecroticSkullofPower(Serial serial) : base(serial)
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

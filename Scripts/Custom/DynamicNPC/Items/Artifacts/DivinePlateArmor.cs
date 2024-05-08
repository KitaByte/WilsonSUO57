namespace Server.Services.DynamicNPC.Items
{
	internal class DivinePlateArmor : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public DivinePlateArmor(string name) : base(0x3DAA)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Armourer;

			Name = "Divine Plate Armor";

			Hue = Utility.RandomMetalHue();
		}

		public DivinePlateArmor(Serial serial) : base(serial)
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

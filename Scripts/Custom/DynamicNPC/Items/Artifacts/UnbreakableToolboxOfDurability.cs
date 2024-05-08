namespace Server.Services.DynamicNPC.Items
{
	internal class UnbreakableToolboxOfDurability : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public UnbreakableToolboxOfDurability(string name) : base(0x1EBA)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Tinker;

			Name = "Unbreakable Toolbox of Durability";

			Hue = Utility.RandomBirdHue();
		}

		public UnbreakableToolboxOfDurability(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class DragonwingDrumstick : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public DragonwingDrumstick(string name) : base(0x160a)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Butcher;

			Name = "Dragonwing Drumstick";

			Hue = Utility.RandomRedHue();
		}

		public DragonwingDrumstick(Serial serial) : base(serial)
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

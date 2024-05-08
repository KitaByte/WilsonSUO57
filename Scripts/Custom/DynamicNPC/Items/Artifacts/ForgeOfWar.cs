namespace Server.Services.DynamicNPC.Items
{
	internal class ForgeOfWar : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public ForgeOfWar(string name) : base(0xFB1)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Weaponsmith;

			Name = "Forge of War";

			Hue = Utility.RandomRedHue();
		}

		public ForgeOfWar(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class BrokenTavernSignOfHistory : BaseTaskReward
	{
		public override bool IsArtifact => true;

			[Constructable]
		public BrokenTavernSignOfHistory(string name) : base(0xB95)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.TavernKeeper;

			Name = "Broken Tavern Sign of History";

			Hue = Utility.RandomAnimalHue();
		}

		public BrokenTavernSignOfHistory(Serial serial) : base(serial)
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

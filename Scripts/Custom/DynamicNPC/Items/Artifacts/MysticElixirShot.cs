namespace Server.Services.DynamicNPC.Items
{
	internal class MysticElixirShot : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public MysticElixirShot(string name) : base(0x99A)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Barkeeper;

			Name = "Mystic Elixir Shot";

			Hue = Utility.RandomMetalHue();
		}

		public MysticElixirShot(Serial serial) : base(serial)
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

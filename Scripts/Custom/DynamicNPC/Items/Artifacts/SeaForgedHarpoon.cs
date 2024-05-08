namespace Server.Services.DynamicNPC.Items
{
	internal class SeaForgedHarpoon : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SeaForgedHarpoon(string name) : base(0xF62)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.CrabFisher;

			Name = "Sea-Forged Harpoon";

			Hue = Utility.RandomMetalHue();
		}

		public SeaForgedHarpoon(Serial serial) : base(serial)
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

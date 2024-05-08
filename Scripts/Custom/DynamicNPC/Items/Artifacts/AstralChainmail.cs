namespace Server.Services.DynamicNPC.Items
{
	internal class AstralChainmail : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public AstralChainmail(string name) : base(5052)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Armourer;

			Name = "Astral Chainmail";

			Hue = Utility.RandomMetalHue();
		}

		public AstralChainmail(Serial serial) : base(serial)
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

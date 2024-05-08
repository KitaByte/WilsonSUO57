namespace Server.Services.DynamicNPC.Items
{
	internal class AstralStardustFlask : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public AstralStardustFlask(string name) : base(0x1832)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Alchemist;

			Name = "Astral Stardust Flask";

			Hue = Utility.RandomBlueHue();
		}

		public AstralStardustFlask(Serial serial) : base(serial)
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

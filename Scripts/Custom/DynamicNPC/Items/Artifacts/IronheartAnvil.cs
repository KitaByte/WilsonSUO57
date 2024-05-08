namespace Server.Services.DynamicNPC.Items
{
	internal class IronheartAnvil : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public IronheartAnvil(string name) : base(0xFAF)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.IronWorker;

			Name = "Ironheart Anvil";

			Hue = Utility.RandomRedHue();
		}

		public IronheartAnvil(Serial serial) : base(serial)
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

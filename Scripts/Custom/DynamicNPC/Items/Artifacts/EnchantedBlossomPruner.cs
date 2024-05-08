namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedBlossomPruner : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedBlossomPruner(string name) : base(0xF9F)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Gardener;

			Name = "Enchanted Blossom Pruner";

			Hue = Utility.RandomGreenHue();
		}

		public EnchantedBlossomPruner(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedPearlNet : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedPearlNet(string name) : base(0x0DCA)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.CrabFisher;

			Name = "Enchanted Pearl Net";

			Hue = 1153;
		}

		public EnchantedPearlNet(Serial serial) : base(serial)
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

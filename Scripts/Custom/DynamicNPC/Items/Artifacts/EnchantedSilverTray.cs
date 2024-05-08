namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedSilverTray : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedSilverTray(string name) : base(0x991)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Waiter;

			Name = "Enchanted Silver Tray";

			Hue = 1153;
		}

		public EnchantedSilverTray(Serial serial) : base(serial)
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

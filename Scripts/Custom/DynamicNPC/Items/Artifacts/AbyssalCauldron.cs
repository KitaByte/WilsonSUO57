namespace Server.Services.DynamicNPC.Items
{
	internal class AbyssalCauldron : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public AbyssalCauldron(string name) : base(0x11C7)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.DocksAlchemist;

			Name = "Abyssal Cauldron";

			Hue = Utility.RandomBlueHue();
		}
		public AbyssalCauldron(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			var version = reader.ReadInt();
		}
	}
}

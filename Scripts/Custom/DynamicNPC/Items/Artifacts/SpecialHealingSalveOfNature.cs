namespace Server.Services.DynamicNPC.Items
{
	internal class SpecialHealingSalveOfNature : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SpecialHealingSalveOfNature(string name) : base(0x183B)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Veterinarian;

			Name = "Special Healing Salve of Nature";

			Hue = Utility.RandomGreenHue();
		}

		public SpecialHealingSalveOfNature(Serial serial) : base(serial)
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

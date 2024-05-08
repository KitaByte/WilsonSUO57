namespace Server.Services.DynamicNPC.Items
{
	internal class ShadowCloakOfStealth : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public ShadowCloakOfStealth(string name) : base(0x2FB9)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Thief;

			Name = "Shadow Cloak of Stealth";

			Hue = 1175;
		}

		public ShadowCloakOfStealth(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class ShadowedOakScepter : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public ShadowedOakScepter(string name) : base(Utility.RandomList(0xDF2, 0xDF3, 0xDF4, 0xDF5))
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Carpenter;

			Name = "Shadowed Oak Scepter";

			Hue = 1175;
		}

		public ShadowedOakScepter(Serial serial) : base(serial)
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

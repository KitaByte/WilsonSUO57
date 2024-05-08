namespace Server.Services.DynamicNPC.Items
{
	internal class FrostfurCloak : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public FrostfurCloak(string name) : base(0x230A)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Furtrader;

			Name = "Frostfur Cloak";

			Hue = 1152;
		}

		public FrostfurCloak(Serial serial) : base(serial)
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

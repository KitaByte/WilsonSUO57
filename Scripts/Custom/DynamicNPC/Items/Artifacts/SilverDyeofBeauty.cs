namespace Server.Services.DynamicNPC.Items
{
	internal class SilverDyeofBeauty : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SilverDyeofBeauty(string name) : base(0xEFF)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.HairStylist;

			Name = "Silver Comb of Beauty";

			Hue = 1153;
		}

		public SilverDyeofBeauty(Serial serial) : base(serial)
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

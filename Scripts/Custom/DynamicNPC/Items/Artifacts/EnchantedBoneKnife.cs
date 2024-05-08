namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedBoneKnife : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedBoneKnife(string name) : base(0x13F6)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.LeatherWorker;

			Name = "Enchanted Bone Knife";

			Hue = 1153;
		}

		public EnchantedBoneKnife(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class SilverAnchorChainOfDurability : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SilverAnchorChainOfDurability(string name) : base(0x14F7)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Shipwright;

			Name = "Silver Anchor Chain of Durability";

			Hue = 1153;
		}

		public SilverAnchorChainOfDurability(Serial serial) : base(serial)
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

namespace Server.Services.DynamicNPC.Items
{
	internal class FrostforgedWarAxe : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public FrostforgedWarAxe(string name) : base(0x13B0)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Blacksmith;

			Name = "Frostforged War Axe";

			Hue = Utility.RandomBlueHue();
		}

		public FrostforgedWarAxe(Serial serial) : base(serial)
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

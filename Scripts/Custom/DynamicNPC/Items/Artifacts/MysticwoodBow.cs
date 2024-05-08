namespace Server.Services.DynamicNPC.Items
{
	internal class MysticwoodBow : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public MysticwoodBow(string name) : base(0x13B2)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Ranger;

			Name = "Mysticwood Bow";

			Hue = Utility.RandomBirdHue();
		}

		public MysticwoodBow(Serial serial) : base(serial)
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

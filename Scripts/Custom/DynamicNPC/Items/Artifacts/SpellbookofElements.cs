namespace Server.Services.DynamicNPC.Items
{
	internal class SpellbookofElements : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public SpellbookofElements(string name) : base(0xEFA)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Mage;

			Name = "Spellbook of Elements"; 
			
			Hue = Utility.RandomBrightHue();
		}

		public SpellbookofElements(Serial serial) : base(serial)
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

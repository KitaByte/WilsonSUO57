namespace Server.Services.DynamicNPC.Items
{
	internal class ThunderstrikeBattleHammer : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public ThunderstrikeBattleHammer(string name) : base(0x1439)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Blacksmith;

			Name = "Thunderstrike Battle Hammer";

			Hue = Utility.RandomBlueHue();
		}

		public ThunderstrikeBattleHammer(Serial serial) : base(serial)
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

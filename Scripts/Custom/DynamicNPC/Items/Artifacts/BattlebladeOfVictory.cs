namespace Server.Services.DynamicNPC.Items
{
	internal class BattlebladeOfVictory : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public BattlebladeOfVictory(string name) : base(0xF5E)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Weaponsmith;

			Name = "Battleblade of Victory";

			Hue = Utility.RandomMetalHue();
		}

		public BattlebladeOfVictory(Serial serial) : base(serial)
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

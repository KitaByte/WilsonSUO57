﻿namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedMysticsTome : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedMysticsTome(string name) : base(0xEFA)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Mystic;

			Name = "Enchanted Mystic's Tome";

			Hue = Utility.RandomMetalHue();
		}

		public EnchantedMysticsTome(Serial serial) : base(serial)
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

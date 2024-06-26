﻿namespace Server.Services.DynamicNPC.Items
{
	internal class EnchantedLockpick : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public EnchantedLockpick(string name) : base(0x14fc)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Thief;

			Name = "Enchanted Lockpick";

			Hue = Utility.RandomMetalHue();
		}

		public EnchantedLockpick(Serial serial) : base(serial)
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

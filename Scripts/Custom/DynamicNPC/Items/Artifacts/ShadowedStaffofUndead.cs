namespace Server.Services.DynamicNPC.Items
{
	internal class ShadowedStaffofUndead : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public ShadowedStaffofUndead(string name) : base(0xDF0)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Necromancer;

			Name = "Shadowed Staff of Undead";

			Hue = 1175;
		}

		public ShadowedStaffofUndead(Serial serial) : base(serial)
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

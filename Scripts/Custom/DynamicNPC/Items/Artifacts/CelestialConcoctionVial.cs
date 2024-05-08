namespace Server.Services.DynamicNPC.Items
{
	internal class CelestialConcoctionVial : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public CelestialConcoctionVial(string name) : base(0x185D)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.Alchemist;

			Name = "Celestial Concoction Vial";

			Hue = Utility.RandomMetalHue();
		}

		public CelestialConcoctionVial(Serial serial) : base(serial)
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

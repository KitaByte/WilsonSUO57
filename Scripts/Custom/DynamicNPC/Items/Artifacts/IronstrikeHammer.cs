namespace Server.Services.DynamicNPC.Items
{
	internal class IronstrikeHammer : BaseTaskReward
	{
		public override bool IsArtifact => true;

		[Constructable]
		public IronstrikeHammer(string name) : base(0x102A)
		{
			VendorName = name;

			VendorType = Data.VendorProfessions.VendorTypes.IronWorker;

			Name = "Ironstrike Hammer";

			Hue = 1174;
		}

		public IronstrikeHammer(Serial serial) : base(serial)
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

using Server.Items;

namespace Server.Services.DynamicNPC.Items
{
	internal class GoldVendorSash : BodySash
	{
		[Constructable]
		public GoldVendorSash(string prof) 
		{
			Name = $"Grandmaster {prof}";

			Hue = 1174;

			LootType = LootType.Blessed;
		}

		public GoldVendorSash(Serial serial) : base(serial)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}
	}
}

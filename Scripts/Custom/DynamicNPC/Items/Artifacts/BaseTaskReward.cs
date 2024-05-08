using Server.Mobiles;
using Server.Services.DynamicNPC.Data;

namespace Server.Services.DynamicNPC.Items
{
	internal class BaseTaskReward : Item, ITaskReward
	{
		public string VendorName { get; set; }

		public VendorProfessions.VendorTypes VendorType { get; set; }

		public Item RewardItem { get; private set; }

		public BaseTaskReward()
		{
			RewardItem = this;
		}

		public BaseTaskReward(int itemID) : base(itemID)
		{
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from is PlayerMobile player)
			{
				player.Say(53, $"Reward for your patronage, from {VendorName}!");
			}
		}

		public BaseTaskReward(Serial serial) : base(serial)
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

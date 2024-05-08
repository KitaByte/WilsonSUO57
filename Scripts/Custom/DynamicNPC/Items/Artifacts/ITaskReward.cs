using Server.Services.DynamicNPC.Data;

namespace Server.Services.DynamicNPC.Items
{
	internal interface ITaskReward
	{
		string VendorName { get; }

		VendorProfessions.VendorTypes VendorType { get; }

		Item RewardItem { get; }
	}
}

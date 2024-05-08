using Server.Mobiles;
using Server.Services.DynamicNPC.Data;

namespace Server.Services.DynamicNPC
{
	internal class DynamicVendorMobile
	{
		static internal void OnVendorCreated(MobileCreatedEventArgs e)
		{
			if (e?.Mobile is BaseVendor vendor && VendorProfessions.IsGoodVendor(vendor?.GetType()))
			{
				ProfileStore.AddVendor(vendor, new VendorProfile(vendor));

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Created : {vendor.Name} {vendor.Title}", true);
				}
			}
		}

		static internal void OnVendorDeleted(MobileDeletedEventArgs e)
		{
			if (e?.Mobile is BaseVendor vendor && VendorProfessions.IsGoodVendor(vendor?.GetType()))
			{
				ProfileStore.RemoveVendor(vendor);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Deleted : {vendor.Name} {vendor.Title}", true);
				}
			}
		}
	}
}
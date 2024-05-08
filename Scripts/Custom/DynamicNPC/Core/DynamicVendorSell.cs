using System;
using Server.Mobiles;

namespace Server.Services.DynamicNPC
{
	static internal class DynamicVendorSell
	{
		public static void OnVendorSell(ValidVendorSellEventArgs e)
		{
			var profile = ProfileStore.GetVendorProfile(e?.Vendor);

			if (profile != null && e?.Sold is Item item && e?.Mobile is PlayerMobile player)
			{
				int bonus;

				if (!profile.HasCustomer(player))
				{
					profile.AddCustomer(new Customer(player, profile.VendorHandle) { LastVisited = DateTime.Now });
				}
				else
				{
					profile.GetCustomer(player).LastVisited = DateTime.Now;
				}

				if (item.Amount > 1)
				{
					bonus = ((e.AmountPerUnit * item.Amount) * profile.Level) / DynamicSettings.BonusMod;
				}
				else
				{
					bonus = (e.AmountPerUnit * profile.Level) / DynamicSettings.BonusMod;
				}

				profile.GivePatronageBonus(player, true, bonus);

				profile.TrySkillGain();

				profile.TryTask(player);

				VendorResponses.VendorSellResponse(profile, player.Name);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Sell : {profile.VendorHandle.Name} Sold to {player}", true);
				}
			}
		}
	}
}

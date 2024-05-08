using System;
using Server.Mobiles;

namespace Server.Services.DynamicNPC
{
	static internal class DynamicVendorBuy
	{
		public static void OnVendorBuy(ValidVendorPurchaseEventArgs e)
		{
			var profile = ProfileStore.GetVendorProfile(e?.Vendor);

			if (profile != null && e?.Bought is Item item && e?.Mobile is PlayerMobile player)
			{
				int bonus;

				bool isMulti;

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

					isMulti = true;
				}
				else
				{
					bonus = (e.AmountPerUnit * profile.Level) / DynamicSettings.BonusMod;

					isMulti = false;
				}

				profile.GivePatronageBonus(player, false, bonus);

				if (isMulti)
				{
					profile.TryAddMore(player, item);
				}
				else
				{
					profile.TryModItem(player, item);
				}

				profile.TrySkillGain();

				profile.TryTask(player);

				VendorResponses.VendorBuyResponse(profile, player.Name);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Buy : {profile.VendorHandle.Name} bought off {player}", true);
				}
			}
		}
	}
}

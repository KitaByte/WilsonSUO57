namespace Server.Services.DynamicNPC
{
	static internal class VendorResponses
	{
		static internal void AgeResponse(VendorProfile profile)
		{
			profile.VendorHandle.Say($"It's my Birthday, I just turned {profile.Age}");
		}

		static internal void BonusGoldResponse(VendorProfile profile, string patron, int amount)
		{
			profile.VendorHandle.Say(GetLoyalLevelResponce(profile, patron));

			profile.VendorHandle.Say($"Thank you for the business, here is {amount} gold for you!");
		}

		static internal void BonusModResponse(VendorProfile profile, string patron, string item)
		{
			profile.VendorHandle.Say(GetLoyalLevelResponce(profile, patron));

			profile.VendorHandle.Say($"Thank you for the business, take this special {item}, I made for you!");
		}

		static internal void BonusMoreResponse(VendorProfile profile, string patron)
		{
			profile.VendorHandle.Say(GetLoyalLevelResponce(profile, patron));

			profile.VendorHandle.Say("Thank you for the business, I added an extra something to your order!");
		}

		static internal void GiveTaskResponse(VendorProfile profile, string patron)
		{
			profile.VendorHandle.Say(GetLoyalLevelResponce(profile, patron));

			profile.VendorHandle.Say("Thank you for the business, I have a task for you, will you accept?");
		}

		static internal void LevelGainResponse(VendorProfile profile)
		{
			profile.VendorHandle.Say($"I feel {Utility.RandomList("smarter", "enlightened", "illuminated")}!");
		}

		static internal void VendorBuyResponse(VendorProfile profile, string patron)
		{
			profile.VendorHandle.Say(GetLoyalLevelResponce(profile, patron));

			profile.VendorHandle.Say($"Thank you for your business, {patron}");
		}

		static internal void VendorSellResponse(VendorProfile profile, string patron)
		{
			profile.VendorHandle.Say(GetLoyalLevelResponce(profile, patron));

			profile.VendorHandle.Say($"Thank you for selling your items, {patron}");
		}

		private static string GetLoyalLevelResponce(VendorProfile profile, string customer)
		{
			var reply = "Have a nice day?";

			if (profile.HasCustomer(customer))
			{
				var patron = profile.GetCustomer(customer);

				if (patron != null)
				{
					var topVisitorIndex = profile.GetTopIndex(customer);

					if (topVisitorIndex == 0)
						reply = $"{customer}, your my favorite customer, you get the best deals!";
					else if (topVisitorIndex < 10)
						reply = $"{customer}, your one of my top customers!";
					else
						reply = $"{customer}, good to see you today!";
				}
			}

			return reply;
		}
	}
}

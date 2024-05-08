using System.Linq;
using Server.Engines.Quests;
using Server.Mobiles;
using Server.Services.DynamicNPC.Data;

namespace Server.Services.DynamicNPC.Commands
{
	static internal class DynCmdSpeech
	{
		static internal void OnSpeech(SpeechEventArgs e)
		{
			if (e.Mobile is PlayerMobile player && !string.IsNullOrEmpty(e.Speech))
			{
                string txt = e.Speech.ToLower();

                bool goodMsg = false;

                switch (txt)
                {
                    case string speech1 when speech1.StartsWith("hello"):
                    case string speech2 when speech2.StartsWith("bye"):
                    case string speech3 when speech3.EndsWith("task"):
                    case string speech4 when speech4.EndsWith("record"):
                        goodMsg = true; break;
                }

                if (goodMsg)
                {
                    foreach (var m in player.GetMobilesInRange(5))
                    {
                        CheckVendorTarget(player, m, txt);
                    }
                }
            }
		}

		private static void CheckVendorTarget(PlayerMobile player, Mobile m, string txt)
		{
			if (m != null && m is BaseVendor bv && bv != null)
			{
                var title = bv.Title?.Split(' ')?.Last();

                if (!string.IsNullOrEmpty(title) && VendorProfessions.IsGoodVendor(title, out var type))
                {
                    var vendor = BaseVendor.AllVendors.Find(v => v.Name == bv.Name && v.GetMobilesInRange(5).Contains(player));

                    if (vendor != null)
                    {
                        QuestSystem.FocusTo(player, vendor);

                        var profile = ProfileStore.GetVendorProfile(vendor);

                        if (txt == "hello")
                        {
                            if (profile?.GetLoyalityMod(player.Name) > 2)
                            {
                                bv.Say($"{player.Name}, it is so nice to see my best customer, how are you today?");
                            }
                            else if (profile?.GetLoyalityMod(player.Name) == 2)
                            {
                                bv.Say($"Hello, {player.Name}, how are you today?");
                            }
                            else
                            {
                                bv.Say("Hello!");
                            }

                            return;
                        }

                        if (txt == "bye")
                        {
                            if (profile?.GetLoyalityMod(player.Name) > 2)
                            {
                                bv.Say($"{player.Name}, have a great day, come back soon!");
                            }
                            else if (profile?.GetLoyalityMod(player.Name) == 2)
                            {
                                bv.Say($"Bye, {player.Name}");
                            }
                            else
                            {
                                bv.Say("Bye!");
                            }

                            return;
                        }

                        if (txt == "task")
                        {
                            if (profile?.GetCustomer(player.Name) is Customer customer)
                            {
                                bv.Say(customer.VendTask.Taker?.Name == player.Name ? $"{player.Name}, Has Task" : $"{player.Name}, Has No Task");

                                if (customer.VendTask.Taker?.Name == player.Name)
                                    bv.Say(customer.VendTask.IsAccepted ? "Task Accepted!" : "Task Not Accepted Yet!");
                            }
                            else
                            {
                                bv.Say("Your not a customer of mine, you havn't bought or sold anything with me yet!");
                            }

                            return;
                        }

                        if (txt == "record")
                        {
                            if (profile?.GetCustomer(player.Name) is Customer customer)
                            {
                                bv.Say($"{player.Name}, here is my record on you!");
                                bv.Say($"{customer.TotVisits}, Total visits!");
                                bv.Say($"{customer.LastVisited}, Last visited!");
                                bv.Say($"{customer.TotBought}, Total bought!");
                                bv.Say($"{customer.TotSold}, Total sold!");
                                bv.Say($"{customer.TotBonus}, Total bonus!");
                                bv.Say($"{customer.TotTasks}, Total tasks completed!");
                                bv.Say($"{profile.GetTopIndex(player.Name)}/{profile.GetCustomerCount()}, Customer rank!");
                            }
                            else
                            {
                                bv.Say("Your not a customer of mine, you havn't bought or sold anything with me yet!");
                            }

                            return;
                        }
                    }
                }
			}
		}
	}
}

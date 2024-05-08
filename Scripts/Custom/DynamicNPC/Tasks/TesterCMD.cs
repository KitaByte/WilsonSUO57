using System.Linq;
using Server.Commands;
using Server.Mobiles;

namespace Server.Services.DynamicNPC.Tasks
{
	internal class TesterCMD
	{
		public static void Initialize()
		{
			CommandSystem.Register("RunTest", AccessLevel.Administrator, TestSys_OnCommand);
		}

		[Usage("RunTest <Vendor Name>")]
		[Description("Test vendor task : in game test!")]
		private static void TestSys_OnCommand(CommandEventArgs e)
		{
			if (e.Mobile is PlayerMobile admin && admin != null && e.Arguments.Length > 0)
			{
				if (e.Arguments[0] != null)
				{
					var vendor = BaseVendor.AllVendors.Find(v => v.Name == e.Arguments[0]);

					if (vendor != null)
					{
						var testTask = new VendorTask(vendor, admin);

						TestMsg(admin, testTask);
					}
					else
					{
						admin.SendMessage("That vendor doesn't exist!");
					}
				}
				else
				{
					admin.SendMessage("Need to input name of Vendor to use!");
				}
			}
		}

		private static void TestMsg(PlayerMobile admin, VendorTask testTask)
		{
			admin.SendMessage($"--------Test--------");

			try
			{
				admin.SendMessage($"M = {TaskTargets.GetMobileTarget(Utility.RandomList(1, 2, 3))?.GetType().ToString().Split('.').Last()}");
			}
			catch
			{
				admin.SendMessage("Mobile would have crashed");
			}

			try
			{
				admin.SendMessage($"I = {TaskTargets.GetItemTarget()?.GetType().ToString().Split('.').Last()}");
			}
			catch
			{
				admin.SendMessage("Item would have crashed");
			}

			try
			{
				admin.SendMessage($"L = {TaskTargets.GetLocationTarget(admin, out var target)}");

				admin.SendMessage($"T = {target?.Name}");
			}
			catch
			{
				admin.SendMessage("Location would have crashed");
			}

			admin.SendMessage($"--------Task--------");
			admin.SendMessage($"V: {testTask.Giver?.Name}");
			admin.SendMessage($"P: {testTask.Taker?.Name}");
			admin.SendMessage($"--------------------");
			admin.SendMessage($"T: {testTask.TaskType}");
			admin.SendMessage($"N: {testTask.TaskName}");
			admin.SendMessage($"--------------------");
			admin.SendMessage($"M: {testTask.TargetMobile?.GetType().ToString().Split('.').Last()}");
			admin.SendMessage($"I: {testTask.TargetItem?.GetType().ToString().Split('.').Last()}");
			admin.SendMessage($"L: {testTask.TargetLocation}");
			admin.SendMessage($"--------------------");
			admin.SendMessage($"A: {testTask.TargetAmount}");
			admin.SendMessage($"R: {testTask.Reward}");
			admin.SendMessage($"D: {testTask.Description}");
			admin.SendMessage($"--------------------");
		}
	}
}

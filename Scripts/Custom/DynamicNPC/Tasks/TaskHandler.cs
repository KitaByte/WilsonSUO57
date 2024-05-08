using System.Collections.Generic;
using System.Linq;
using Server.Engines.Quests;
using Server.Mobiles;
using Server.Services.DynamicNPC.Commands;

namespace Server.Services.DynamicNPC.Tasks
{
	static internal class TaskHandler
	{
		public static readonly List<VendorTask> TaskList = new List<VendorTask>();

		private static List<VendorTask> GetAcceptedTasks(PlayerMobile player, TaskTypes task)
		{
			if (task == TaskTypes.Fetch)
			{
				return TaskList?.FindAll(t => t.IsAccepted && !t.IsComplete && t.TargetItem != null && t.Taker == player);
			}

			if (task == TaskTypes.Delivery)
			{
				return TaskList?.FindAll(t => t.IsAccepted && !t.IsComplete && t.TargetLocation != new Point3D(0,0,0) && t.Taker == player);
			}

			if (task == TaskTypes.Kill)
			{
				return TaskList?.FindAll(t => t.IsAccepted && !t.IsComplete && t.TargetMobile != null && t.Taker == player);
			}

			return TaskList?.FindAll(t => t.IsAccepted && !t.IsComplete && t.Taker == player);
		}

		private static List<VendorTask> GetCompletedTasks(PlayerMobile player)
		{
			return TaskList?.FindAll(t => t.IsAccepted && t.IsComplete && t.Taker == player);
		}

		static internal void OnCreatureDeath(CreatureDeathEventArgs e)
		{
			if (e.Killer != null && e.Killer is PlayerMobile player)
			{
				var playerTasks = GetAcceptedTasks(player, TaskTypes.Kill);

				if (playerTasks.Count > 0)
				{
					for (var i = 0; i < playerTasks.Count; i++)
					{
						CheckMobileTarget(e, player, playerTasks[i]);
					}
				}
			}
		}

		private static void CheckMobileTarget(CreatureDeathEventArgs e, PlayerMobile player, VendorTask task)
		{
			if (task.TargetMobile.GetType() == e.Creature.GetType() && task.TargetAmount > 0)
			{
				task.TargetAmount--;

				if (task.TargetAmount == 0)
				{
					if (task.IsComplete)
					{
						player.SendMessage($"You should go see {task.Giver.Name}, for your reward!");
					}
					else
					{
						player.SendMessage($"You have finished killing {task.AddS(task.GetMobileName())} for your task with {task.Giver.Name}");

						task.IsComplete = true;
					}
				}
				else
				{
					player.SendMessage($"You have killed a {task.GetMobileName()} for your task with {task.Giver.Name}");
				}
			}
		}

		static internal void OnItemObtained(OnItemObtainedEventArgs e)
		{
			if (e.Mobile != null && e.Mobile is PlayerMobile player)
			{
				var playerTasks = GetAcceptedTasks(player, TaskTypes.Fetch);

				if (playerTasks.Count > 0)
				{
					for (var i = 0; i < playerTasks.Count; i++)
					{
						CheckItemTarget(e, player, playerTasks[i]);
					}
				}
			}
		}

		private static void CheckItemTarget(OnItemObtainedEventArgs e, PlayerMobile player, VendorTask task)
		{
			if (task.TargetItem.GetType() == e.Item.GetType() && task.TargetAmount > 0)
			{
				task.TargetAmount--;

				MarkTaskItem(e);

				if (task.TargetAmount == 0)
				{
					player.SendMessage($"You have finished obtaining {task.AddS(task.GetItemName())} for your task with {task.Giver.Name}");

					task.IsComplete = true;
				}
				else
				{
					player.SendMessage($"You have obtained a {task.GetItemName()} for your task with {task.Giver.Name}");
				}
			}
		}

		private static void MarkTaskItem(OnItemObtainedEventArgs e)
		{
			e.Item.Movable = false;

			e.Item.Name = e.Item.Name + " (Task Item)";

			e.Item.Hue = 1174;
		}

		static internal void OnSpeech(SpeechEventArgs e)
		{
			if (e.Mobile is PlayerMobile player)
			{
                if (!DynamicHandler.IsNight(e.Mobile))
                {
                    var playerTasks = GetAcceptedTasks(player, TaskTypes.None);

                    if (e.Speech.ToLower() == "cancel task" || e.Speech.ToLower() == "accept task")
                    {
                        if (playerTasks.Count > 0)
                            UpdateTask(e, player, playerTasks);

                        return;
                    }

                    playerTasks = GetAcceptedTasks(player, TaskTypes.Delivery);

                    var playerCompletedTasks = GetCompletedTasks(player);

                    if (playerTasks.Count > 0)
                        UpdateLocationTask(e, player, playerTasks);

                    if (playerCompletedTasks.Count > 0)
                        UpdateCompletion(e, player, playerCompletedTasks);

                    if (e != null)
                    {
                        DynCmdSpeech.OnSpeech(e);
                    }
                }
            }
		}

		private static void UpdateTask(SpeechEventArgs e, PlayerMobile player, List<VendorTask> playerTasks)
		{
			for (var i = 0; i < playerTasks.Count; i++)
			{
				CheckUpdate(e, player, playerTasks[i]);
			}
		}

		private static void CheckUpdate(SpeechEventArgs e, PlayerMobile player, VendorTask task)
		{
			if (player.GetMobilesInRange(5).Contains(task.Giver))
			{
				QuestSystem.FocusTo(player, task.Giver);

				if (e.Speech.ToLower() == "accept task" && !task.IsAccepted)
				{
					task.IsAccepted = true;

					player.SendMessage("Your task has been accepted!");

					return;
				}

				if (e.Speech.ToLower() == "cancel task")
				{
					var vendor = ProfileStore.GetVendorProfile(task.Giver);

					var customer = vendor.GetCustomer(player);

					customer.VendTask = null;

					player.SendMessage("Your task has been cancelled!");

					return;
				}

				task.Giver.Say("I don't understand? Do you need to service a Task?");

				player.SendMessage("Use 'accept task' or 'cancel task'");
			}
		}

		private static void UpdateLocationTask(SpeechEventArgs e, PlayerMobile player, List<VendorTask> playerTasks)
		{
			for (var i = 0; i < playerTasks.Count; i++)
			{
				CheckLocationTarget(e, player, playerTasks[i]);
			}
		}

		private static void CheckLocationTarget(SpeechEventArgs e, PlayerMobile player, VendorTask task)
		{
			var vendor = task.TargetMobile;

			if (task.TargetAmount == 1)
			{
				QuestSystem.FocusTo(player, task.Giver);

				task.TargetAmount = 0;

				if (player.GetMobilesInRange(5).Contains(vendor))
				{
					switch (task.TaskName)
					{
						case TaskNames.TakeTo:
							if (e.Speech.ToLower() == $"{task.Giver.Name} sent me")
							{
								player.SendMessage($"You have finished your task to {task.Giver.Name} for seeking {vendor.Name}");

								task.IsComplete = true;

								vendor.Say($"I know {task.Giver.Name}, it is a pleasure to meet you, my name is {vendor.Name}!");

								vendor.Say($"I retrieved from your pack the item sent from {task.Giver.Name}!");
							}
							break;

						case TaskNames.VisitPlace:
							if (e.Speech.ToLower() == $"{task.Giver.Name} sent me")
							{
								player.SendMessage($"You have finished your task to {task.Giver.Name} for seeking {vendor.Name}");

								task.IsComplete = true;

								vendor.Say($"{task.Giver.Name} knew I needed a visit, sure gets lonely around here!");

								vendor.Say($"Thanks so much {player.Name}, I realy needed this!");
							}
							break;

						case TaskNames.TalkTo:
							if (e.Speech.ToLower() == $"{task.Giver.Name} sent me")
							{
								player.SendMessage($"You have finished your task to {task.Giver.Name} for seeking {vendor.Name}");

								task.IsComplete = true;

								vendor.Say($"I found the message from {task.Giver.Name}, message is well received!");

								vendor.Say($"Tell {task.Giver.Name}, thanks!");
							}
							break;

						default:
							if (e.Speech.ToLower() != $"{task.Giver.Name} sent me")
							{
								vendor.Say("Do I know you?");
							}
							else
							{
								vendor.Say("Can I help you?");
							}
							break;
					}
				}
			}
		}

		private static void UpdateCompletion(SpeechEventArgs e, PlayerMobile player, List<VendorTask> playerCompletedTasks)
		{
			for (var i = 0; i < playerCompletedTasks.Count; i++)
			{
				CheckCompletion(e, player, playerCompletedTasks[i]);
			}
		}

		private static void CheckCompletion(SpeechEventArgs e, PlayerMobile player, VendorTask task)
		{
			if (player.GetMobilesInRange(5).Contains(task.Giver) && task.TargetAmount == 0)
			{
				QuestSystem.FocusTo(player, task.Giver);

				if (e.Speech.ToLower() == "task complete" || e.Speech.ToLower() == "task done")
				{
					var bonusMobile = task.TargetMobile?.Hits ?? 0;

					var bonusItem = task.TargetItem?.DefaultWeight * 10 ?? 0;

					double? bonusLocation = task.TargetLocation != new Point3D(0, 0, 0)
											? task?.Giver.GetDistanceToSqrt(task.TargetLocation)
											: 0;

					var vendor = ProfileStore.GetVendorProfile(task?.Giver);

					var totalBonus = bonusMobile + bonusItem + bonusLocation + (vendor?.GetCustomer(player)?.TotVisits / 100) ?? 0;

					task.Complete(player, (int)totalBonus);

					if (task.Reward != null)
					{
						player.AddToBackpack(task.Reward);
					}

					vendor.GetCustomer(player).TotTasks++;

					if (vendor != null)
					{
						if (Utility.RandomMinMax(1, 1000) < vendor?.GetCustomer(player)?.TotVisits / 100)
						{
							task.GiveSpecialTask();
						}
						else
						{
							vendor.GetCustomer(player).VendTask = null;
						}
					}
				}
			}
		}
	}
}

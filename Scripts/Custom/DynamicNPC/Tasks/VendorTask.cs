using System;
using System.Linq;
using System.Text;
using Server.Items;
using Server.Mobiles;
using Server.Services.DynamicNPC.Items;

namespace Server.Services.DynamicNPC.Tasks
{
	public enum TaskTypes
	{
		None		= 0,
		Fetch		= 1,
		Delivery	= 2,
		Kill		= 3
	}

	public enum TaskNames // Gold = 10 * TaskTypes * TaskNames;
	{
		None			= 0,  // Gold Payout
		FindLost		= 11, // 110
		FindTen			= 12, // 120
		FindTwenty		= 13, // 130
		TakeTo			= 21, // 420
		VisitPlace		= 22, // 440
		TalkTo			= 23, // 460
		KillTarget		= 31, // 930
		KillTen			= 32, // 960
		KillTwenty		= 33  // 990
	}

	public class VendorTask
	{
		public bool IsAccepted { get; set; } = false;

		public bool IsComplete { get; set; } = false;

		public BaseVendor Giver { get; set; }

		public PlayerMobile Taker { get; set; }

		public TaskTypes TaskType { get; set; }

		public TaskNames TaskName { get; set; }

		public Mobile TargetMobile { get; set; }

		public Item TargetItem { get; set; }

		public int TargetAmount { get; set; }

		public Item Reward { get; set; }

		public string Description { get; set; }

		public Point3D TargetLocation { get; set; }

		public VendorTask()
		{
			TaskHandler.TaskList.Add(this);

			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Blank Task Created", true);
			}
		}

		public VendorTask(BaseVendor vendor, PlayerMobile player)
		{
			TaskHandler.TaskList.Add(this);

			Giver = vendor;

			Taker = player;

			TaskType = GetRandomTask();

			if (TaskType != TaskTypes.None)
			{
				TaskName = GetTaskName(TaskType);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Task Created", true);
				}
			}
		}

		private TaskTypes GetRandomTask()
		{
			return (TaskTypes)Utility.RandomList(1, 2, 3);
		}

		private TaskNames GetTaskName(TaskTypes tasktype)
		{
			if (tasktype == TaskTypes.None) return TaskNames.None;

			var RandomThree = Utility.RandomList(1, 2, 3);

			if (tasktype == TaskTypes.Fetch)
			{
				TargetItem = TaskTargets.GetItemTarget();

				RandomThree += 10;

				switch ((TaskNames)RandomThree)
				{
					case TaskNames.FindLost:
						{
							TargetAmount = 1;

							var name = GetItemName().EndsWith("s") ? GetItemName() : "a " + GetItemName();

							Giver.Say(Description = $"Find and return with {name}");

							return TaskNames.FindLost;
						}
					case TaskNames.FindTen:
						{
							TargetAmount = 10;

							Giver.Say(Description = $"Find and return with 10 {AddS(GetItemName())}");

							return TaskNames.FindTen;
						}
					case TaskNames.FindTwenty:
						{
							TargetAmount = 20;

							Giver.Say(Description = $"Find and return with 20 {AddS(GetItemName())}");

							return TaskNames.FindTwenty;
						}
						default: return TaskNames.None;
				}
			}

			if (tasktype == TaskTypes.Delivery)
			{
				TargetLocation = TaskTargets.GetLocationTarget(Taker, out var vendor);

				TargetMobile = vendor;

				TargetAmount = 1;

				RandomThree += 20;

				switch ((TaskNames)RandomThree)
				{
					case TaskNames.TakeTo:
						{
							Giver.Say(Description = $"Find and give this 'Special Item' to {vendor.Name} in {vendor?.Region}");

							Giver.Say(Description = $"I hide the item in your bag for safe keeping, {vendor.Name} will know where to look");

							return TaskNames.TakeTo;
						}
					case TaskNames.VisitPlace:
						{
							Giver.Say(Description = $"Find and visit with {vendor.Name} in {vendor?.Region}, tell them, I sent you!");

							return TaskNames.VisitPlace;
						}
					case TaskNames.TalkTo:
						{
							Giver.Say(Description = $"Find and give this 'Special Message' to {vendor.Name} in {vendor?.Region}, tell them, I sent you!");

							Giver.Say(Description = $"I hide the message in your bag for safe keeping, {vendor.Name} will know where to look");

							return TaskNames.TalkTo;
						}
					default: return TaskNames.None;
				}
			}

			if (tasktype == TaskTypes.Kill)
			{

				RandomThree += 30;

				switch ((TaskNames)RandomThree)
				{
					case TaskNames.KillTarget:
						{
							TargetMobile = TaskTargets.GetMobileTarget(1);

							var name = GetMobileName().EndsWith("s") ? GetMobileName() : "a " + GetMobileName();

							Giver.Say(Description = $"Find and kill {name}");

							TargetAmount = 1;

							return TaskNames.KillTarget;
						}
					case TaskNames.KillTen:
						{
							TargetMobile = TaskTargets.GetMobileTarget(2);

							Giver.Say(Description = $"Find and kill 10 {AddS(GetMobileName())}");

							TargetAmount = 10;

							return TaskNames.KillTen;
						}
					case TaskNames.KillTwenty:
						{
							TargetMobile = TaskTargets.GetMobileTarget(3);

							Giver.Say(Description = $"Find and kill 20 {AddS(GetMobileName())}");

							TargetAmount = 20;

							return TaskNames.KillTwenty;
						}
					default: return TaskNames.None;
				}
			}

			return TaskNames.None;
		}

		public string GetMobileName()
		{
			return InsertSpaces(TargetMobile?.GetType().ToString().Split('.').Last());
		}

		public string GetItemName()
		{
			return InsertSpaces(TargetItem?.GetType().ToString().Split('.').Last());
		}

		public void GiveSpecialTask()
		{
			// Reset
			IsComplete = false;
			IsAccepted = false;
			TargetItem = null;
			TargetLocation = new Point3D(0, 0, 0);

			// Add
			TargetMobile = TaskTargets.GetMobileTarget(1);
			TaskType = TaskTypes.Kill;
			TaskName = TaskNames.KillTwenty;
			TargetAmount = 20;
			Reward = TaskRewards.GetProfRewards(Giver.Title.Split(' ').Last()) as Item;

			if (Reward == null)
			{
				Reward = new GoldVendorSash(Giver.Title.Split(' ').Last());
			}

			Giver.Say(Description = $"Find and kill 20 {AddS(GetMobileName())}");

			TaskHandler.TaskList.Add(this);
		}

		private string InsertSpaces(string input)
		{
			if (input == null)
				return String.Empty;

			var output = new StringBuilder();

			for (var i = 0; i < input.Length; i++)
			{
				if (Char.IsUpper(input[i]) && i > 0)
				{
					output.Append(" ");
				}
				output.Append(input[i]);
			}

			return output.ToString();
		}

		public string AddS(string input)
		{
			if (input.EndsWith("s"))
				return input;
			else
				return input + "s";
		}

		public void Complete(PlayerMobile player, int bonus)
		{
			var reward = new Gold(10 * (int)TaskType * (int)TaskName + bonus);

			player.AddToBackpack(reward);

			if (TargetItem != null)
			{
				player.Backpack.AcquireItems().RemoveAll(t => t?.GetType() == TargetItem.GetType() && !t.Movable && t.Hue == 1174);
			}

			TaskHandler.TaskList.Remove(this);

			player.SendMessage($"Task complete, you received {reward} gold for your efforts");

			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Task Completed : {player.Name}", true);
			}
		}
	}
}
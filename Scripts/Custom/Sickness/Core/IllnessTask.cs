using System;
using Server.Engines.Sickness.RemedyTypes;
using Server.Mobiles;

namespace Server.Engines.Sickness
{
	public class IllnessTask
	{
		public bool TaskStarted { get; private set; }

		public bool HasTask { get; private set; }

		public string TargetToKill { get; private set; }

		public RemedyType Remedy { get; private set; }

		public bool AddTask(string target, RemedyType remedy)
		{
			if (target != String.Empty)
			{
				HasTask = true;
				TargetToKill = target;
				Remedy = remedy;

				return true;
			}

			HasTask = false;

			return false;
		}

		public bool StartTask()
		{
			if (TargetToKill != String.Empty)
			{
				TaskStarted = true;
			}
			else
			{
				TaskStarted = false;
			}

			return TaskStarted;
		}

		// Random Mob picker for Task
		public static string GetTargetToKill(int loc = 0)
		{
			var getSwitchNum = 0;

			if (loc != 0)
			{
				getSwitchNum = loc;
			}
			else
			{
				getSwitchNum = Utility.Random(1, 9);
			}

			switch (getSwitchNum)
			{
				case 1: return nameof(LichLord);
				case 2: return nameof(Dragon);
				case 3: return nameof(PlagueBeast);
				case 4: return nameof(BloodElemental);
				case 5: return nameof(Reaper);
				case 6: return nameof(Wisp);
				case 7: return nameof(ShadowWisp);
				case 8: return nameof(Phoenix);
				case 9: return nameof(Unicorn);
			}

			return String.Empty;
		}

		// Check Mob killed is for Task, if so then drap cure
		public static bool CheckToKill(string target, Mobile targetName)
		{
			if (target != null && targetName != null)
			{
				if (targetName.Name.ToLower().Contains(target.ToLower()))
					return true;
				else
					return false;
			}
			else
				return false;
		}
	}
}

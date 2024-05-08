using System;
using Server.Engines.Sickness.IllnessTypes;
using Server.Engines.Sickness.Items;
using Server.Engines.Sickness.RemedyTypes;
using Server.Mobiles;

namespace Server.Engines.Sickness
{
	public static class IllnessDeath
	{
		// SickChance is the percentage chance that an illness
		// will be spawned when a creature dies
		private const int SickChance = IllnessSettings.SickChance;

		// CureLootChance is the chance for a cure item to drop on
		// a creature's death, 10% of n
		private const int CureLootChance = IllnessSettings.CureLootChance;

		// EventSink_CreatureDeath is the event handler for
		// the CreatureDeath event. It has a chance to spawn
		// an illness and also has a chance to drop a
		// cure item on the creature's corpse.
		public static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
		{
			if (e.Creature.LastKiller is PlayerMobile player && IllnessHandler.ContainsPlayer(player.Name))
			{
				var taskInfo = IllnessHandler.GetPlayerIllness(player.Name).TaskInfo;

				if (IllnessTask.CheckToKill(taskInfo.TargetToKill, e.Creature))
				{
					RemedyUtility.AddTaskCure(taskInfo.Remedy, e.Corpse);

					IllnessHandler.GetPlayerIllness(player.Name).TaskInfo = new IllnessTask();

					if (IllnessHandler.InDebug)
					{
						var msg = $"[ Added: {e.Creature.Name} =>  ( {taskInfo.Remedy} ) ]";

						IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
					}
				}

				if (Utility.RandomMinMax(0, 100) < SickChance) // chance to spawn sickness
				{
					SicknessUtility.AddSicknessToWorld(e.Creature, SicknessUtility.GetRandomSickness());

					if (IllnessHandler.InDebug)
					{
						var msg = $"[ Added: {e.Creature.Name} =>  ( Wandering Sickness ) ]";

						IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
					}
				}

				if (Utility.Random(0, CureLootChance) < 10) // .01% chance to drop cure item!
				{
					RemedyUtility.AddRandomCure(e.Creature, e.Corpse);

					if (Utility.Random(0, CureLootChance * 2) < 100)
					{
						if (Utility.Random(0, CureLootChance) < 100)
						{
							e.Corpse.AddItem(new MedicCloth());
						}
						else
						{
							e.Corpse.AddItem(new Thermometer());
						}
					}

					if (IllnessHandler.InDebug)
					{
						var msg = $"[ Added: {e.Creature.Name} =>  ( Random Cure ) ]";

						IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
					}
				}
			}
		}
	}
}

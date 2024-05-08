using System;
using Server.Engines.Sickness.IllnessTypes;

namespace Server.Engines.Sickness
{
	public class IllnessDataStore
	{
		// FilePath is the file where the PlayerIllnessDict will
		// be serialized to and deserialized from
		private static readonly string FilePath = IllnessSettings.FilePath;

		// OnSave is the event handler for the WorldSave event.
		// It serializes the PlayerIllnessDict to a file.
		public static void OnSave(WorldSaveEventArgs e)
		{
			Persistence.Serialize(FilePath, OnSerialize);
		}

		// OnLoad is the event handler for the WorldLoad event.
		// It deserializes the PlayerIllnessDict from a file.
		public static void OnLoad()
		{
			Persistence.Deserialize(FilePath, OnDeserialize);
		}

		// OnSerialize is a callback method used by the Persistence
		// class to serialize the PlayerIllnessDict
		private static void OnSerialize(GenericWriter writer)
		{
			writer.Write(IllnessVersion.SysVersion);

			lock (IllnessHandler.DictLock)
			{
				writer.Write(IllnessHandler.PlayerIllnessDict.Count);

				writer.Write(IllnessHandler.InDebug);

				foreach (var kvp in IllnessHandler.PlayerIllnessDict)
				{
					writer.Write(kvp.Key);

					writer.Write(kvp.Value.SicknessList.Count);

					foreach (var sickness in kvp.Value.SicknessList)
					{
						writer.Write((int)sickness.Sickness);

						writer.Write(sickness.Severity);

						writer.Write(sickness.SymptomDamage);

						writer.Write(sickness.SymptomDelay);

						writer.Write(sickness.IsCured);

						writer.Write(sickness.IsDiscovered);
					}
				}
			}
		}

		// OnDeserialize is a callback method used by the Persistence
		// class to deserialize the PlayerIllnessDict
		private static void OnDeserialize(GenericReader reader)
		{
			var saveVer = reader.ReadInt();

			if (saveVer != IllnessVersion.SysVersion)
			{
				IllnessVersion.CleanUpOldSaves();
			}

			var numPlayers = reader.ReadInt();

			IllnessHandler.InDebug = reader.ReadBool();

			for (var i = 0; i < numPlayers; i++)
			{
				var player = reader.ReadString();

				var illness = new Illness(player);

				var numIllnesses = reader.ReadInt();

				var isCured = 0;

				var isNotCured = 0;

				for (var j = 0; j < numIllnesses; j++)
				{
					var sickness = SicknessUtility.GetSickness(reader.ReadInt());

					sickness.Severity = reader.ReadInt();

					sickness.SymptomDamage = reader.ReadInt();

					sickness.SymptomDelay = reader.ReadInt();

					sickness.IsCured = reader.ReadBool();

					if (sickness.IsCured)
					{
						isCured++;
					}
					else
					{
						isNotCured++;
					}

					sickness.IsDiscovered = reader.ReadBool();

					illness.SicknessList.Add(sickness);
				}

				IllnessHandler.AddPlayerIllness(player, illness);

				string msg;

				if (IllnessHandler.InDebug)
				{
					if (isCured > 0 || isNotCured > 0)
					{
						msg = $"[ {player} : Loading Infections : {isNotCured} Active - {isCured} Inactive ] ...";
					}
					else
					{
						msg = $"[ {player} : Loading 0 Infections ... ]";
					}

					IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
				}
			}
		}
	}
}

using System.IO;

namespace Server.Engines.Sickness
{
	public class IllnessVersion
	{
		// Version
		public static int SysVersion = IllnessSettings.SysVersion;

		// Method to clean up old saves, do not use/edit, will be removed once system is complete!
		public static void CleanUpOldSaves()
		{
			var oldfilePath = Path.Combine("Saves", "Illness.bin");

			if (File.Exists(oldfilePath))
			{
				File.Delete(oldfilePath);
			}

			for (var i = 0; i < SysVersion - 1; i++)
			{
				var filePath = Path.Combine(@"Saves\Sickness101", $"Sickness{i}.bin");

				if (File.Exists(filePath) && i != SysVersion)
				{
					File.Delete(filePath);
				}
			}
		}
	}
}

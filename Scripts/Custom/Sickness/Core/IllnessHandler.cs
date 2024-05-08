using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Engines.Sickness.IllnessTypes;
using Server.Services.Custom.Sickness;

namespace Server.Engines.Sickness
{
	// IllnessHandler is a static class that handles the creation,
	// management, and storage of illnesses in the world. It listens
	// for various events in the game and takes appropriate
	// action when those events occur.
	public static class IllnessHandler
	{
		// PlayerIllnessDict stores the illnesses of all players
		// in the game, keyed by their name
		static readonly internal Dictionary<string, Illness> PlayerIllnessDict = new Dictionary<string, Illness>();

		// DictLock is an object used to synchronize access
		// to the PlayerIllnessDict
		static readonly internal object DictLock = new object();

		// InDebug is a flag that controls whether debug messages
		// will be printed to the console
		public static bool InDebug { get; set; } = IllnessSettings.InDebug;

		// Configure sets up the IllnessHandler by
		// subscribing to various events
		public static void Configure()
		{
			PrintVersion();
			SubscribeToWorldSave();
			SubscribeToWorldLoad();
			SubscribeToLogin();
			SubscribeToCreatureDeath();
			SubscribeToHungerChanged();

			if (InDebug)
			{
				var msg = "[ Main Dictionary Created ]";

				IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
			}
		}

		// Print the version number and running message
		private static void PrintVersion()
		{
			var msg = $"Version [ 1.0.0.{IllnessVersion.SysVersion} ]";

			IllnessUtility.ToConsole(msg, ConsoleColor.DarkMagenta);

			msg = "Running...";

			IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
		}

		// Subscribe to the world save event 
		private static void SubscribeToWorldSave()
		{
			EventSink.WorldSave += IllnessDataStore.OnSave;
		}

		// Subscribe to the world load event
		private static void SubscribeToWorldLoad()
		{
			EventSink.WorldLoad += IllnessDataStore.OnLoad;
		}

		// Subscribe to the login event
		private static void SubscribeToLogin()
		{
			EventSink.Login += IllnessLogin.EventSink_GameLogin;
		}

		// Subscribe to the creature death event
		private static void SubscribeToCreatureDeath()
		{
			EventSink.CreatureDeath += IllnessDeath.EventSink_CreatureDeath;
		}

		// Subscribe to the hunger changed event
		private static void SubscribeToHungerChanged()
		{
			EventSink.HungerChanged += IllnessHunger.EventSink_HungerChanged;
		}

		// ContainsKey returns true if the PlayerIllnessDict
		// contains an entry for the given player name.
		public static bool ContainsPlayer(string player)
		{
			if (player != null)
			{
				return PlayerIllnessDict.ContainsKey(player);
			}
			else
			{
				return false;
			}
		}

		// AddIllness adds a new Illness object to the
		// PlayerIllnessDict for the given player name.
		public static void AddPlayerIllness(string player, Illness illness)
		{
			if (player != null && !ContainsPlayer(player))
			{
				lock (DictLock)
				{
					PlayerIllnessDict.Add(player, illness);
				}
			}
		}

		// ResetIllness removes all illnesses from a player's
		// IllnessList and clears their symptoms.
		public static bool ResetPlayerIllness(PlayerMobile player)
		{
			if (player != null && ContainsPlayer(player.Name))
			{
				lock (DictLock)
				{
					PlayerIllnessDict.Remove(player.Name);

					AddPlayerIllness(player.Name, new Illness(player.Name));

					if (InDebug)
					{
						var msg = $"[ RESET: {player.Name} => New Illness Made ]";

						IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
					}

					return true;
				}
			}

			return false;
		}

		// Reset Dictionary
		public static bool ResetAll()
		{
			lock (DictLock)
			{
				if (PlayerIllnessDict.Count > 0)
				{
					PlayerIllnessDict.Clear();

					return true;
				}
			}

			return false;
		}

		// GetIllness returns the Illness object for the given
		// player name from the PlayerIllnessDict.
		public static Illness GetPlayerIllness(string player)
		{
			if (player != null && ContainsPlayer(player))
			{
				lock (DictLock)
				{
					return PlayerIllnessDict[player];
				}
			}

			return null;
		}

		// UpdateIllness adds or updates an illness for a player.
		// If the player already has the illness, it will
		// overwrite the existing one with the new one.
		public static void UpdatePlayerIllness(PlayerMobile player, BaseSickness sickness)
		{
			if (player != null && ContainsPlayer(player.Name))
			{
				lock (DictLock)
				{
					PlayerIllnessDict[player.Name].TryInfection(player, sickness);

					if (InDebug)
					{
						var msg = $"[ Updated: {player.Name} => ( {sickness.IllName} ) ]";

						IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
					}
				}
			}
		}
	}
}

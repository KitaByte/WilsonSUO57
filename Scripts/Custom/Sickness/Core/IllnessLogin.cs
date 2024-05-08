using Server.Mobiles;
using System;

namespace Server.Engines.Sickness
{
	public static class IllnessLogin
	{
		// EventSink_GameLogin is the event handler for the Login event.
		// It adds a new illness for the player if they don't already have
		// one, or adds a "None" sickness if they don't already have one.
		public static void EventSink_GameLogin(LoginEventArgs e)
		{
			if (e.Mobile is PlayerMobile player)
			{
				if (player.Hunger <= 1)
				{
					player.Hunger = 2;
				}

				if (player != null && !IllnessHandler.ContainsPlayer(player.Name))
				{
					var newIllness = new Illness(player.Name)
					{
						LastImmunity = IllnessImmunity.PlayerImmunityLevel(player)
					};

					IllnessHandler.AddPlayerIllness(player.Name, newIllness);

					if (IllnessHandler.InDebug)
					{
						var msg = $"[ Login/Added: {player.Name} =>  ( Sickness System Directory ) ]";

						IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
					}
				}
				else if (IllnessHandler.InDebug)
				{
					var msg = $"[ Login: {player.Name} =>  ( Sickness System Directory ) ]";

					IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
				}
			}
		}
	}
}

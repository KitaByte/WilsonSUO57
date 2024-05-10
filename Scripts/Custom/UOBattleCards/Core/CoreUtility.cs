using System;
using Server.Mobiles;

namespace Server.Services.UOBattleCards.Core
{
    public static class CoreUtility
    {
        public static RoundTimer GameTimer { get; private set; }

        public static void Initialize()
        {
            GameTimer = new RoundTimer();

            EventSink.ServerStarted += CoreLoadUtility.EventSink_ServerStarted;

            EventSink.CreatureDeath += CoreDeathUtility.EventSink_CreatureDeath;

            EventSink.BeforeWorldSave += CoreSaveUtility.EventSink_BeforeWorldSave;

            EventSink.AfterWorldSave += CoreSaveUtility.EventSink_AfterWorldSave;

            EventSink.Shutdown += CoreSaveUtility.EventSink_Shutdown;

            EventSink.Crashed += CoreSaveUtility.EventSink_Crashed;

			EventSink.Login += EventSink_Login;

			EventSink.Logout += EventSink_Logout;
        }

		private static void EventSink_Login(LoginEventArgs e)
		{
			if (e.Mobile is PlayerMobile pm)
			{
				if (PlayerUtility.PlayerStats?.Count > 0)
				{
					if (PlayerUtility.PlayerStats.Find(p => p.Name == pm.Name) == null)
					{
						PlayerUtility.PlayerStats.Add(new PlayerInfo(pm.Name));

						// todo : Add Gump to intro game & offer instructions 
					}
				}
			}
		}

		private static void EventSink_Logout(LogoutEventArgs e)
		{
			if (e.Mobile is PlayerMobile pm)
			{
				if (MatchUtility.InMatch(pm))
				{
					var match = MatchUtility.GetMatchInfo(pm);

					if (match != null)
					{
						match.RoundTimeSet = 0;

						match.PlayerOneReady = true;

						match.PlayerTwoReady = true;

						if (match.PlayerOne == pm)
						{
							match.RoundEndedBy = PlayerTypes.PlayerOne;

							match.P1GameGumpHandle?.EndMatch();
						}
						else
						{
							match.RoundEndedBy = PlayerTypes.PlayerTwo;

							match.P2GameGumpHandle?.EndMatch();
						}
					}
				}
			}
		}

		public static PlayerMobile AntiTheftCheck(Mobile user, Mobile owner, Item item)
		{
			if (owner == null)
			{
				owner = user;

				user.SendMessage(52, $"You are now marked as the owner of this {item.Name}");
			}

			if (user != owner && user.AccessLevel == AccessLevel.Player)
			{
				ReturnToOwner(user, item, owner);
			}

            return owner as PlayerMobile;
        }

		private static void ReturnToOwner(Mobile player, Item item, Mobile owner)
		{
			owner.AddToBackpack(item);

			player.SendMessage(42, "This is not yours, returning to Owner!");
		}

		public static void LogMessage(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;

            Console.WriteLine(message);

            Console.ResetColor();
        }

		public static void StartTimer()
        {
            if (GameTimer == null)
            {
                GameTimer = new RoundTimer();
            }

            GameTimer.Start();

            LogMessage(ConsoleColor.Magenta, "UO Battle Cards : [Game Timer - Started]");
        }

		public static void StopTimer()
        {
            GameTimer?.Stop();

            LogMessage(ConsoleColor.DarkMagenta, "UO Battle Cards : [Game Timer - Stopped]");
        }

		public static void RestartTimer()
        {
            if (GameTimer == null)
            {
                GameTimer = new RoundTimer();
            }

            GameTimer.Start();
        }
    }
}

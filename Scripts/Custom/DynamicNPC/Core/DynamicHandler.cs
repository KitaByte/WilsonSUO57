using System;
using System.Linq;
using System.Text;
using Server.Items;
using Server.Services.DynamicNPC.Tasks;

namespace Server.Services.DynamicNPC
{
	static internal class DynamicHandler
	{
		private static DynamicClock clock;

		public static void Configure()
		{
			MsgToConsole($"Version [1.0.0.{DynamicSettings.Version}]", false);

			// Config

			EventSink.WorldLoad += ProfileStore.OnWorldLoad;

			// Store

			EventSink.WorldSave += ProfileStore.OnSave;

			EventSink.WorldLoad += ProfileStore.OnLoad;

			// Vendor

			EventSink.MobileCreated += DynamicVendorMobile.OnVendorCreated;

			EventSink.MobileDeleted += DynamicVendorMobile.OnVendorDeleted;

			EventSink.ValidVendorPurchase += DynamicVendorBuy.OnVendorBuy;

			EventSink.ValidVendorSell += DynamicVendorSell.OnVendorSell;

			// Task

			EventSink.CreatureDeath += TaskHandler.OnCreatureDeath;

			EventSink.OnItemObtained += TaskHandler.OnItemObtained;

			EventSink.Speech += TaskHandler.OnSpeech;

			// Timer

			clock = new DynamicClock();

			clock.OnTickClock += OnTickEventMethod;

			// Debug

			if (DynamicSettings.InDebug)
			{
				MsgToConsole("Configured & Running...", true);
			}
		}

		private static void OnTickEventMethod()
		{
			OnTickCheck();

			if (DynamicSettings.InDebug)
			{
				MsgToConsole("Main Clock Tick...", true);
			}
		}

		private static void OnTickCheck()
		{
			var profiles = ProfileStore.GetAllProfiles();

			for (var i = 0; i < profiles.Count(); i++)
			{
				profiles[i].UpdateVendor();
			}
		}

		public static string Capitalize(string text)
		{
			var chars = text.ToCharArray();

			var SB = new StringBuilder();

			var counter = 0;

			foreach (var letter in chars)
			{
				if (counter == 0)
				{
					SB.Append(letter.ToString().ToUpper());
				}
				else
				{
					SB.Append(letter);
				}

				counter++;
			}

			return SB.ToString();
		}

		public static bool IsNight(Mobile m)
		{
            if (DynamicSettings.WorksNight)
            {
                return false;
            }

			Clock.GetTime(m, out var locTime, out _);

			if (locTime == 1042957) // It's late at night
			{
				return true;
			}

			if (locTime == 1042950) // 'Tis the witching hour, 12 Midnight
			{
				return true;
			}

			if (locTime == 1042951) // It's the middle of the night
			{
				return true;
			}

			if (locTime == 1042952) // It's early in the morning
			{
				return true;
			}

			return false;
		}

		public static void MsgToConsole(string msg, bool debug)
		{
			if (!debug)
			{
				Console.ForegroundColor = ConsoleColor.DarkMagenta;

				Console.WriteLine($"Engine: Dynamic NPC: {msg}");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;

				Console.WriteLine($"Engine: Dynamic NPC: Debug => {msg}");
			}

			Console.ResetColor();
		}
	}
}

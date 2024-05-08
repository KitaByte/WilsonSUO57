using Server.Engines.Sickness;
using Server.Mobiles;

namespace Server.Services.Custom.Sickness
{
	public class IllnessHunger
	{
		// Min that immunity must be before player can catch infections
		private const int MinImmunity = IllnessSettings.MinImmunity;

		// EventSink_HungerChanged is the event handler
		// for the HungerChanged event. It sends symptoms
		// to the player based on their current immune level.
		public static void EventSink_HungerChanged(HungerChangedEventArgs e)
		{
			if (e.Mobile is PlayerMobile player && IllnessHandler.ContainsPlayer(e.Mobile.Name))
			{
				string logTick;

				if (!BounceHunger(ref player))
				{
					var illness = IllnessHandler.GetPlayerIllness(player.Name);

					var immunity = IllnessImmunity.CheckHungerThirst(player);

					if (illness.LastImmunity > immunity && illness.SicknessList.Count > 0)
					{
						illness.TryDamage(ref player);

						if (IllnessHandler.InDebug)
						{
							player.SendMessage(53, $"Hunger/Thirst (-{immunity}: Damage Check Tick-)");
						}
					}
					else if (immunity < MinImmunity)
					{
						IllnessImmunity.TryAttackImmune(ref player);

						if (IllnessHandler.InDebug)
						{
							player.SendMessage(53, $"Hunger/Thirst (-{immunity}: Attack Check Tick-)");
						}
					}

					illness.LastImmunity = immunity;

					logTick = "Main";
				}
				else
				{
					logTick = "Bounced";
				}

				if (IllnessHandler.InDebug)
				{
					if (player.AccessLevel > AccessLevel.Counselor)
					{
						player.SendMessage(53, $"Hunger/Thirst (-{logTick} Tick-)");
					}
				}
			}
		}

		// Ensure hunger never reaches 0 as the event will never call if Hunger = 0
		// Doubles as a small immunity booster as if thirst high, then the bounce 
		// can take it to 5 max instead of 2 which makes it extremely easy to get sick.
		private static bool BounceHunger(ref PlayerMobile player)
		{
			if (player.Hunger <= 1)
			{
				if (player.Thirst >= 12)
					player.Hunger = player.Thirst / 4;
				else
				{
					player.Hunger = 2;
				}

				return true;
			}

			return false;
		}
	}
}

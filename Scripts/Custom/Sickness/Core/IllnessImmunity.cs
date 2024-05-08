using System;
using Server.Mobiles;
using Server.Misc;
using Server.Engines.Sickness.IllnessTypes;

namespace Server.Engines.Sickness
{

	// IllnessImmunity is a static class that contains methods
	// related to player immunity to sicknesses.
	public static class IllnessImmunity
	{
		// Adjustment for Immunity, 4 default, + would make it easier to catch and - harder to catch
		private static readonly int ImmunityMod = IllnessSettings.ImmunityMod;

		// PlayerImmunityLevel returns the (n / 10) base of the
		// player's hunger and thirst values.
		public static int PlayerImmunityLevel(PlayerMobile player)
		{
			return (player.Hunger + player.Thirst) / ImmunityMod;
		}

		// IsImmune checks whether the given player is immune
		// to the given sickness. If the player has a
		// high enough immunity level, or if the player
		// is a staff member, then they are immune.
		public static bool IsImmune(BaseSickness sickness, PlayerMobile player)
		{
			if (player.AccessLevel > AccessLevel.Player) // Staff is immune
			{
				if (IllnessHandler.InDebug)
				{
					return false;
				}
				else
				{
					return true;
				}
			}

			if (sickness.Severity > PlayerImmunityLevel(player))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		// TryAttackImmune checks whether the given player is immune
		// to an sickness attack. It takes into account the player's
		// current weather and clothing layers, as well as the
		// player's immunity level. If the player is not immune,
		// it will infect them with an sickness.
		public static void TryAttackImmune(ref PlayerMobile player)
		{
			var immunity = PlayerImmunityLevel(player);

			if (Utility.Random(0, 10) > immunity)
			{
				var infectionChance = 0;

				// Makes up 50% of the overall chance!
				if (HasWeather(player, out var weatherChance))
				{
					if (weatherChance > 50)
						infectionChance += 50;
					else
						infectionChance += weatherChance;
				}

				// Makes up 50% of the overall chance!
				if (HasLayers(player, out var layerChance))
				{
					if (layerChance > 50)
						infectionChance += 50;
					else
						infectionChance += layerChance;
				}

				// Checks overall chance against immunity level via Random Range!
				if (infectionChance >= Utility.Random(immunity, immunity * 10))
				{
					IllnessHandler.UpdatePlayerIllness(player, SicknessUtility.GetRandomSickness());

					if (IllnessHandler.InDebug)
					{
						var msg = $"[ Immune Attacked: {player.Name} has caught something! ]";

						IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
					}
				}
			}
		}

		// HasWeather checks whether the given player is
		// currently in a weather region and, if so, returns
		// the chance of the player getting sick
		// based on the weather.
		private static bool HasWeather(PlayerMobile player, out int chance)
		{
			var weatherList = Weather.GetWeatherList(player.Map);

			foreach (var weather in weatherList)
			{
				if (weather.Bounds.Contains(player.Location))
				{
					var temp = weather.Temperature;

					if (temp < 0)
					{
						temp *= -1;
					}

					var chacePercipitation = weather.ChanceOfPercipitation;

					var chaceExtremeTemp = weather.ChanceOfPercipitation;

					var total = temp + chacePercipitation + chaceExtremeTemp;

					chance = temp + chacePercipitation + chaceExtremeTemp;

					return true;
				}
			}

			chance = 0;

			return false;
		}

		// HasLayers checks the player's equipped clothing
		// layers and returns the chance of the player getting
		// sick based on those layers.
		private static bool HasLayers(PlayerMobile player, out int chance)
		{
			var startChnace = 0;

			startChnace += GetLayerChance(player, Layer.Shoes);
			startChnace += GetLayerChance(player, Layer.InnerLegs);
			startChnace += GetLayerChance(player, Layer.OuterLegs);
			startChnace += GetLayerChance(player, Layer.InnerTorso);
			startChnace += GetLayerChance(player, Layer.MiddleTorso);
			startChnace += GetLayerChance(player, Layer.OuterTorso);
			startChnace += GetLayerChance(player, Layer.Arms);
			startChnace += GetLayerChance(player, Layer.Gloves);
			startChnace += GetLayerChance(player, Layer.Helm);
			startChnace += GetLayerChance(player, Layer.Hair);
			startChnace += GetLayerChance(player, Layer.FacialHair);

			if (startChnace > 0)
			{
				chance = startChnace;

				return true;
			}
			else
			{
				chance = 0;

				return false;
			}
		}

		// GetLayerChance returns the chance of the player
		// getting sick based on their equipped item in the
		// given layer. If the player has no item equipped
		// in that layer, or if the item is not a clothing
		// item, then it returns 0.
		private static int GetLayerChance(PlayerMobile player, Layer layer)
		{
			var layerChance = 0;

			if (player.FindItemOnLayer(layer) == null)
			{
				layerChance += 5;
			}

			return layerChance;
		}

		// Hunger/Thirst Warnings
		public static int CheckHungerThirst(PlayerMobile player)
		{
			if (player.Thirst < 10)
			{
				if (player.Thirst < 5)
				{
					player.Say("*lips crack*");

					player.SendMessage("You are in dire need of some water!");
				}
				else
				{
					player.Say("*mouth drys*");

					player.SendMessage("You are in need of some water!");
				}
			}

			if (player.Hunger < 10)
			{
				if (player.Hunger < 5)
				{
					player.Say("*stomach cramps*");

					player.SendMessage("You are in dire need of some food!");
				}
				else
				{
					player.Say("*stomach ache*");

					player.SendMessage("You are in need of some food!");
				}
			}

			return PlayerImmunityLevel(player);
		}
	}
}

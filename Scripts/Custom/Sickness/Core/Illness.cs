using System;
using System.Linq;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Engines.Sickness.IllnessTypes;

namespace Server.Engines.Sickness
{
	// Illness represents an illness that holds player Infections.
	// It stores the illness's owner, a list of BaseSickness
	// objects representing the sickness effects, and the
	// player's last combined hunger and thirst level (Immunity).

	public class Illness
	{
		// Owner is the player who has this illness
		private string Owner { get; set; }

		// TaskInfo : when accepted by medic is stored here
		public IllnessTask TaskInfo { get; set; }

		// IllnessList is a list of BaseSickness objects
		// representing the different effects of this sickness
		public List<BaseSickness> SicknessList { get; set; }

		// LastImmunity is the player's last
		// combined hunger and thirst level
		public int LastImmunity { get; set; }

		// DelayCounter is a counter used to delay
		// the application of the sickness effects
		private int DelayCounter { get; set; }

		// Illness constructor that takes the
		// owner's name as a parameter
		public Illness(string owner)
		{
			Owner = owner;

			TaskInfo = new IllnessTask();

			SicknessList = new List<BaseSickness>();

			DelayCounter = 0;
		}

		// TryInfection adds the given sickness to the player's SicknessList.
		// If the player already has the sickness, it will overwrite the existing
		// one with the new one. If the player is immune to the sickness, it will not be added.
		// It also considers if the player had the sickness before and if it was cured,
		// the new severity will be half of the original infection
		public void TryInfection(PlayerMobile player, BaseSickness sickness)
		{
			if (!IllnessImmunity.IsImmune(sickness, player))
			{
				if (SicknessList.Any(BaseSickness => BaseSickness.IllName == sickness.IllName))
				{
					BaseSickness sickToRemove = null;

					foreach (var infection in SicknessList)
					{
						if (infection.IllName == sickness.IllName)
						{
							sickToRemove = infection;

							break;
						}
					}

					// If player already had this infection, it has less severity!
					if (sickToRemove != null && sickToRemove.IsCured)
					{
						SicknessList.Remove(sickToRemove);

						sickness.Severity /= 2;

						AddSickness(player, sickness);
					}
				}
				else
				{
					AddSickness(player, sickness);
				}
			}
		}

		// AddInfection adds the given sickness to the
		// player's SicknessList and prints a debug message
		// if InDebug is true.
		private void AddSickness(PlayerMobile player, BaseSickness sickness)
		{
			SicknessList.Add(sickness);

			if (IllnessHandler.InDebug)
			{
				var msg = $"[ Infected: {player.Name} with {sickness.IllName} ]";

				IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
			}
		}

		// GetFirstInfection returns the first non-cured
		// sickness in the player's SicknessList, null if none!
		public BaseSickness GetFirstInfection()
		{
			if (SicknessList.Count > 0)
			{
				foreach (var sickness in SicknessList)
				{
					if (!sickness.IsCured)
					{
						return sickness;
					}
				}
			}

			return null;
		}

		// GetActiveTotal returns the total of non-cured (active) and
		// cured (inActive) in the player's SicknessList.
		public int GetActiveTotal(out int inActive)
		{
			var activeCount = 0;

			var inActiveCount = 0;

			if (SicknessList.Count > 0)
			{
				foreach (var sickness in SicknessList)
				{
					if (sickness.IsCured)
					{
						inActiveCount++;
					}
					else
					{
						activeCount++;
					}
				}
			}

			inActive = inActiveCount;

			return activeCount;
		}

		// TryDamage applies the effects of the player's
		// active sickness to the player. It will delay the
		// application of the effects if necessary, and apply
		// the effects if the delay has been reached. It
		// also has a chance to spread the sickness to other
		// players if the sickness is contagious.
		public void TryDamage(ref PlayerMobile player)
		{
			var sickness = GetFirstInfection();

			if (sickness != null && DelayCounter >= sickness.SymptomDelay)
			{
				IllnessEmote.RunAnimation(ref player, true);

				var InfectionDamage = sickness.SymptomDamage * sickness.Severity;

				var PlayerHealthMod = (player.Hunger + player.Thirst) / 4;

				player.Damage(InfectionDamage / PlayerHealthMod);

				if (sickness.IsContagious)
				{
					if (Utility.Random(100) < sickness.Severity)
					{
						SicknessUtility.AddSicknessToWorld(player, sickness);
					}
				}

				if (IllnessHandler.InDebug)
				{
					var msg = $"[ Damaged: {player.Name} for {InfectionDamage / PlayerHealthMod} ]";

					IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
				}

				DelayCounter = 0;
			}
			else
			{
				DelayCounter++;
			}
		}

		// TryDiscovery tries to discover a cure for the player's
		// active sickness. It has a chance to discover
		// a cure based on the player's chance + Medic Level,
		// and if a cure is found it will update the
		// sickness from the player's SicknessList.
		public bool TryDiscovery(ref PlayerMobile player, int chance, out BaseSickness sickness)
		{
			sickness = GetFirstInfection();

			if (sickness != null && Utility.Random(0, 99) < chance)
			{
				if (SicknessList.Contains(sickness) && sickness.IsDiscovered == false)
				{
					IllnessEmote.RunAnimation(ref player, true);

					sickness.IsDiscovered = true;

					if (IllnessHandler.InDebug)
					{
						var msg = $"[ Discovered: {player.Name} has {sickness} ]";

						IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
					}

					return true;
				}
			}
			
			return false;
		}

		// TryCure tries to cure the player's active sickness
		// with the given cure item. It has a chance to cure
		// the sickness based on the cure item's cure chance,
		// and if a cure is successful it will remove the
		// sickness from the player's SicknessList.
		public bool TryCure(PlayerMobile player, BaseRemedy remedy, out bool wasCured)
		{
			BaseSickness sickness = null;

			if (SicknessList.Count > 0)
			{
				foreach (var infection in SicknessList)
				{
					if (infection.IsDiscovered)
					{
						if (infection.Remedy == remedy.RemType)
						{
							sickness = infection;

							break;
						}
					}
				}
			}

			if (sickness != null)
			{
				if (sickness.IsDiscovered && !sickness.IsCured)
				{
					if (sickness.Severity > remedy.RemPotency)
					{
						sickness.Severity -= remedy.RemPotency;
					}
					else
					{
						sickness.IsCured = true;

						if (IllnessHandler.InDebug)
						{
							var msg = $"[ Cured: {player.Name} of {sickness.IllName} ]";

							IllnessUtility.ToConsole(msg, ConsoleColor.DarkCyan);
						}
					}
				}

				wasCured = sickness.IsCured;

				return sickness.IsDiscovered && !sickness.IsCured;
			}
			else
			{
				wasCured = false;

				return false;
			}
		}

		public int GetImmunityLevel(PlayerMobile player)
		{
			return IllnessImmunity.PlayerImmunityLevel(player);
		}

		// Returns - "Player Name + Sickness List" // Not used currently in code, place holder
		public override string ToString()
		{
			return $"{Owner}'s Sickness List";
		}
	}
}

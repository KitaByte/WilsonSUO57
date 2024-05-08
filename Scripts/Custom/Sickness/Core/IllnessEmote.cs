using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Engines.Sickness
{
	public static class IllnessEmote
	{
		// Runs animation on player
		public static void RunAnimation(ref PlayerMobile player, bool isSymptom)
		{
			(string, int) symptoms;

			if (isSymptom)
			{
				symptoms = SymptomEmote(player.Female);
			}
			else
			{
				symptoms = CureEmote(player.Female);
			}

			player.Say(symptoms.Item1);

			player.PlaySound(symptoms.Item2);

			player.Animate(RandomAnimType(), Utility.RandomMinMax(0, 1));
		}

		// Returns a tuple(string, int) emote/sound for sickness
		public static (string, int) SymptomEmote(bool isFemale)
		{
			switch (Utility.Random(20))
			{
				case 0: return ("*aches*", isFemale ? 811 : 1085);
				case 1: return ("*feverish*", isFemale ? 795 : 1067);
				case 2: return ("*cough*", isFemale ? 785 : 1056);
				case 3: return ("*sneeze*", isFemale ? 817 : 1091);
				case 4: return ("*nauseous*", isFemale ? 811 : 1085);
				case 5: return ("*vomit*", isFemale ? 813 : 1087);
				case 6: return ("*headache*", isFemale ? 811 : 1085);
				case 7: return ("*stomachache*", isFemale ? 811 : 1085);
				case 8: return ("*fatigue*", isFemale ? 793 : 1065);
				case 9: return ("*sore throat*", isFemale ? 784 : 1055);
				case 10: return ("*congestion*", isFemale ? 795 : 1067);
				case 11: return ("*sickly*", isFemale ? 795 : 1067);
				case 12: return ("*runny nose*", isFemale ? 817 : 1091);
				case 13: return ("*watery eyes*", isFemale ? 817 : 1091);
				case 14: return ("*earache*", isFemale ? 795 : 1067);
				case 15: return ("*backache*", isFemale ? 795 : 1067);
				case 16: return ("*chest congestion*", isFemale ? 795 : 1067);
				case 17: return ("*sweats*", isFemale ? 793 : 1065);
				case 18: return ("*chills*", isFemale ? 795 : 1067);
				default: return ("*dizzy*", isFemale ? 799 : 1071);
			}
		}

		// Returns a tuple(string, int) emote/sound for cure
		public static (string, int) CureEmote(bool isFemale)
		{
			switch (Utility.Random(20))
			{
				case 0: return ("*thankful*", isFemale ? 783 : 1054);
				case 1: return ("*ecstatic*", isFemale ? 784 : 1055);
				case 2: return ("*healthy*", isFemale ? 778 : 1051);
				case 3: return ("*miraculous*", isFemale ? 783 : 1054);
				case 4: return ("*great*", isFemale ? 784 : 1055);
				case 5: return ("*grateful*", isFemale ? 778 : 1051);
				case 6: return ("*blessed*", isFemale ? 783 : 1054);
				case 7: return ("*relieved*", isFemale ? 784 : 1055);
				case 8: return ("*overjoyed*", isFemale ? 778 : 1051);
				case 9: return ("*elated*", isFemale ? 783 : 1054);
				case 10: return ("*thankful*", isFemale ? 784 : 1055);
				case 11: return ("*free*", isFemale ? 778 : 1051);
				case 12: return ("*alive*", isFemale ? 783 : 1054);
				case 13: return ("*whole*", isFemale ? 784 : 1055);
				case 14: return ("*reborn*", isFemale ? 778 : 1051);
				case 15: return ("*new*", isFemale ? 783 : 1054);
				case 16: return ("*cured at last*", isFemale ? 784 : 1055);
				case 17: return ("*healthy again*", isFemale ? 778 : 1051);
				case 18: return ("*restored*", isFemale ? 783 : 1054);
				default: return ("*saved*", isFemale ? 784 : 1055);
			}
		}

		// Animation types used randomly for both sickness and cures
		public static AnimationType RandomAnimType()
		{
			var animType = new List<AnimationType>()
			{
				AnimationType.Block,
				AnimationType.Impact,
				AnimationType.Eat,
				AnimationType.Emote,
				AnimationType.Parry,
				AnimationType.Die,
				AnimationType.Alert,
				AnimationType.Fidget,
				AnimationType.Spell
			};

			var rnd = Utility.RandomMinMax(0, animType.Count - 1);

			return animType[rnd];
		}
	}
}

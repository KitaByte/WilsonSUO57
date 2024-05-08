using System;
using Server.Engines.Sickness.Mobiles;

namespace Server.Engines.Sickness.IllnessTypes
{
	// When adding in or removing sicknesses, ensure
	// you match up a remedy in the same position

	// Enum containing the different types of sickness
	public enum SicknessType
	{
		[Description("Abyssinian Plague")]
		AbyssinianPlague,
		[Description("Ancient Curse")]
		AncientCurse,
		[Description("Arcane Sickness")]
		ArcaneSickness,
		[Description("Black Death")]
		BlackDeath,
		[Description("Blight")]
		Blight,
		[Description("Corrupted Blood")]
		CorruptedBlood,
		[Description("Dragon Pox")]
		DragonPox,
		[Description("Filth Fever")]
		FilthFever,
		[Description("Gorgon Plague")]
		GorgonPlague,
		[Description("Green Rot")]
		GreenRot,
		[Description("Hell Cough")]
		HellCough,
		[Description("Necrosis")]
		Necrosis,
		[Description("Red Fever")]
		RedFever,
		[Description("Shadow Plague")]
		ShadowPlague,
		[Description("Warlock Fever")]
		WarlockFever,
		[Description("Weeping Sores")]
		WeepingSores,
		[Description("White Fever")]
		WhiteFever,
		[Description("Wyrm Pox")]
		WyrmPox,
		[Description("Yellow Fever")]
		YellowFever,
		[Description("Zombie Virus")]
		ZombieVirus
	}

	public static class SicknessUtility
	{
		// AddSicknessToWorld adds an Wandering Sickness to the game world. 
		public static void AddSicknessToWorld(Mobile m, BaseSickness sickness)
		{
			if (m != null && sickness != null)
			{
				var infection = new WanderingSickness();

				infection.Sickness = sickness;

				infection.MoveToWorld(m.Location, m.Map);
			}
		}

		// Returns a random sickness from the SicknessType enum
		public static BaseSickness GetRandomSickness()
		{
			return GetSickness(Utility.RandomMinMax(0, Enum.GetValues(typeof(SicknessType)).Length - 1));
		}

		// Returns a sickness from the SicknessType enum based on the provided integer index
		public static BaseSickness GetSickness(int location)
		{
			switch (location)
			{
				case 0: return new AbyssinianPlagueType();
				case 1: return new AncientCurseType();
				case 2: return new ArcaneSicknessType();
				case 3: return new BlackDeathType();
				case 4: return new BlightType();
				case 5: return new CorruptedBloodType();
				case 6: return new DragonPoxType();
				case 7: return new FilthFeverType();
				case 8: return new GorgonPlagueType();
				case 9: return new GreenRotType();

				case 10: return new HellCoughType();
				case 11: return new NecrosisType();
				case 12: return new RedFeverType();
				case 13: return new ShadowPlagueType();
				case 14: return new WarlockFeverType();
				case 15: return new WeepingSoresType();
				case 16: return new WhiteFeverType();
				case 17: return new WyrmPoxType();
				case 18: return new YellowFeverType();
				case 19: return new ZombieVirusType();

				default: return GetSickness(Utility.RandomMinMax(0, Enum.GetValues(typeof(SicknessType)).Length - 1));
			}
		}
	}
}

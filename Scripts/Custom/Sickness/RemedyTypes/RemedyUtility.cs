using Server.Engines.Sickness.Items;
using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Engines.Sickness.RemedyTypes
{
	// When adding in or removing remedies, ensure
	// you match up a sickness in the same position

	// Enum containing the different types of remedies
	public enum RemedyType
	{
		[Description("Enchanted Forest Moss")]
		EnchantedForestMoss,
		[Description("Holy Water")]
		HolyWater,
		[Description("Arcane Elixir")]
		ArcaneElixir,
		[Description("Cure Disease Potion")]
		CureDiseasePotion,
		[Description("Healing Unguent")]
		HealingUnguent,
		[Description("Antidote")]
		Antidote,
		[Description("Healing Poultice")]
		HealingPoultice,
		[Description("Healing Ointment")]
		HealingOintment,
		[Description("Elixir Of Life")]
		ElixirOfLife,
		[Description("Healing Salve")]
		HealingSalve,
		[Description("Phoenix Feather")]
		PhoenixFeather,
		[Description("Bloody Tear")]
		BloodyTear,
		[Description("Healing Balm")]
		HealingBalm,
		[Description("Unicorn Horn")]
		UnicornHorn,
		[Description("Cure Curse Potion")]
		CureCursePotion,
		[Description("Starlight Potion")]
		StarlightPotion,
		[Description("Healing Liniment")]
		HealingLiniment,
		[Description("Dragonfire Potion")]
		DragonfirePotion,
		[Description("Healing Tincture")]
		HealingTincture,
		[Description("Demon Heart")]
		DemonHeart
	}


	public static class RemedyUtility
	{

		// Takes in a Corpse object and an Item object, and adds the Item
		// object to the Corpse object's Loot list. This method is used to randomly add
		// a "cure" item (such as a potion or unguent) to a creature's corpse after
		// they have died.
		public static void AddRandomCure(Mobile creature, Container corpse)
		{
			if (!IsCreature(creature, corpse))
			{
				AddCure(Utility.RandomMinMax(0, Enum.GetValues(typeof(RemedyType)).Length - 1), corpse);
			}
		}

		// Takes in a Corpse object and an Item object, and adds the Item
		// object to the Corpse object's Loot list. This method is used to add
		// a "cure" item (such as a potion or unguent) to a creature's corpse after
		// they have died.
		public static void AddCure(int remedy, Container corpse)
		{
			switch (remedy)
			{
				case 0: corpse.AddItem(new Antidote()); break;
				case 1: corpse.AddItem(new ArcaneElixir()); break;
				case 2: corpse.AddItem(new BloodyTear()); break;
				case 3: corpse.AddItem(new CureCursePotion()); break;
				case 4: corpse.AddItem(new CureDiseasePotion()); break;
				case 5: corpse.AddItem(new DemonHeart()); break;
				case 6: corpse.AddItem(new DragonfirePotion()); break;
				case 7: corpse.AddItem(new ElixirOfLife()); break;
				case 8: corpse.AddItem(new EnchantedForestMoss()); break;
				case 9: corpse.AddItem(new HealingBalm()); break;
				case 10: corpse.AddItem(new HealingLiniment()); break;
				case 11: corpse.AddItem(new HealingOintment()); break;
				case 12: corpse.AddItem(new HealingPoultice()); break;
				case 13: corpse.AddItem(new HealingSalve()); break;
				case 14: corpse.AddItem(new HealingTincture()); break;
				case 15: corpse.AddItem(new HealingUnguent()); break;
				case 16: corpse.AddItem(new HolyWater()); break;
				case 17: corpse.AddItem(new PhoenixFeather()); break;
				case 18: corpse.AddItem(new StarlightPotion()); break;
				case 19: corpse.AddItem(new UnicornHorn()); break;
				default: break;
			}
		}

		public static void AddTaskCure(RemedyType remedy, Container corpse)
		{
			switch (remedy)
			{
				case RemedyType.Antidote: corpse.AddItem(new Antidote()); break;
				case RemedyType.ArcaneElixir: corpse.AddItem(new ArcaneElixir()); break;
				case RemedyType.BloodyTear: corpse.AddItem(new BloodyTear()); break;
				case RemedyType.CureCursePotion: corpse.AddItem(new CureCursePotion()); break;
				case RemedyType.CureDiseasePotion: corpse.AddItem(new CureDiseasePotion()); break;
				case RemedyType.DemonHeart: corpse.AddItem(new DemonHeart()); break;
				case RemedyType.DragonfirePotion: corpse.AddItem(new DragonfirePotion()); break;
				case RemedyType.ElixirOfLife: corpse.AddItem(new ElixirOfLife()); break;
				case RemedyType.EnchantedForestMoss: corpse.AddItem(new EnchantedForestMoss()); break;
				case RemedyType.HealingBalm: corpse.AddItem(new HealingBalm()); break;
				case RemedyType.HealingLiniment: corpse.AddItem(new HealingLiniment()); break;
				case RemedyType.HealingOintment: corpse.AddItem(new HealingOintment()); break;
				case RemedyType.HealingPoultice: corpse.AddItem(new HealingPoultice()); break;
				case RemedyType.HealingSalve: corpse.AddItem(new HealingSalve()); break;
				case RemedyType.HealingTincture: corpse.AddItem(new HealingTincture()); break;
				case RemedyType.HealingUnguent: corpse.AddItem(new HealingUnguent()); break;
				case RemedyType.HolyWater: corpse.AddItem(new HolyWater()); break;
				case RemedyType.PhoenixFeather: corpse.AddItem(new PhoenixFeather()); break;
				case RemedyType.StarlightPotion: corpse.AddItem(new StarlightPotion()); break;
				case RemedyType.UnicornHorn: corpse.AddItem(new UnicornHorn()); break;
				default: break;
			}
		}

		// Drop specific cures on specific creatures.
		private static bool IsCreature(Mobile creature, Container corpse)
		{
			if (creature is EvilMageLord)
			{
				corpse.AddItem(new ArcaneElixir());

				return true;
			}

			if (creature is Dragon)
			{
				corpse.AddItem(new DragonfirePotion());

				return true;
			}

			if (creature is Daemon)
			{
				corpse.AddItem(new DemonHeart());

				return true;
			}

			if (creature is BloodElemental)
			{
				corpse.AddItem(new BloodyTear());

				return true;
			}

			if (creature is Reaper)
			{
				corpse.AddItem(new EnchantedForestMoss());

				return true;
			}

			if (creature is Wisp)
			{
				corpse.AddItem(new ElixirOfLife());

				return true;
			}

			if (creature is ShadowWisp)
			{
				corpse.AddItem(new StarlightPotion());

				return true;
			}

			if (creature is Phoenix)
			{
				corpse.AddItem(new PhoenixFeather());

				return true;
			}

			if (creature is Unicorn)
			{
				corpse.AddItem(new UnicornHorn());

				return true;
			}

			if (creature is HolyMage)
			{
				corpse.AddItem(new HolyWater());

				return true;
			}

			return false;
		}
	}
}

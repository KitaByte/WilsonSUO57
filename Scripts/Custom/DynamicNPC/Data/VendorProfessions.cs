using System;

namespace Server.Services.DynamicNPC.Data
{
	public static class VendorProfessions
	{
		public static bool IsGoodVendor(string profession, out VendorTypes vendorTypes)
		{
			if (Enum.TryParse<VendorTypes>(DynamicHandler.Capitalize(profession), out var vendorType))
			{
				var IsGood =  Enum.IsDefined(typeof(VendorTypes), vendorType);

				vendorTypes = vendorType;

				return IsGood;
			}

			vendorTypes = VendorTypes.None;

			return false;
		}

		public static bool IsGoodVendor(Type vendorClass)
		{
			return Enum.IsDefined(typeof(VendorTypes), vendorClass.Name);
		}

		public enum VendorTypes
		{
			None					= 0,
			Alchemist				= 1,
			Architect				= 2,
			Armourer				= 3,
			Baker					= 4,
			Bard					= 5,
			Barkeeper				= 6,
			Beekeeper				= 7,
			Blacksmith				= 8,
			BoatPainter				= 9,
			Bowyer					= 10,
			Butcher					= 11,
			Carpenter				= 12,
			Cobbler					= 13,
			Cook					= 14,
			CrabFisher				= 15,
			CustomHairstylist		= 16, // Broken due to title not matching
			DocksAlchemist			= 17,
			Farmer					= 18,
			Fisher					= 19,
			Furtrader				= 20,
			Gardener				= 21,
			Glassblower				= 22,
			GolemCrafter			= 23,
			HairStylist				= 24,
			Healer					= 25,
			Herbalist				= 26,
			Innkeeper				= 27,
			IronWorker				= 28,
			Jeweler					= 29,
			LeatherWorker			= 30,
			Mage					= 31,
			Mapmaker				= 32,
			Miner					= 33,
			Monk					= 34,
			Mystic					= 35,
			Necromancer				= 36,
			Provisioner				= 37,
			Rancher					= 38,
			Ranger					= 39,
			RealEstateBroker		= 40,
			Scribe					= 41,
			SeaMarketTavernKeeper	= 42,
			Shipwright				= 43,
			StoneCrafter			= 44,
			Tailor					= 45,
			Tanner					= 46,
			TavernKeeper			= 47,
			Thief					= 48,
			Tinker					= 49,
			Veterinarian			= 50,
			Waiter					= 51,
			Weaponsmith				= 52,
			Weaver					= 53
		}
	}
}

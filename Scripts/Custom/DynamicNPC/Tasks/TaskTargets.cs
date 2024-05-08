using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Mobiles;

namespace Server.Services.DynamicNPC.Tasks
{
	static internal class TaskTargets
	{
		public static BaseCreature GetMobileTarget(int level)
		{
			BaseCreature target = null;

			var OnlyOneMob = new List<BaseCreature>();

			foreach (var mob in World.Mobiles.Values.ToList().FindAll(m => m is BaseCreature))
			{
				if (OnlyOneMob.Find(m => m.GetType() == mob.GetType()) == null)
				{
					if (mob?.Fame < 15001 && mob?.Karma < 0)
					{
						OnlyOneMob.Add(mob as BaseCreature);
					}
				}
			}

			var chanceCount = OnlyOneMob.Count;

			foreach (var creature in OnlyOneMob)
			{
				if (creature.FightMode == FightMode.Closest && CheckMobileLevel(creature, level))
				{
					if (Utility.RandomMinMax(0, chanceCount) < DynamicSettings.BaseDrawChance)
					{
						target = creature;

						break;
					}
					else
					{
						chanceCount--;
					}
				}
			}

			if (target == null)
			{
				target = World.Mobiles.Values.ToList().First(m => m is BaseCreature bc && bc.Karma < 0) as BaseCreature;
			}

			return target;
		}

		private static bool CheckMobileLevel(BaseCreature creature, int level)
		{
			if (creature?.Fame > 0)
			{
				if (level == 1)
				{
					if (creature?.Fame > 10000)
						return true;
				}

				if (level == 2)
				{
					if (creature?.Fame > 5000 && creature?.Fame < 10001)
						return true;
				}

				if (level == 3)
				{
					if (creature?.Fame < 5001)
						return true;
				}
			}

			return false;
		}

		public static Item GetItemTarget()
		{
			Item target = null;

			var chanceCount = World.Items.Values.ToList().FindAll(m => m.Movable).Count();

			foreach (var item in World.Items.Values.ToList().FindAll(m => m.Movable))
			{
				if (item is BaseWeapon weapon)
				{
					if (Utility.RandomMinMax(0, chanceCount) < DynamicSettings.BaseDrawChance)
					{
						if (weapon != null)
						{
							target = weapon;

							break;
						}
					}
				}

				if (item is Food food)
				{
					if (Utility.RandomMinMax(0, chanceCount) < DynamicSettings.BaseDrawChance)
					{
						if (food != null)
						{
							target = food;

							break;
						}
					}
				}

				if (item is BaseArmor armor)
				{
					if (Utility.RandomMinMax(0, chanceCount) < DynamicSettings.BaseDrawChance)
					{
						if (armor != null)
						{
							target = armor;

							break;
						}
					}
				}

				if (item is BaseClothing clothing)
				{
					if (Utility.RandomMinMax(0, chanceCount) < DynamicSettings.BaseDrawChance)
					{
						if (clothing != null)
						{
							target = clothing;

							break;
						}
					}
				}

				chanceCount--;
			}

			if (target == null)
			{
				target = World.Items.Values.ToList().First(m => m.Movable);
			}

			return target;
		}

		public static Point3D GetLocationTarget(PlayerMobile player, out BaseVendor venTarget)
		{
			BaseVendor target = null;

			var chanceCount = BaseVendor.AllVendors.Count();

			foreach (var vendor in BaseVendor.AllVendors)
			{
				if (Utility.RandomMinMax(0, chanceCount) < DynamicSettings.BaseDrawChance && RegionNameCheck(vendor, player))
				{
					if (CheckDistance(vendor.Location, player) && IsLettersOnly(vendor.Region.ToString()))
					{
						target = vendor;

						break;
					}
				}
				else
				{
					chanceCount--;
				}
			}

			if (target != null)
			{
				venTarget = target;

				return venTarget.Location;
			}
			else
			{
				venTarget = BaseVendor.AllVendors.First(v => v.Region != null);

				return venTarget.Location;
			}
		}

		private static bool RegionNameCheck(BaseVendor vendor, PlayerMobile player)
		{
			if (vendor.Map != player.Map)
				return false;

			return vendor.Region != null
					&& vendor.Region.ToString() != "Region"
						&& vendor.Region.ToString() != "BaseRegion";
		}

		private static bool CheckDistance(Point3D point, PlayerMobile player)
		{
			var distance = player.GetDistanceToSqrt(point);

			if (distance > 500)
			{
				return true;
			}

			return false;
		}

		private static bool IsLettersOnly(string input)
		{
			return input.All(Char.IsLetter);
		}
	}
}

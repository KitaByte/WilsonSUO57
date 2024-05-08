using System;
using System.Linq;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Services.DynamicNPC.Items;
using Server.Services.DynamicNPC.Tasks;

namespace Server.Services.DynamicNPC
{
	public class VendorProfile
	{
		public readonly BaseVendor VendorHandle;

		public int Age { get; set; }

		private readonly int MinAge = DynamicSettings.MinAge;

		private readonly int MaxAge = DynamicSettings.MaxAge;

		public int Level { get; set; }

		private readonly int MinLevel = DynamicSettings.MinLevel;

		private readonly int MaxLevel = DynamicSettings.MaxLevel;

		private string SkillTitle => GetTitle();

		private readonly List<Customer> CustomerList;

		private DateTime LastBirthday { get; set; }

		private readonly int Chance = DynamicSettings.BaseChance;

		public void GetProfileProps(PlayerMobile admin)
		{
			admin.SendMessage(317, $"Vendor : {VendorHandle.Name}");
			admin.SendMessage(321, $"Title : {SkillTitle} {DynamicHandler.Capitalize(VendorHandle.Title.Split(' ').Last())}");
			admin.SendMessage(1153, $"Birthday : {LastBirthday.ToShortDateString()}");
			admin.SendMessage(1161, $"Age : {Age} [Max : {MaxAge}]");
			admin.SendMessage(1161, $"Level : {Level}");
			admin.SendMessage(CustomerList.Count > 0? 67: 37, $"Customers : {CustomerList.Count}");

			if (CustomerList.Count > 0)
			{
				foreach (var custy in CustomerList)
				{
					admin.SendMessage(297, $"Customer : {custy.Name}");
					admin.SendMessage(301, $"Loyalty : {GetTopIndex(custy.Name)}/{CustomerList.Count}");
					admin.SendMessage(1153, $"Last Visited : {custy.LastVisited.ToShortDateString()}");
					admin.SendMessage(1161, $"Total Visits : {custy.TotVisits}");
					admin.SendMessage(1161, $"Total Gold : {custy.TotBonus}");
					admin.SendMessage(1161, $"Tasks Completed : {custy.TotTasks}");
					admin.SendMessage(custy.VendTask.IsAccepted? 67:37, $"Has Task : {custy.VendTask.IsAccepted}");
				}
			}
		}

		public VendorProfile(BaseVendor vendor)
		{
			VendorHandle = vendor;

			CustomerList = new List<Customer>();

			Age = MinAge;

			Level = MinLevel;

			LastBirthday = DateTime.Now;

			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Vendor Created for {vendor.Name} at {LastBirthday}", true);
			}
		}

		// Called OnTick 
		internal void UpdateVendor()
		{
			CheckAgeGain();

			var profTitle = "";

			if (VendorHandle.Title != null)
			{
				profTitle = VendorHandle.Title.Split(' ').Last();
			}

			VendorHandle.Title = $"the {SkillTitle} {profTitle}";

			VendorHandle.HairHue = AdjustHairColor();

			if (!VendorHandle.Female)
			{
				VendorHandle.FacialHairHue = AdjustHairColor();
			}

			CheckGM();

			CheckShift();

			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Vendor Updated : {VendorHandle.Name}", true);
			}
		}

		private void CheckShift()
		{
			if (!DynamicSettings.WorksNight && DynamicHandler.IsNight(VendorHandle))
			{
				VendorHandle.Hidden = true;

				VendorHandle.CantWalk = true;

				return;
			}

			VendorHandle.Hidden = false;

			VendorHandle.CantWalk = false;
		}

		private void CheckGM()
		{
			var item = VendorHandle.FindItemOnLayer(Layer.MiddleTorso);

			if (Level == MaxLevel)
			{
				if (item != null && nameof(item) != nameof(GoldVendorSash))
				{
					VendorHandle.RemoveItem(item);
				}

				VendorHandle.EquipItem(new GoldVendorSash(VendorHandle.Title.Split(' ').Last()));
			}
			else
			{
				if (item != null && item is GoldVendorSash)
				{
					VendorHandle.RemoveItem(item);
				}
			}
		}

		private int AgingHairHue = 0;

		private int AdjustHairColor()
		{
			if (AgingHairHue == 0)
			{
				var lastDigit = VendorHandle.HairHue % 10;

				if (lastDigit < 6 && lastDigit >= 2)
				{
					lastDigit = 2;
				}
				else
				{
					lastDigit = 7;
				}

				AgingHairHue = (VendorHandle.HairHue / 10) * 10 + lastDigit;
			}

			switch (Age)
			{
				case int n when (n < 30):
					{
						return AgingHairHue;
					}
				case int n when (n > 29 && n < 50):
					{
						return AgingHairHue + 1;
					}
				case int n when (n > 49 && n < 75):
					{
						return AgingHairHue + 2;
					}
				case int n when (n > 74 && n < 90):
					{
						return AgingHairHue + 3;
					}
				case int n when (n > 89 && n < 99):
					{
						return AgingHairHue + 4;
					}
				default:
					{
						return 1153;
					}
			}
		}

		private void UpdateCustomer(Mobile patron, bool IsSold, int gold)
		{
			if (GetCustomer(patron) is Customer customer)
			{
				customer.TotVisits++;

				customer.TotBonus += gold;

				if (IsSold)
				{
					customer.TotSold++;
				}
				else
				{
					customer.TotBought++;
				}
			}
		}

		private void CheckAgeGain()
		{
			if (DateTime.Now > LastBirthday + TimeSpan.FromDays(1)) 
			{
				UpdateAge();

				LastBirthday = DateTime.Now;

				VendorResponses.AgeResponse(this);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Had birthday : {VendorHandle.Name}", true);
				}
			}
		}

		public void GivePatronageBonus(Mobile patron, bool isSold, int amount)
		{
			if (GetCustomer(patron) is Customer customer && Utility.Random(100) < Chance + customer?.TotVisits / 100)
			{
				var gold = new Gold(amount * GetLoyalityMod(patron.Name));

				patron.AddToBackpack(gold);

				PlayDroppedSound(patron, gold);

				patron.SendMessage($"You have been rewarded {amount} gold for your patronage!");

				UpdateCustomer(patron, isSold, amount);

				PlayHappySound();

				VendorResponses.BonusGoldResponse(this, patron.Name, amount);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Gave {amount} Gold : {VendorHandle.Name} to {patron.Name}", true);
				}
			}
		}

		public void TryModItem(Mobile patron, Item item)
		{
			if (GetCustomer(patron) is Customer customer && Utility.Random(1000) < Chance + customer?.TotVisits / 100)
			{
				var isModded = false;

				var name = String.Empty;

				switch (item)
				{
					case BaseWeapon weapon:
						{
							isModded = true;
							name = weapon.Name;

							weapon.Crafter = VendorHandle;
							weapon.LootType = LootType.Newbied;
							weapon.Hue = Utility.RandomBrightHue();
							break;
						}
					case BaseArmor armor:
						{
							isModded = true;
							name = armor.Name;

							armor.Crafter = VendorHandle;
							armor.LootType = LootType.Newbied;
							armor.Hue = Utility.RandomBrightHue();
							break;
						}
					case BaseClothing clothing:
						{
							isModded = true;
							name = clothing.Name;

							clothing.Crafter = VendorHandle;
							clothing.LootType = LootType.Newbied;
							clothing.Hue = Utility.RandomBrightHue();
							break;
						}
				}

				if (isModded)
				{
					patron.AddToBackpack(item);

					PlayDroppedSound(patron, item);

					patron.SendMessage($"You have been rewarded a modified {name} for your patronage!");

					PlayHappySound();

					VendorResponses.BonusModResponse(this, patron.Name, name);

					if (DynamicSettings.InDebug)
					{
						DynamicHandler.MsgToConsole($"Gave modded {name} : {VendorHandle.Name} to {patron.Name}", true);
					}
				}
			}
		}

		public void TryAddMore(Mobile patron, Item item)
		{
			if (GetCustomer(patron) is Customer customer && Utility.Random(1000) < Chance + customer?.TotVisits / 100)
			{
				var bonusItem = (Item)Activator.CreateInstance(item.GetType());

				patron.AddToBackpack(bonusItem);

				PlayDroppedSound(patron, item);

				patron.SendMessage($"You have been rewarded an extra one for your patronage!");

				PlayHappySound();

				VendorResponses.BonusMoreResponse(this, patron.Name);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Gave more {bonusItem} : {VendorHandle.Name} to {patron.Name}", true);
				}
			}
		}

		public void TrySkillGain()
		{
			if (Utility.Random(1000) <= DynamicSettings.ChanceToLevel)
			{
				UpdateLevel(true);

				VendorResponses.LevelGainResponse(this);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Gained Skill : {VendorHandle.Name}", true);
				}
			}
		}

		public void UpdateAge()
		{
			Age = MaxAge > Age? Age++ : Age;

			if (Age == MaxAge)
			{
				Age = MinAge;

				Level = MinLevel + Level / 5;

				AgingHairHue = 0;

				VendorHandle.Name = NameList.RandomName(VendorHandle.Female ? "female":"male");

				VendorHandle.SpeechHue = Utility.RandomDyedHue();

				VendorHandle.Hue = Utility.RandomSkinHue();
				
				Utility.AssignRandomHair(VendorHandle);

				if (VendorHandle.Female)
				{
					Utility.AssignRandomFacialHair(VendorHandle);
				}

				PlayVendorEffect();

				for (var i = 0; i < CustomerList.Count; i++)
				{
					CustomerList[i].VendorOwner = VendorHandle;

					if (CustomerList[i].VendTask.Giver != null)
					{
						CustomerList[i].VendTask.Giver = VendorHandle;
					}
				}
			}
			else
			{
				UpdateLevel(false);
			}
		}

		public void UpdateLevel(bool isGain)
		{
			Level = isGain ? MaxLevel > Level ? Level++ : Level : MinLevel < Level ? Level-- : Level;

			PlayVendorEffect();
		}

		private string GetTitle()
		{
			if (Level < 30)
			{
				return "Apprentice";
			}

			if (Level < 50)
			{
				return "Adept";
			}

			if (Level < 75)
			{
				return "Skilled";
			}

			if (Level < 100)
			{
				return "Master";
			}

			return "GrandMaster";
		}

		public void TryTask(Mobile patron)
		{
			if (GetCustomer(patron) is Customer customer && Utility.Random(100) < Chance + customer?.TotTasks / 100)
			{
				TryGiveTask(customer);
			}
		}

		private void TryGiveTask(Customer customer)
		{
			if (Utility.Random(1000) < 5 + customer?.TotVisits / 100)
			{
				if (!customer.VendTask.IsAccepted)
				{
					customer.VendTask = new VendorTask(VendorHandle, customer.Player);

					PlayHappySound();

					VendorResponses.GiveTaskResponse(this, customer.Name);

					if (DynamicSettings.InDebug)
					{
						DynamicHandler.MsgToConsole($"Gave task : {VendorHandle.Name} to {customer.Name}", true);
					}
				}
				else
				{
					customer.Player.SendMessage("You have a task with this vendor, accept task or cancel task!");
				}
			}
		}

		public bool HasCustomer(Mobile customer)
		{
			return CustomerList.Find(c => c.Name == customer?.Name) != null;
		}

		public bool HasCustomer(string customer)
		{
			return CustomerList.Find(c => c.Name == customer) != null;
		}

		public void AddCustomer(Customer customer)
		{
			if (CustomerList.Contains(customer)) 
			{ 
				return; 
			}

			CustomerList.Add(customer);

			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Customer : {VendorHandle.Name} added {customer.Name}", true);
			}
		}

		public Customer GetCustomer(Mobile patron)
		{
			var customer = GetCustomerList()?.Find(c => c.Name == patron?.Name);

			if (customer != null)
			{
				return customer;
			}
			else if (patron is PlayerMobile pm)
			{
				customer = new Customer(pm, VendorHandle);

				AddCustomer(customer);

				return customer;
			}
			else 
			{ 
				return null; 
			}
		}

		public Customer GetCustomer(string patron)
		{
			var customer = GetCustomerList()?.Find(c => c.Name == patron);

			if (customer != null)
			{
				return customer;
			}
			else
			{
				return null;
			}
		}

		public void RemoveCustomer(Customer customer)
		{
			if (CustomerList.Contains(customer))
			{
				CustomerList.Remove(customer);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Customer : {VendorHandle.Name} removed {customer.Name}", true);
				}
			}
		}

		public int GetCustomerCount()
		{
			if (CustomerList?.Count > 0)
			{
				return CustomerList.Count;
			}
			else
			{
				return 0;
			}
		}

		public List<Customer> GetCustomerList()
		{
			if (CustomerList?.Count > 0)
			{
				return CustomerList;
			}
			else
			{
				return null;
			}
		}

		public void PlayVendorEffect()
		{
			var v = VendorHandle;

			var hue = VendorHandle.SpeechHue;

			Effects.SendLocationEffect(new Point3D(v.X, v.Y, v.Z + 1), v.Map, 0x376A, 15, hue, 0); //0x47D );

			Effects.PlaySound(v.Location, v.Map, 492);
		}

		private void PlayDroppedSound(Mobile m, Item i)
		{
			m.SendSound(m.Backpack.GetDroppedSound(i), m.Location);
		}

		private void PlayHappySound()
		{
			Effects.PlaySound(VendorHandle.Location, VendorHandle.Map, VendorHandle.Female ? 783 : 1054);
		}

		public int GetTopIndex(string customer)
		{
			if (CustomerList?.Count > 0)
			{
				CustomerList?.Sort((c1, c2) => c2.TotVisits.CompareTo(c1.TotVisits));

				var topVisitorIndex = CustomerList.FindIndex(c => c.Name == customer);

				return topVisitorIndex;
			}
			else
			{
				return -1;
			}
		}

		public int GetLoyalityMod(string customer)
		{
			var position = GetTopIndex(customer) + 1;

			if (position == 1)
			{
				return 3;
			}

			if (position > 1 && position < 11)
			{
				return 2;
			}

			return 1;
		}
	}
}
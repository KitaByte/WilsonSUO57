using System.Collections.Generic;
using System.IO;
using Server.Mobiles;
using Server.Services.DynamicNPC.Data;
using Server.Services.DynamicNPC.Tasks;

namespace Server.Services.DynamicNPC
{
	static internal class ProfileStore
	{
		private static readonly int Version = DynamicSettings.Version;

		private static Dictionary<Mobile, VendorProfile> VendorsDictionary = new Dictionary<Mobile, VendorProfile>();

		private static readonly string FilePath = Path.Combine(@"Saves\DynamicNPC", $"ProfileStore{Version}.bin");

		static readonly internal object DictLock = new object();

		public static bool HasVendor(Mobile vendor)
		{
			lock (DictLock)
				return VendorsDictionary.ContainsKey(vendor);
		}

		public static void AddVendor(Mobile vendor, VendorProfile profile)
        {
            if (!HasVendor(vendor))
            {
                lock (DictLock)
                    VendorsDictionary.Add(vendor, profile);
            }
            else
            {
                EditVendor(vendor, profile);
            }

            if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Added : {vendor.Name} {vendor.Title}", true);
			}
		}

		public static void EditVendor(Mobile vendor, VendorProfile newProfile)
		{
			if (HasVendor(vendor))
			{
				lock (DictLock)
					VendorsDictionary[vendor] = newProfile;
			}

			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Edited : {vendor.Name} {vendor.Title}", true);
			}
		}

		public static void RemoveVendor(Mobile vendor)
		{
			if (HasVendor(vendor))
			{
				lock (DictLock)
					VendorsDictionary.Remove(vendor);

				if (DynamicSettings.InDebug)
				{
					DynamicHandler.MsgToConsole($"Removed : {vendor.Name} {vendor.Title}", true);
				}
			}
		}
		
		public static VendorProfile GetVendorProfile(Mobile vendor)
		{
			if (HasVendor(vendor))
			{
				return VendorsDictionary[vendor];
			}
			else
			{
				return null;
			}
		}

		// First Load Only
		private static bool HadFirstStart { get; set; }

		static internal void OnWorldLoad()
		{
			if (HadFirstStart)
			{
				CheckAllVendors();

				HadFirstStart = true;
			}
		}

		// Save-Load
		private static void CheckAllVendors()
		{
			foreach (var vendor in BaseVendor.AllVendors)
			{
				if (VendorProfessions.IsGoodVendor(vendor.GetType()))
				{
					if (!VendorsDictionary.ContainsKey(vendor))
					{
						AddVendor(vendor, new VendorProfile(vendor));
					}
				}
			}

			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Vendor All Checked : {VendorsDictionary.Count} profiles logged!", true);
			}
		}

		public static void OnSave(WorldSaveEventArgs e)
		{
			Persistence.Serialize(FilePath, OnSerialize);
		}

		private static void OnSerialize(GenericWriter writer)
		{
			CheckAllVendors();

			writer.Write(0);

			writer.Write(HadFirstStart);

			writer.Write(VendorsDictionary.Count);

			foreach (var vendor in VendorsDictionary)
			{
				writer.Write(vendor.Key);
				writer.Write(vendor.Value.Age);
				writer.Write(vendor.Value.Level);

				writer.Write(vendor.Value.GetCustomerCount());

				if (vendor.Value.GetCustomerCount() > 0)
				{
					foreach (var customer in vendor.Value.GetCustomerList())
					{
						writer.Write(customer.Name);
						writer.Write(customer.Player);
						writer.Write(customer.VendorOwner);
						writer.Write(customer.TotVisits);
						writer.Write(customer.LastVisited);
						writer.Write(customer.TotBought);
						writer.Write(customer.TotSold);
						writer.Write(customer.TotBonus);
						writer.Write(customer.TotTasks);
						writer.Write(customer.VendTask.IsAccepted);

						if (customer.VendTask.IsAccepted)
						{
							writer.Write(customer.VendTask.IsAccepted);
							writer.Write(customer.VendTask.IsComplete);
							writer.Write(customer.VendTask.Giver);
							writer.Write(customer.VendTask.Taker);
							writer.Write((int)customer.VendTask.TaskType);
							writer.Write((int)customer.VendTask.TaskName);
							writer.Write(customer.VendTask.TargetMobile);
							writer.Write(customer.VendTask.TargetItem);
							writer.Write(customer.VendTask.Reward);
							writer.Write(customer.VendTask.Description);
							writer.Write(customer.VendTask.TargetLocation);
						}
					}
				}
			}
		}

		public static void OnLoad()
		{
			Persistence.Deserialize(FilePath, OnDeserialize);
		}

		private static void OnDeserialize(GenericReader reader)
		{
			VendorsDictionary = new Dictionary<Mobile, VendorProfile>();

			reader.ReadInt();

			HadFirstStart = reader.ReadBool();

			var vendorCount = reader.ReadInt();

			if (vendorCount > 0)
			{
				for (var i = 0; i < vendorCount; i++)
				{
					var vendor = reader.ReadMobile() as BaseVendor;

					var profile = new VendorProfile(vendor)
					{
						Age = reader.ReadInt(),
						Level = reader.ReadInt(),
					};

					var customerCount = reader.ReadInt();

					for (var j = 0; j < customerCount; j++)
					{
						var custy = new Customer()
						{
							Name = reader.ReadString(),
							Player = reader.ReadMobile() as PlayerMobile,
							VendorOwner = reader.ReadMobile() as BaseVendor,
							TotVisits = reader.ReadInt(),
							LastVisited = reader.ReadDateTime(),
							TotBought = reader.ReadInt(),
							TotSold = reader.ReadInt(),
							TotBonus = reader.ReadInt(),
							TotTasks = reader.ReadInt(),
						};

						if (reader.ReadBool())
						{
							custy.VendTask = new VendorTask()
							{
								IsAccepted = reader.ReadBool(),
								IsComplete = reader.ReadBool(),
								Giver = reader.ReadMobile() as BaseVendor,
								Taker = reader.ReadMobile() as PlayerMobile,
								TaskType = (TaskTypes)reader.ReadInt(),
								TaskName = (TaskNames)reader.ReadInt(),
								TargetMobile = reader.ReadMobile(),
								TargetItem = reader.ReadItem(),
								Reward = reader.ReadItem(),
								Description = reader.ReadString(),
								TargetLocation = reader.ReadPoint3D()
							};
						}

						profile.AddCustomer(custy);
					}

					AddVendor(vendor, profile);
				}
			}

			CheckAllVendors();
		}

		static internal List<VendorProfile> GetAllProfiles()
		{
			var profiles = new List<VendorProfile>();

			foreach (var profile in VendorsDictionary.Values)
			{
				profiles.Add(profile);
			}

			return profiles;
		}
	}
}

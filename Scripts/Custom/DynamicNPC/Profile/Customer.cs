using System;
using Server.Mobiles;
using Server.Services.DynamicNPC.Tasks;

namespace Server.Services.DynamicNPC
{
	public class Customer
	{
		public string Name { get; set; }

		public PlayerMobile Player { get; set; }

		public BaseVendor VendorOwner { get; set; }

		public int TotVisits { get; set; }

		public DateTime LastVisited { get; set; }

		public int TotBought { get; set; }

		public int TotSold { get; set; }

		public int TotBonus { get; set; }

		public int TotTasks { get; set; }

		private VendorTask _vendTask;
		public VendorTask VendTask
		{
			get => _vendTask ?? new VendorTask();
			set => _vendTask = value;
		}

		public Customer()
		{
			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Blank Customer Created!", true);
			}
		}

		public Customer(PlayerMobile mobile, BaseVendor vendorOwner)
		{
			Name = mobile.Name;

			Player = mobile;

			VendorOwner = vendorOwner;

			if (DynamicSettings.InDebug)
			{
				DynamicHandler.MsgToConsole($"Customer Created for {mobile.Name} by {vendorOwner.Name} {vendorOwner.Title}", true);
			}
			VendorOwner = vendorOwner;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj.GetType() != GetType())
				return false;

			var other = (Customer)obj;

			return VendorOwner == other.VendorOwner;
		}

		public override int GetHashCode()
		{
			return VendorOwner.GetHashCode();
		}
	}
}
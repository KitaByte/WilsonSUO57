using Server.Commands;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Services.DynamicNPC.Commands
{
	internal class DynCmdFindVendor
	{
		public static void Initialize()
		{
			CommandSystem.Register("FindVendor", AccessLevel.Administrator, FindVendor_OnCommand);
		}

		[Usage("FindVendor <name>")]
		[Description("Dynamic NPC : Find Vendor by Name")]
		private static void FindVendor_OnCommand(CommandEventArgs e)
		{
			if (e.Arguments.Length > 0)
			{
				var arg = e.Arguments[0].ToLower();

				if (arg != null)
				{
					var vendor = BaseVendor.AllVendors.Find(v => v.Name.ToLower() == arg);

					if (vendor != null && e.Mobile is PlayerMobile admin && admin != null)
					{
						admin.Map = vendor.Map;
						admin.Location = vendor.Location;

						admin.SendMessage(53, $"Moved to {vendor.Map} : {vendor.Location}");

						if (DynamicSettings.InDebug)
						{
							admin.SendGump(new PropertiesGump(admin, vendor));

							if (ProfileStore.HasVendor(vendor))
							{
								ProfileStore.GetVendorProfile(vendor).GetProfileProps(admin);
							}
						}
					}
					else
					{
						e.Mobile.SendMessage($"Usage Error : FindVendor {arg} (Doesn't Exist)");
					}
				}
				else
				{
					e.Mobile.SendMessage($"Usage Error : FindVendor {arg}");
				}
			}
			else
			{
				e.Mobile.SendMessage("Usage : FindVendor <name>");
			}
		}
	}
}

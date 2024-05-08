using System.IO;
using Server.Mobiles;
using Server.Commands;
using System.Collections.Generic;

namespace Server.Custom.RegionBanner
{
    internal class RegionBannerCommand
    {
        private static string RBDataFile = Path.Combine(Directory.GetCurrentDirectory(), "Data", "RB_ExcludeList");

        private static List<string> ExcludeBannerList = new List<string>();

        public static void Initialize()
        {
            CommandSystem.Register("ToggleRBanner", AccessLevel.Player, new CommandEventHandler(RBanner_OnCommand));

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;

            try
            {
                if (File.Exists(RBDataFile))
                {
                    var list = File.ReadAllLines(RBDataFile);

                    foreach (var info in list)
                    {
                        if (ExcludeBannerList == null)
                        {
                            ExcludeBannerList = new List<string>();
                        }

                        ExcludeBannerList.Add(info);
                    }
                }
            }
            catch
            {
                // do nothing
            }
        }

        [Usage("ToggleRegionBannerSystem")]
        [Description("Region Banner System: ON/OFF")]
        private static void RBanner_OnCommand(CommandEventArgs e)
        {
            bool isListed = false;

            if (e.Mobile is PlayerMobile pm)
            {
                isListed = IsExcludedPlayer(pm);

                if (!isListed)
                {
                    ExcludeBannerList.Add($"{pm.Account.Username}:{pm.Name}");

                    pm.SendMessage(53, "Region Banner : Off");
                }
                else
                {
                    ExcludeBannerList.Remove($"{pm.Account.Username}:{pm.Name}");

                    pm.SendMessage(53, "Region Banner : On");
                }
            }
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            if (ExcludeBannerList.Count > 0)
            {
                try
                {
                    File.WriteAllLines(RBDataFile, ExcludeBannerList);
                }
                catch
                {
                    // do nothing
                }
            }
        }

        internal static bool IsExcludedPlayer(PlayerMobile pm)
        {
            foreach (var listing in ExcludeBannerList)
            {
                var listingInfo = listing.Split(':');

                if (listingInfo.Length > 1)
                {
                    if (listingInfo[0] == pm.Account.Username && listingInfo[1] == pm.Name)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

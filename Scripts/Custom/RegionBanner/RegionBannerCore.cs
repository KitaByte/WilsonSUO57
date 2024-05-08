using Server.Gumps;
using Server.Mobiles;
using Server.Regions;

namespace Server.Custom.RegionBanner
{
    internal class RegionBannerCore
    {
        public static void Initialize()
        {
            EventSink.OnEnterRegion += EventSink_OnEnterRegion;
        }

        private static void EventSink_OnEnterRegion(OnEnterRegionEventArgs e)
        {
            if (e.From is PlayerMobile pm && pm.Map != Map.Internal && !RegionBannerCommand.IsExcludedPlayer(pm))
            {
                bool isGood = true;

                if (e.OldRegion.Name != null)
                {
                    if (!e.OldRegion.IsPartOf(typeof(DungeonRegion)) && e.OldRegion.Name.Contains("Cave"))
                    {
                        isGood = false;
                    }

                    if (isGood && e.OldRegion.IsPartOf(typeof(HouseRegion)))
                    {
                        isGood = false;
                    }

                    if (isGood && e.NewRegion.Name != null)
                    {
                        if (!e.NewRegion.IsPartOf(typeof(DungeonRegion)) && e.NewRegion.Name.Contains("Cave"))
                        {
                            isGood = false;
                        }

                        if (isGood && e.NewRegion.IsPartOf(typeof(HouseRegion)))
                        {
                            isGood = false;
                        }
                    }
                }

                if (isGood)
                {
                    if (pm.HasGump(typeof(RegionBannerGump)))
                    {
                        pm.CloseGump(typeof(RegionBannerGump));
                    }

                    BaseGump.SendGump(new RegionBannerGump(pm, e.NewRegion.Name));
                }
            }
        }
    }
}

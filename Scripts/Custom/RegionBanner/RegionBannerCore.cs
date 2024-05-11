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
                    isGood = ValidateRegion(e.OldRegion, isGood);
                }

                if (isGood && e.NewRegion.Name != null)
                {
                    isGood = ValidateRegion(e.NewRegion, isGood);
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

        private static bool ValidateRegion(Region region, bool isGood)
        {
            if (!region.IsPartOf(typeof(DungeonRegion)) && region.Name.Contains("Cave"))
            {
                isGood = false;
            }

            if (isGood && region.IsPartOf(typeof(HouseRegion)))
            {
                isGood = false;
            }

            return isGood;
        }
    }
}

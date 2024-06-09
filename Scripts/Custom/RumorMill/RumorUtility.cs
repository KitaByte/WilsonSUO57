using System.Linq;
using Server.Regions;
using Server.Targeting;
using Server.Custom.RumorMill.Items;

namespace Server.Custom.RumorMill
{
    internal static class RumorUtility
    {
        public static void Initialize()
        {
            RumorCore.LoadRumors();

            EventSink.ValidVendorPurchase += EventSink_ValidVendorPurchase;

            EventSink.ValidVendorSell += EventSink_ValidVendorSell;

            EventSink.Shutdown += EventSink_Shutdown;

            EventSink.Crashed += EventSink_Crashed;
        }

        private static void EventSink_ValidVendorPurchase(ValidVendorPurchaseEventArgs e)
        {
            TrySpreadRumor(e.Vendor, e.Mobile);
        }

        private static void EventSink_ValidVendorSell(ValidVendorSellEventArgs e)
        {
            TrySpreadRumor(e.Vendor, e.Mobile);
        }

        private static void TrySpreadRumor(Mobile from, Mobile to)
        {
            if (Utility.RandomDouble() < 0.05 || to.AccessLevel > AccessLevel.Player)
            {
                RumorCore.SpreadRumor(from, to);
            }
        }

        private static void EventSink_Shutdown(ShutdownEventArgs e)
        {
            RumorCore.RunCleanUp();
        }

        private static void EventSink_Crashed(CrashedEventArgs e)
        {
            RumorCore.RunCleanUp();
        }

        internal static Map GetRandomMap()
        {
            return Utility.RandomList(Map.Felucca, Map.Trammel, Map.Ilshenar, Map.Malas, Map.Tokuno, Map.TerMur);
        }

        internal static Point3D GetAnyLocation(Map map)
        {
            return GetLocation(map);
        }

        internal static Point3D GetLandLocation(Map map)
        {
            return GetLocation(map, true, false);
        }

        internal static Point3D GetWaterLocation(Map map)
        {
            return GetLocation(map, false);
        }

        private static Point3D GetLocation(Map map, bool isLand = true, bool isWater = true)
        {
            Point3D location = Point3D.Zero;

            var mobs = World.Mobiles.Values.Where(m => m.Map == map && ValidateRegion(m)).ToList();

            if (mobs != null && mobs.Count > 0)
            {
                if (isLand && isWater)
                {
                    location = mobs[Utility.Random(mobs.Count - 1)].Location;
                }
                else
                {
                    if (!isWater)
                    {
                        var landMobs = mobs.Where(m => !m.CanSwim).ToList();

                        if (landMobs != null && landMobs.Count > 0)
                        {
                            location = landMobs[Utility.Random(landMobs.Count - 1)].Location;
                        }
                    }
                    else
                    {
                        var waterMobs = mobs.Where(m => m.CanSwim).ToList();

                        if (waterMobs != null && waterMobs.Count > 0)
                        {
                            location = waterMobs[Utility.Random(waterMobs.Count - 1)].Location;
                        }
                    }
                }
            }

            return location;
        }

        private static bool ValidateRegion(Mobile m)
        {
            if (m.Region.IsPartOf(typeof(DungeonRegion)))
            {
                return false;
            }

            if (m.Region.IsPartOf(typeof(TownRegion)))
            {
                return false;
            }

            if (m.Region.IsPartOf(typeof(HouseRegion)))
            {
                return false;
            }

            if (m.Region.IsPartOf(typeof(GuardedRegion)))
            {
                return false;
            }

            return true;
        }

        internal static void RunMessage(IRumor rumor, Mobile from, Mobile to, string subject)
        {
            if (from.Map != rumor.RumorMap)
            {
                from.SayTo(to, true, $"I heard of {subject}, in the lands of {rumor.RumorMap.Name}!");
            }
            else
            {
                var mobs = rumor.RumorMap.GetMobilesInRange(rumor.RumorLocation, 10).ToList();

                if (mobs != null && mobs.Count > 0 && Utility.RandomDouble() < 0.5)
                {
                    from.SayTo(to, true, $"I heard of {subject}, where {mobs[0].Name} roams!");
                }
                else
                {
                    var items = rumor.RumorMap.GetItemsInRange(rumor.RumorLocation, 10).ToList();

                    if (items != null && items.Count > 0)
                    {
                        from.SayTo(to, true, $"I heard of {subject}, near a {items[0].Name}!");
                    }
                    else
                    {
                        string name = new LandTarget(rumor.RumorLocation, rumor.RumorMap).Name;

                        from.SayTo(to, true, $"I heard of {subject}, around some {name}!");
                    }
                }

                int distance = (int)from.GetDistanceToSqrt(rumor.RumorLocation);

                string direction = from.GetDirectionTo(rumor.RumorLocation).ToString();

                from.SayTo(to, true, $"Last seen about {distance} cubits {direction}, from our location!");

                var compass = to.Backpack.FindItemByType(typeof(GoldenCompass));

                if (compass != null && compass is GoldenCompass gc)
                {
                    gc.SetRumor(rumor, to);

                    from.SayTo(to, true, $"I've set the location on your golden compass!");
                }
            }
        }
    }
}

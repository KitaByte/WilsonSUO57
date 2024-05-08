using System;
using Server.Misc;
using System.Linq;

namespace Server.Custom.UOSurvivalEvent.Islands
{
    public enum IslandType
    {
        Default,
        Grass,
        Forest,
        Desert,
        Snow,
        Swamp,
        Fire,
        Blood
    }

    public class BaseIsland
    {
        public string Name { get; private set; } = string.Empty;

        public IslandType IslandBiome { get; private set; } = IslandType.Default;

        public Rectangle2D IslandBounds { get; private set; }

        public DefaultIsland IslandEntity { get; private set; }

        public bool TeamOne { get; private set; } = true;

        public BaseIsland(Rectangle2D bounds)
        {
            if (bounds.Width > 10 && bounds.Height > 10)
            {
                IslandBounds = bounds;
            }
            else
            {
                IslandBounds = new Rectangle2D(bounds.Start, new Point2D(bounds.Start.X + 11, bounds.Start.Y + 11));
            }
        }

        public void SpawnIsland(SurvivalTeam team, Map map, IslandType biome)
        {
            IslandBiome = biome;

            IslandEntity = SurvivalUtility.GetIslandEntity(IslandBiome);

            SurvivalUtility.AddIsland(team, map, map.GetAverageZ(IslandBounds.Start.X, IslandBounds.Start.Y));
        }

        private bool inCallBack = false;

        public void CheckTotem(SurvivalTeam myTeam, SurvivalTeam theirTeam, Mobile m)
        {
            if (!inCallBack)
            {
                var mobs = theirTeam.TeamTotem.GetMobilesInRange(1).ToList();

                if (mobs == null || mobs.Count == 0)
                {
                    if (theirTeam.TotemTime > 0)
                    {
                        theirTeam.TotemTime--;
                    }
                }
                else
                {
                    if (mobs != null && mobs.Contains(m))
                    {
                        if (theirTeam.TotemTime == 0)
                        {
                            SurvivalCore.SendTotemMsg(theirTeam, "I am being deactivated, help!");
                        }
                        else
                        {
                            var timeLeft = SurvivalCore.TotemDeactivation - theirTeam.TotemTime;

                            SurvivalCore.SendTotemMsg(theirTeam, $"I am being deactivated, {timeLeft}");
                        }

                        if (theirTeam.TotemTime < SurvivalCore.TotemDeactivation)
                        {
                            theirTeam.TotemTime++;

                            inCallBack = true;

                            Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                            {
                                inCallBack = false;

                                IslandEntity.CheckIslandMove(myTeam, theirTeam, m);
                            });
                        }
                        else
                        {
                            SurvivalCore.SendTotemMsg(theirTeam, "I am deactivated, Game Over!");

                            theirTeam.TeamTotem.Hue = Utility.RandomRedHue();

                            inCallBack = true;

                            Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                            {
                                SurvivalCore.Winner = myTeam;

                                SurvivalCore.EndMatch();
                            });
                        }
                    }
                }
            }
        }

        // Weather
        private static bool hasWeather = false;

        internal static bool HasWeather()
        {
            return hasWeather;
        }

        internal static void SetWeather(bool isHot)
        {
            if (!hasWeather)
            {
                hasWeather = true;

                Rectangle2D[] rectArray = { SurvivalCore.GameBounds };

                if (isHot)
                {
                    _ = new Weather(Map.Felucca, rectArray, 15, 100, 0, TimeSpan.FromSeconds(5));
                }
                else
                {
                    _ = new Weather(Map.Felucca, rectArray, -15, 100, 0, TimeSpan.FromSeconds(5));
                }
            }
        }

        internal static void EndWeather()
        {
            if (hasWeather)
            {
                hasWeather = false;

                Weather.Initialize();
            }
        }
    }
}

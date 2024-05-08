using System;
using Server.Items;
using Server.Custom.UOSurvivalEvent.Islands;


namespace Server.Custom.UOSurvivalEvent
{
    internal static class SurvivalUtility
    {
        internal static void SpawnGameBoundry()
        {
            var bounds = SurvivalCore.GameBounds;

            var height = Map.Felucca.GetAverageZ(bounds.Start.X, bounds.Start.Y);

            for (int x = bounds.Start.X; x <= bounds.End.X; x++)
            {
                AddBoundry(Map.Felucca, new Point3D(x, bounds.Start.Y, height));

                AddBoundry(Map.Felucca, new Point3D(x, bounds.End.Y, height));
            }

            for (int y = bounds.Start.Y; y <= bounds.End.Y; y++)
            {
                AddBoundry(Map.Felucca, new Point3D(bounds.Start.X, y, height));

                AddBoundry(Map.Felucca, new Point3D(bounds.End.X, y, height));
            }
        }

        private static void AddBoundry(Map map, Point3D location)
        {
            Static boundry = new Static(Utility.RandomList(0x45B4, 0x45B5));

            boundry.MoveToWorld(location, map);

            SurvivalCore.BoundryList.Add(boundry);
        }

        public static void AddIsland(SurvivalTeam team, Map map, int height)
        {
            var bounds = team.TeamIsland.IslandBounds;

            var center = new Point2D(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);

            PlaceXPedestals(team, map, bounds.Start.X, bounds.End.X, bounds.Start.Y, height);

            PlaceYPedestals(team, map, bounds.Start.Y, bounds.End.Y, bounds.Start.X, height);

            PlaceXPedestals(team, map, bounds.Start.X, bounds.End.X, bounds.End.Y - 1, height);

            PlaceYPedestals(team, map, bounds.Start.Y, bounds.End.Y, bounds.End.X - 1, height);

            for (int x = bounds.Start.X + 1; x < bounds.End.X - 1; x++)
            {
                for (int y = bounds.Start.Y + 1; y < bounds.End.Y - 1; y++)
                {
                    AddSurface(team, map, new Point3D(x, y, height + 6));

                    if (team.TeamIsland.IslandEntity.Name == nameof(DefaultIsland))
                    {
                        AddMask(team, map, new Point3D(x, y, height + 7));
                    }
                    else
                    {
                        if (team.TeamIsland.IslandEntity.Name == nameof(BloodIsland))
                        {
                            AddBlood(team, map, new Point3D(x, y, height + 7));

                            if (center != new Point2D(x, y))
                            {
                                AddSurfaceDecor(team, map, new Point3D(x, y, height + 8));
                            }
                        }
                        else
                        {
                            if (center != new Point2D(x, y))
                            {
                                AddSurfaceDecor(team, map, new Point3D(x, y, height + 7));
                            }
                        }
                    }
                }
            }

            SetTeamTotem(team, map, new Point3D(center.X, center.Y, height + 8));
        }

        private static void PlaceXPedestals(SurvivalTeam team, Map map, int startX, int endX, int y, int height)
        {
            for (int x = startX; x < endX; x++)
            {
                AddPedestal(team, map, new Point3D(x, y, height));
            }
        }

        private static void PlaceYPedestals(SurvivalTeam team, Map map, int startY, int endY, int x, int height)
        {
            for (int y = startY; y < endY; y++)
            {
                AddPedestal(team, map, new Point3D(x, y, height));
            }
        }

        private static void AddPedestal(SurvivalTeam team, Map map, Point3D location, bool pedOnly = false)
        {
            if (!pedOnly)
            {
                Static surface = new Static(0x40A3);

                if (Utility.RandomDouble() < 0.5)
                {
                    Static boulder = new Static(Utility.RandomList(0x9CAA, 0x9CAB, 0x9CAC))
                    {
                        Movable = false
                    };

                    boulder.MoveToWorld(new Point3D(location.X, location.Y, location.Z + 7), map);

                    team.IslandStatics.Add(boulder);
                }
                else
                {
                    Static surfaceTop = new Static(Utility.RandomList(0x40A3, 0x40A4, 0x40A5, 0x40A6, 0x40A7, 0x40A8, 0x40A9, 0x40AA, 0x40AB, 0x40AC, 0x40AD, 0x40AE, 0x40AF))
                    {
                        Movable = false
                    };

                    surfaceTop.MoveToWorld(new Point3D(location.X, location.Y, location.Z + 7), map);

                    team.IslandStatics.Add(surfaceTop);
                }

                surface.Movable = false;

                surface.MoveToWorld(new Point3D(location.X, location.Y, location.Z + 6), map);

                team.IslandStatics.Add(surface);

            }

            Static pedestal = pedOnly ? new Static(0x0720) : new Static(0x32F2);

            pedestal.MoveToWorld(location, map);

            team.IslandStatics.Add(pedestal);
        }

        private static void AddSurface(SurvivalTeam team, Map map, Point3D location)
        {
            var (ID, Hue) = team.TeamIsland.IslandEntity.GetRandomLandTile();

            Static surface = new Static(ID)
            {
                Hue = Hue,
                Movable = false
            };

            surface.MoveToWorld(location, map);

            team.IslandStatics.Add(surface);
        }

        private static void AddMask(SurvivalTeam team, Map map, Point3D location)
        {
            Static mask = new Static(Utility.RandomList(0x050D, 0x050E, 0x050F, 0x0510, 0x0511, 0x0512, 0x0513, 0x0514))
            {
                Movable = false
            };

            mask.MoveToWorld(location, map);

            team.IslandStatics.Add(mask);
        }

        private static void AddBlood(SurvivalTeam team, Map map, Point3D location)
        {
            Static blood = new Static(Utility.RandomList(0x1CD9, 0x1CDA, 0x1CDB, 0x1CDC))
            {
                Movable = false
            };

            blood.MoveToWorld(location, map);

            team.IslandStatics.Add(blood);
        }

        private static void AddSurfaceDecor(SurvivalTeam team, Map map, Point3D location)
        {
            if (Utility.RandomDouble() < 0.25)
            {
                Static decor = new Static(team.TeamIsland.IslandEntity.GetRandomDecorTile())
                {
                    Movable = false
                };

                decor.MoveToWorld(location, map);

                team.IslandStatics.Add(decor);
            }
        }

        private static void SetTeamTotem(SurvivalTeam team, Map map, Point3D location)
        {
            Static totem;

            if (team == SurvivalCore.TeamOne)
            {
                totem = new Static(0x9F41);
            }
            else
            {
                totem = new Static(0x9F5E);
            }

            totem.Movable = false;

            totem.MoveToWorld(location, map);

            team.AddTotem(totem);

            PlayTotemEffect(map, location, team == SurvivalCore.TeamOne);

            team.AddHomeLocation(new Point3D(location.X + 1, location.Y + 1, location.Z - 2));
        }

        internal static void TryResPlayer(Mobile m, SurvivalTeam team)
        {
            if (!m.Alive)
            {
                if (!team.IslandHasPlayer(m))
                {
                    ReturnHome(m, team);
                }

                m.Resurrect();

                if (Utility.RandomDouble() < 0.5)
                {
                    SurvivalCore.SendTotemMsg(team, $"{m.Name}, Get back out there and fight!");
                }
                else
                {
                    SurvivalCore.SendTotemMsg(team, $"{m.Name}, Your alive, for now, see you soon!");
                }
            }
        }

        internal static void ReturnHome(Mobile m, SurvivalTeam team)
        {
            PlayTeleportEffect(team.TeamTotem.Map, team.TeamTotem.Location);

            m.MoveToWorld(team.HomeLocation, m.Map);

            m.AddToBackpack(new RecallScroll());

            m.SendMessage(53, "A recall scroll magically appeared in your pack!");

            Timer.DelayCall(TimeSpan.FromMilliseconds(500), () =>
            {
                if (m.Z < team.HomeLocation.Z)
                {
                    m.Z = team.HomeLocation.Z;
                }
            });
        }

        public static void PlayTotemEffect(Map map, Point3D location, bool teamOne)
        {
            if (teamOne)
            {
                Effects.SendLocationEffect(location, map, 0x9F42, 9);

                Effects.PlaySound(location, map, 0x23B);
            }
            else
            {
                Effects.SendLocationEffect(location, map, 0x9F5F, 9);

                Effects.PlaySound(location, map, 0x23C);
            }
        }

        public static void PlayHealEffect(Map map, Point3D location)
        {
            Effects.SendLocationEffect(location, map, 0x3789, 15);

            Effects.PlaySound(location, map, 0x1F2);
        }

        public static void PlayDamageEffect(Map map, Point3D location)
        {
            Effects.SendLocationEffect(location, map, Utility.RandomList(0x42CF, 0x374A), 15);

            Effects.PlaySound(location, map, 0x1F8);
        }

        internal static void PlayFireEffect(Map map, Point3D location)
        {
            Effects.SendLocationEffect(location, map, Utility.RandomList(0x36B0, 0x36BD), 13);

            Effects.PlaySound(location, map, Utility.RandomList(0x11B, 0x11C, 0x11D, 0x11E));
        }

        internal static void PlayColdEffect(Map map, Point3D location)
        {
            Effects.SendLocationEffect(location, map, 0x3789, 20);

            Effects.PlaySound(location, map, Utility.RandomList(0xFA, 0xFC));
        }

        public static void PlayPoisonEffect(Mobile m, Map map, Point3D location)
        {
            m.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);

            Effects.PlaySound(location, map, 0x205);
        }

        internal static void PlayWindEffect(Map map, Point3D location)
        {
            Effects.SendLocationEffect(location, map, 0x37CC, 25, 2, 0x4001, 0);

            Effects.PlaySound(location, map, Utility.RandomList(0x14, 0x15, 0x16));
        }

        public static void PlayTeleportEffect(Map map, Point3D location)
        {
            Effects.SendLocationEffect(location, map, 0x9F89, 15);

            Effects.PlaySound(location, map, Utility.RandomList(0x3E, 0x3F));
        }

        internal static void PlayBloodEffect(Map map, Point3D location)
        {
            Effects.SendLocationEffect(location, map, Utility.RandomList(0x37B9, 0x37BE, 0x37C4), 15);

            Effects.PlaySound(location, map, Utility.RandomList(0x5DC, 0x5DD));
        }

        public static void PlayMushroomEffect(Static mushroom, Map map, Point3D location)
        {
            mushroom.ItemID = 0x1126;

            Effects.PlaySound(location, map, 0x306);

            Timer.DelayCall(TimeSpan.FromSeconds(2), () => { OnMushroomReset(mushroom); });
        }

        private static void OnMushroomReset(Static mushroom)
        {
            mushroom.ItemID = 0x1125;
        }

        internal static DefaultIsland GetIslandEntity(IslandType biome)
        {
            switch (biome)
            {
                case IslandType.Grass:  return new GrassIsland();
                case IslandType.Forest: return new ForestIsland();
                case IslandType.Desert: return new DesertIsland();
                case IslandType.Snow:   return new SnowIsland();
                case IslandType.Swamp:  return new SwampIsland();
                case IslandType.Fire:   return new FireIsland();
                case IslandType.Blood:  return new BloodIsland();
            }

            return new DefaultIsland();
        }

        internal static void UpdateWeather(bool isHot)
        {
            if (!BaseIsland.HasWeather())
            {
                BaseIsland.SetWeather(isHot);
            }
        }
    }
}

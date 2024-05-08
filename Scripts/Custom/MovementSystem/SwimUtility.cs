using System;
using Server.Misc;
using System.Linq;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.MovementSystem
{
    internal class SwimUtility
    {
        private static readonly Dictionary<Mobile, List<(Item, int)>> PlayerWares = new Dictionary<Mobile, List<(Item, int)>>();

        private static void ModItemHue(Mobile from, Layer layer, bool isSwim)
        {
            if (isSwim)
            {
                if (from.FindItemOnLayer(layer) != null)
                {
                    var item = from.FindItemOnLayer(layer);

                    PlayerWares[from].Add((item, item.Hue));

                    item.Hue = MoveSettings.SwimHueMod;
                }
            }
            else
            {
                if (PlayerWares.ContainsKey(from))
                {
                    for (int i = 0; i < PlayerWares[from].Count; i++)
                    {
                        PlayerWares[from][i].Item1.Hue = PlayerWares[from][i].Item2;
                    }

                    PlayerWares.Remove(from);
                }
            }
        }

        internal static void StartSwimming(Mobile from)
        {
            if (!PlayerWares.ContainsKey(from))
            {
                if (!PlayerWares.TryGetValue(from, out List<(Item, int)> list))
                {
                    PlayerWares.Add(from, new List<(Item, int)>());
                }

                MovementCore.AddPlayerLoc(from);

                ModItemHue(from, Layer.Shoes, true);

                ModItemHue(from, Layer.OuterTorso, true);

                ModItemHue(from, Layer.OuterLegs, true);

                if (from.HasGump(typeof(MovementGump)))
                {
                    from.CloseGump(typeof(MovementGump));
                }

                BaseGump.SendGump(new MovementGump(from as PlayerMobile));
            }

            from.ClearHands();

            int time = MoveSettings.SwimSpeedMod;

            if (IsFast(from.Direction))
            {
                time = MoveSettings.SwimSpeedMod - (MoveSettings.SwimSpeedMod / 3);
            }

            Timer.DelayCall(TimeSpan.FromMilliseconds(time), () =>
            {
                if (HasWater(from.Location, from.Map))
                {
                    if (from.Stam > (MoveSettings.SwimStamMod * 4))
                    {
                        SwimAnimation(from);

                        PlayWaterEffect(from);

                        if (HasWater(from.Direction, from.Location, from.Map, out bool isDeep))
                        {
                            from.X += GetOffset(from.Direction, out int val);

                            from.Y += val;

                            PlayWaterEffect(from, true);

                            if (time > MoveSettings.SwimSpeedMod)
                            {
                                from.Stam -= isDeep ? (MoveSettings.SwimStamMod * 4) : (MoveSettings.SwimStamMod * 2);
                            }
                            else
                            {
                                from.Stam -= isDeep ? (MoveSettings.SwimStamMod * 2) : MoveSettings.SwimStamMod;
                            }

                            if (isDeep)
                            {
                                double chance = Weather.CheckContains(new Rectangle2D(from.X, from.Y, 3, 3), new Rectangle2D(from.X, from.Y, 3, 3)) ? 0.1 : 0.01;

                                if (Utility.RandomDouble() < chance)
                                {
                                    from.Damage(Utility.RandomMinMax(1, MoveSettings.SwimMaxDamage));

                                    from.SendMessage(42, "Hyperthermia is killing you!");
                                }
                            }
                        }
                        else
                        {
                            if (Utility.RandomDouble() < 0.1)
                            {
                                from.SendMessage(53, "You are wading water!");
                            }

                            from.Stam++;

                            from.Direction = GetSlowDirection(from.Direction);
                        }
                    }
                    else
                    {
                        from.SendMessage(42, "Your exhausted!");

                        from.Stam++;

                        from.Direction = GetSlowDirection(from.Direction);
                    }

                    if (HasWater(from.Location, from.Map))
                    {
                        StartSwimming(from);
                    }
                    else
                    {
                        StopSwimming(from);
                    }
                }
            });
        }

        internal static void StopSwimming(Mobile from)
        {
            if (HasWater(from.Location, from.Map))
            {
                ModItemHue(from, Layer.OuterTorso, false);

                MovementCore.RemovePlayerLoc(from);
            }

            from.Mana = from.ManaMax;
        }

        private static readonly int[] m_WaterTiles = new int[]
        {
            //Shallow Water
            0x5797, 0x579C,
            0x746E, 0x7485,
            0x7490, 0x74AB,
            0x74B5, 0x75D5,
            //Static tiles
            0x1797, 0x1798,
            0x1799, 0x179A,
            0x179B, 0x179C,
            0x179D, 0x179E,
            0x179F, 0x17A0,
            0x17A1, 0x17A2,
            0x17A3, 0x17A3,
            0x17A5, 0x17A6,
            0x17A7, 0x17A8,
            0x17A9, 0x17AA,
            0x17AB, 0x17AC,
        };

        private static readonly int[] m_DeepWaterTiles = new int[]
        {
            //Deep Water
            0x00AA, 0x00A9,
            0x00A8, 0x00AB,
            0x0136, 0x0137,
        };

        private static void SwimAnimation(Mobile from)
        {
            from.Animate(AnimationType.Spell, 0);

            if (IsFast(from.Direction))
            {
                Effects.PlaySound(from.Location, from.Map, Utility.RandomList(0x27, 0x5A4));
            }
            else
            {
                Effects.PlaySound(from.Location, from.Map, Utility.RandomList(0x25, 0x5A4));
            }
        }

        internal static void SplashAnimation(Mobile from, Point3D location)
        {
            Effects.SendLocationEffect(location, from.Map, 0x322C, 18, 2, 0);

            Effects.PlaySound(location, from.Map, 0x26);
        }

        private static void PlayWaterEffect(Mobile from, bool isLarge = false)
        {
            var topLocation = new Point3D(from.Location.X + 1, from.Location.Y + 1, from.Location.Z + MoveSettings.SwimSinkMod);

            var midLocation = new Point3D(from.Location.X + 1, from.Location.Y + 1, from.Location.Z + (MoveSettings.SwimSinkMod / 2));

            if (isLarge)
            {
                Effects.SendLocationEffect(topLocation, from.Map, 0x1FB7, 15, 0, 0);
                Effects.SendLocationEffect(topLocation, from.Map, 0x1FBC, 15, 0, 0);
                Effects.SendLocationEffect(topLocation, from.Map, 0x1FC1, 15, 0, 0);
                Effects.SendLocationEffect(topLocation, from.Map, 0x1FC6, 15, 0, 0);
            }
            else
            {
                Effects.SendLocationEffect(topLocation, from.Map, 0x322C, 18, 2, 0);

                Effects.SendLocationEffect(topLocation, from.Map, 0x1FA3, 15, 0, 0);
                Effects.SendLocationEffect(topLocation, from.Map, 0x1FA8, 15, 0, 0);
                Effects.SendLocationEffect(topLocation, from.Map, 0x1FAD, 15, 0, 0);
                Effects.SendLocationEffect(topLocation, from.Map, 0x1FB2, 15, 0, 0);

                Effects.SendLocationEffect(midLocation, from.Map, Utility.RandomList(0x1153, 0x1158), 18, 1153, 0);
            }
        }

        private static bool HasWater(Direction direction, Point3D location, Map map, out bool isDeep)
        {
            int x = location.X + GetOffset(direction, out int val);

            int y = location.Y + val;

            var landTile = map.Tiles.GetLandTile(x, y).ID & TileData.MaxLandValue;

            StaticTile[] tiles = map.Tiles.GetStaticTiles(x, y, false);

            bool hasWater = false;

            bool isGood = true;

            bool isdeep = false;

            if (m_DeepWaterTiles.Contains(landTile))
            {
                isdeep = true;

                hasWater = true;
            }
            else
            {
                for (int i = 0; i < tiles.Length; ++i)
                {
                    if (m_WaterTiles.Contains(tiles[i].ID) || m_DeepWaterTiles.Contains(tiles[i].ID))
                    {
                        if (m_DeepWaterTiles.Contains(tiles[i].ID))
                        {
                            isdeep = true;
                        }

                        hasWater = true;
                    }
                    else
                    {
                        int total = location.Z + MoveSettings.SwimSinkMod + 5;

                        if (tiles[i].Z < total)
                        {
                            isGood = false;
                        }
                    }
                }
            }

            isDeep = isdeep;

            if (isGood && hasWater)
            {
                return true;
            }

            return false;
        }

        internal static bool HasWater(Point3D location, Map map)
        {
            var landTile = map.Tiles.GetLandTile(location.X, location.Y).ID & TileData.MaxLandValue;

            if (m_DeepWaterTiles.Contains(landTile))
            {
                return true;
            }

            StaticTile[] tiles = map.Tiles.GetStaticTiles(location.X, location.Y, false);

            for (int i = 0; i < tiles.Length; ++i)
            {
                if (m_WaterTiles.Contains(tiles[i].ID) || m_DeepWaterTiles.Contains(tiles[i].ID))
                {
                    if (location.Z < tiles[i].Z)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static int GetOffset(Direction direction, out int offset_Y)
        {
            if (direction == Direction.Mask || direction == Direction.ValueMask)
            {
                offset_Y = -1;

                return -1;
            }
            else if (direction == Direction.Down || direction.ToString() == "131") 
            {
                offset_Y = 1;

                return 1;
            }
            else if (direction == Direction.Left || direction.ToString() == "133")
            {
                offset_Y = 1;

                return -1;
            }
            else if (direction == Direction.Right || direction.ToString() == "129")
            {
                offset_Y = -1;

                return 1;
            }
            else if (direction == Direction.North || direction == Direction.Running)
            {
                offset_Y = -1;

                return 0;
            }
            else if (direction == Direction.South || direction.ToString() == "132")
            {
                offset_Y = 1;

                return 0;
            }
            else if (direction == Direction.East || direction.ToString() == "130")
            {
                offset_Y = 0;

                return 1;
            }
            else if (direction == Direction.West || direction.ToString() == "134")
            {
                offset_Y = 0;

                return -1;
            }
            else
            {
                offset_Y = 0;

                return 0;
            }
        }

        private static bool IsFast(Direction direction)
        {
            if (direction == Direction.ValueMask)
            {
                return true;
            }
            else if (direction.ToString() == "131")
            {
                return true;
            }
            else if (direction.ToString() == "133")
            {
                return true;
            }
            else if (direction.ToString() == "129")
            {
                return true;
            }
            else if (direction == Direction.Running)
            {
                return true;
            }
            else if (direction.ToString() == "132")
            {
                return true;
            }
            else if (direction.ToString() == "130")
            {
                return true;
            }
            else if (direction.ToString() == "134")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Direction GetSlowDirection(Direction direction)
        {
            if (direction == Direction.ValueMask)
            {
                return Direction.Mask;
            }
            else if (direction.ToString() == "131")
            {
                return Direction.Down;
            }
            else if (direction.ToString() == "133")
            {
                return Direction.Left;
            }
            else if (direction.ToString() == "129")
            {
                return Direction.Right;
            }
            else if (direction == Direction.Running)
            {
                return Direction.North;
            }
            else if (direction.ToString() == "132")
            {
                return Direction.South;
            }
            else if (direction.ToString() == "130")
            {
                return Direction.East;
            }
            else if (direction.ToString() == "134")
            {
                return Direction.West;
            }
            else
            {
                return direction;
            }
        }
    }
}

using System.Linq;

namespace Server.Custom.UOSurvivalEvent.Islands
{
    internal class DesertIsland : DefaultIsland
    {
        public DesertIsland() : base(nameof(DesertIsland))
        {
        }

        public override (int ID, int Hue) GetRandomLandTile()
        {
            return Utility.RandomList // sand
                (
                    (0x245E, 0), (0x245F, 0),
                    (0x2460, 0), (0x2461, 0),
                    (0x2462, 0), (0x2463, 0),
                    (0x2468, 0), (0x2469, 0),
                    (0x246A, 0), (0x246B, 0)
                ); 
        }

        public override int GetRandomDecorTile()
        {
            if (Utility.RandomDouble() < 0.5)
            {
                if (Utility.RandomDouble() < 0.2)
                {
                    return Utility.RandomList(0x2464, 0x2465); // hot rocks
                }
                else
                {
                    return Utility.RandomList // misc items
                        (
                            0x24D5, 0x24D6, 0x24D7,
                            0x24D8, 0x24D9, 0x24DA,
                            0x24DB, 0x24DC, 0x24DD,
                            0x24DE, 0x24DF, 0x24E0
                        );
                }
            }
            else
            {
                if (Utility.RandomDouble() < 0.2)
                {
                    return Utility.RandomList(0x2466, 0x2467); // hot rocks
                }
                else
                {
                    return Utility.RandomList(0x246C, 0x246D, 0x246E, 0x246F, 0x2470); // Bamboo
                }
            }
        }

        public override void CheckIslandMove(SurvivalTeam myTeam, SurvivalTeam theirTeam, Mobile m)
        {
            if (m.Alive && theirTeam.IslandHasPlayer(m))
            {
                theirTeam.TeamIsland.CheckTotem(myTeam, theirTeam, m);

                var tiles = theirTeam.IslandStatics?.FindAll(s => s.Location.X == m.Location.X && s.Location.Y == m.Location.Y)?.ToList();

                if (tiles != null && tiles.Count > 0)
                {
                    bool wasHit = false;

                    for (int i = 0; i < tiles.Count; i++)
                    {
                        switch (tiles[i].ItemID)
                        {
                            case 0x2464:
                                {
                                    if (Utility.RandomDouble() > m.FireResistance * 0.01)
                                    {
                                        SurvivalUtility.PlayFireEffect(m.Map, m.Location);

                                        m.Damage(Utility.RandomMinMax(1, m.Hits / 4));

                                        m.SendMessage(53, "You sustain heat damage!");

                                        wasHit = true;
                                    }

                                    break;
                                }

                            case 0x2465:
                                {
                                    if (Utility.RandomDouble() > m.ColdResistance * 0.01)
                                    {
                                        SurvivalUtility.PlayColdEffect(m.Map, m.Location);

                                        m.Damage(Utility.RandomMinMax(1, m.Hits / 4));

                                        m.SendMessage(53, "You sustain cold damage!");

                                        wasHit = true;
                                    }

                                    break;
                                }

                            case 0x2466:
                                {
                                    if (Utility.RandomDouble() > m.PoisonResistance * 0.01)
                                    {
                                        SurvivalUtility.PlayPoisonEffect(m, m.Map, m.Location);

                                        m.Damage(Utility.RandomMinMax(1, m.Hits / 4));

                                        m.ApplyPoison(m, Poison.Lesser);

                                        m.SendMessage(53, "You sustain poison damage!");

                                        wasHit = true;
                                    }

                                    break;
                                }

                            case 0x2467:
                                {
                                    if (Utility.RandomDouble() > m.EnergyResistance * 0.01)
                                    {
                                        m.BoltEffect(Utility.RandomYellowHue());

                                        m.Damage(Utility.RandomMinMax(1, m.Hits / 4));

                                        m.SendMessage(53, "You sustain electrical damage!");

                                        wasHit = true;
                                    }

                                    break;
                                }
                        }
                    }

                    if (!wasHit && Utility.RandomDouble() < 0.05)
                    {
                        m.Frozen = true;

                        SurvivalUtility.PlayWindEffect(m.Map, m.Location);

                        if (m.Hits > 9)
                        {
                            m.Damage(Utility.RandomMinMax(1, m.Hits / 10));
                        }
                        else
                        {
                            m.Damage(Utility.RandomMinMax(1, m.Hits - 1));
                        }

                        if (Utility.RandomDouble() < 0.1)
                        {
                            m.SendMessage(53, "You sustained damage slipping in the sand!");

                            switch (Utility.Random(7))
                            {
                                case 0:
                                    {
                                        m.Direction = Direction.North;

                                        break;
                                    }

                                case 1:
                                    {
                                        m.Direction = Direction.Up;

                                        break;
                                    }

                                case 2:
                                    {
                                        m.Direction = Direction.South;

                                        break;
                                    }

                                case 3:
                                    {
                                        m.Direction = Direction.Down;

                                        break;
                                    }

                                case 4:
                                    {
                                        m.Direction = Direction.West;

                                        break;
                                    }

                                case 5:
                                    {
                                        m.Direction = Direction.Left;

                                        break;
                                    }

                                case 6:
                                    {
                                        m.Direction = Direction.East;

                                        break;
                                    }

                                case 7:
                                    {
                                        m.Direction = Direction.Right;

                                        break;
                                    }
                            }
                        }
                        else
                        {
                            m.SendMessage(53, "You sustained damage from the sand!");
                        }

                        m.Frozen = false;
                    }
                }
            }
            else
            {
                SurvivalUtility.TryResPlayer(m, myTeam);
            }
        }
    }
}

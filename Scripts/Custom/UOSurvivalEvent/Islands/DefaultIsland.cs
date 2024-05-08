using System;
using System.Linq;
using Server.Items;

namespace Server.Custom.UOSurvivalEvent.Islands
{
    public class DefaultIsland
    {
        public string Name { get; private set; } = nameof(DefaultIsland);

        public DefaultIsland()
        {
        }

        public DefaultIsland(string name)
        {
            Name = name;
        }

        public virtual (int ID, int Hue) GetRandomLandTile()
        {
            if (Utility.RandomDouble() < 0.5)
            {
                return Utility.RandomList((0x9B34, 2498), (0x9B39, 2749), (0x9B3A, 2753), (0x9B3B, 2743));
            }
            else
            {
                return (0x9B34, 2498);
            }
        }

        public virtual int GetRandomDecorTile()
        {
            return Utility.RandomList(-1);
        }

        public virtual void CheckIslandMove(SurvivalTeam myTeam, SurvivalTeam theirTeam, Mobile m)
        {
            if (m.Alive && theirTeam.IslandHasPlayer(m))
            {
                var tiles = theirTeam.IslandStatics?.FindAll(s => s.Location.X == m.Location.X && s.Location.Y == m.Location.Y)?.ToList();

                if (tiles != null && tiles.Count > 0)
                {
                    theirTeam.TeamIsland.CheckTotem(myTeam, theirTeam, m);

                    foreach (var tile in tiles)
                    {
                        if (SurvivalCore.MaskTiles.Contains(tile.ItemID))
                        {
                            tile.Delete();
                        }
                        else
                        {
                            switch (tile.ItemID)
                            {
                                case 0x9B34: // Safe Tile
                                    {
                                        m.SendMessage(62, "That was safe!");

                                        SurvivalUtility.PlayHealEffect(myTeam.TeamTotem.Map, myTeam.TeamTotem.Location);

                                        if (Utility.RandomDouble() < 0.1)
                                        {
                                            m.Heal(Utility.RandomMinMax(1, 10));

                                            m.Say(62, "That felt goood!");
                                        }

                                        break;
                                    }

                                case 0x9B39: // Damage Tile
                                    {
                                        m.SendMessage(42, "That was damaging, opps!");

                                        SurvivalUtility.PlayDamageEffect(myTeam.TeamTotem.Map, myTeam.TeamTotem.Location);

                                        if (m.Alive)
                                        {
                                            if (m.Hits > 10)
                                            {
                                                m.Damage(Utility.RandomMinMax(m.Hits / 10, m.Hits / 2));
                                            }
                                            else
                                            {
                                                m.Damage(Utility.RandomMinMax(1, m.Hits - 1));
                                            }
                                        }

                                        break;
                                    }

                                case 0x9B3A: // Lethal
                                    {
                                        m.SendMessage(42, "That was lethal, oh my you might die!");

                                        if (m.Alive && m.Hits > 0)
                                        {
                                            m.Damage(m.Hits - 1);

                                            if (!m.Poisoned)
                                            {
                                                m.ApplyPoison(m, Poison.Lethal);

                                                SurvivalUtility.PlayPoisonEffect(m, myTeam.TeamTotem.Map, myTeam.TeamTotem.Location);
                                            }
                                            else
                                            {
                                                SurvivalUtility.PlayDamageEffect(myTeam.TeamTotem.Map, myTeam.TeamTotem.Location);
                                            }
                                        }

                                        break;
                                    }

                                case 0x9B3B: // Teleport
                                    {
                                        m.SendMessage(42, "That was unlucky, start again!");

                                        SurvivalUtility.ReturnHome(m, myTeam);

                                        SurvivalCore.SendTotemMsg(myTeam, "Hurry up, get back into the fight!");

                                        break;
                                    }
                            }
                        }
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

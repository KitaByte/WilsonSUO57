using System.Collections.Generic;
using System.Linq;

namespace Server.Custom.UOSurvivalEvent.Islands
{
    internal class SwampIsland : DefaultIsland
    {
        public SwampIsland() : base(nameof(SwampIsland))
        {
        }

        public override (int ID, int Hue) GetRandomLandTile()
        {
            return Utility.RandomList((0x3258, 0), (0x3259, 0), (0x325A, 0), (0x325B, 0), (0x325C, 0), (0x325D, 0)); // swamp
        }

        public override int GetRandomDecorTile()
        {
            if (Utility.RandomDouble() < 0.9)
            {
                if (Utility.RandomDouble() < 0.1)
                {
                    return Utility.RandomList(0xA85F, 0xA860); // swamp
                }
                else
                {
                    return Utility.RandomList // Logs
                        (
                             0x324B, 0x324C, 0x324D,
                             0x325E, 0x325F, 0x3260,
                             0x1B09, 0x1B0A, 0x1B0B,
                             0x1B0C, 0x1B0D, 0x1B0E,
                             0x1B0F, 0x1B10
                        );
                }
            }
            else
            {
                return Utility.RandomList(0x322C, 0x0D05, 0x11A6);
            }
        }

        private readonly List<int> m_SwampPiles = new List<int>() { 0xA85F, 0xA860 };

        public override void CheckIslandMove(SurvivalTeam myTeam, SurvivalTeam theirTeam, Mobile m)
        {
            if (m.Alive && theirTeam.IslandHasPlayer(m))
            {
                theirTeam.TeamIsland.CheckTotem(myTeam, theirTeam, m);

                var tiles = theirTeam.IslandStatics?.FindAll(s => s.Location.X == m.Location.X && s.Location.Y == m.Location.Y)?.ToList();

                if (tiles != null && tiles.Count > 0)
                {
                    for (int i = 0; i < tiles.Count; i++)
                    {
                        if (m_SwampPiles.Contains(tiles[i].ItemID))
                        {
                            if (Utility.RandomDouble() > m.PoisonResistance * 0.01)
                            {
                                SurvivalUtility.PlayPoisonEffect(m, m.Map, m.Location);

                                if (m.Poisoned)
                                {
                                    m.Damage(Utility.Random(m.Hits / 2));
                                }
                                else
                                {
                                    m.ApplyPoison(m, Utility.RandomList(Poison.Lesser, Poison.Deadly, Poison.Lethal));
                                }

                                m.SendMessage(62, "You are being poisoned to death!");
                            }
                        }
                        else
                        {
                            if (!m.Poisoned && Utility.RandomDouble() < 0.1)
                            {
                                m.ApplyPoison(m, Poison.Lesser);

                                m.SendMessage(62, "You are being poisoned to death!");
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

using System.Collections.Generic;
using System.Linq;

namespace Server.Custom.UOSurvivalEvent.Islands
{
    internal class SnowIsland : DefaultIsland
    {
        public SnowIsland() : base(nameof(SnowIsland))
        {
        }

        public override (int ID, int Hue) GetRandomLandTile()
        {
            return Utility.RandomList((0x17BD, 0), (0x17BE, 0), (0x17BF, 0), (0x17C0, 0), (0x4C0E, 0), (0x4C0F, 0), (0x4C10, 0), (0x4C11, 0)); // snow
        }

        public override int GetRandomDecorTile()
        {
            if (Utility.RandomDouble() < 0.9)
            {
                if (Utility.RandomDouble() < 0.3)
                {
                    return Utility.RandomList(0xA672, 0xA673, 0xA674, 0xA675, 0x17CD, 0x17CE); // Snow Piles
                }
                else
                {
                    return Utility.RandomList // Crystals
                        (
                            0x2FDC, 0x2FDD, 0x2FE6,
                            0x2FE7, 0x9CB6, 0x9CB5,
                            0x2207, 0x2208, 0x2211,
                            0x2212, 0x221B, 0x221C,
                            0x2225, 0x2226, 0x9CB7,
                            0x9CB8, 0x9CAA, 0x9CAB,
                            0x9CAC
                        );
                }
            }
            else
            {
                return Utility.RandomList // Animations
                    (
                        0x1153, 0x9CBB, 0x9CC1,
                        0x9CC8, 0x9CB8, 0x9CAC,
                        0x9CAB, 0x483B, 0x4853,
                        0x4883, 0x3967, 0x3979
                    );
            }
        }

        private readonly List<int> m_Piles = new List<int>() { 0xA672, 0xA673, 0xA674, 0xA675, 0x17CD, 0x17CE };

        public override void CheckIslandMove(SurvivalTeam myTeam, SurvivalTeam theirTeam, Mobile m)
        {
            SurvivalUtility.UpdateWeather(false);

            if (m.Alive && theirTeam.IslandHasPlayer(m))
            {
                theirTeam.TeamIsland.CheckTotem(myTeam, theirTeam, m);

                var tiles = theirTeam.IslandStatics?.FindAll(s => s.Location.X == m.Location.X && s.Location.Y == m.Location.Y)?.ToList();

                if (tiles != null && tiles.Count > 0)
                {
                    for (int i = 0; i < tiles.Count; i++)
                    {
                        if (m_Piles.Contains(tiles[i].ItemID))
                        {
                            if (Utility.RandomDouble() > m.ColdResistance * 0.01)
                            {
                                SurvivalUtility.PlayColdEffect(m.Map, m.Location);

                                m.Damage(Utility.Random(m.Hits / 2));

                                m.SendMessage(53, "You are freezing to death!");
                            }
                        }
                        else
                        {
                            if (Utility.RandomDouble() < 0.1)
                            {
                                SurvivalUtility.PlayColdEffect(m.Map, m.Location);

                                m.Damage(Utility.RandomMinMax(5, 15));

                                m.SendMessage(53, "You are freezing to death!");
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

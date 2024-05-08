using System.Collections.Generic;
using System.Linq;

namespace Server.Custom.UOSurvivalEvent.Islands
{
    internal class FireIsland : DefaultIsland
    {
        public FireIsland() : base(nameof(FireIsland))
        {
        }

        public override (int ID, int Hue) GetRandomLandTile()
        {
            int hue = Utility.RandomList(Utility.RandomRedHue(), Utility.RandomYellowHue(), Utility.RandomOrangeHue(), 1175);

            return Utility.RandomList((0x0495, hue), (0x0496, hue), (0x0497, hue), (0x0498, hue)); // marble
        }

        public override int GetRandomDecorTile()
        {
            if (Utility.RandomDouble() < 0.9)
            {
                if (Utility.RandomDouble() < 0.3)
                {
                    return Utility.RandomList // cracks
                        (
                            0x1B01, 0x1B02, 0x1B03,
                            0x1B04, 0x1B05, 0x1B06,
                            0x1B07, 0x1B08
                        ); 
                }
                else
                {
                    return Utility.RandomList // stone steps
                        (
                            0x0915, 0x0916,
                            0x0917, 0x0918
                        );
                }
            }
            else
            {
                return Utility.RandomList // Animations
                    (
                        0x1A75, 0x2AE4,
                        0x3709, 0x0DE3,
                        0x29FD
                    );
            }
        }

        private readonly List<int> m_Cracks = new List<int>()
        {
            0x1B01, 0x1B02, 0x1B03,
            0x1B04, 0x1B05, 0x1B06,
            0x1B07, 0x1B08
        };

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
                        if (m_Cracks.Contains(tiles[i].ItemID))
                        {
                            if (Utility.RandomDouble() > m.FireResistance * 0.01)
                            {
                                SurvivalUtility.PlayFireEffect(m.Map, m.Location);

                                m.Damage(Utility.Random(m.Hits / 2));

                                m.SendMessage(53, "You are burning to death!");
                            }
                        }
                        else
                        {
                            if (Utility.RandomDouble() < 0.5 && m.Z <= myTeam.HomeLocation.Z)
                            {
                                SurvivalUtility.PlayFireEffect(m.Map, m.Location);

                                m.Damage(Utility.RandomMinMax(5, 15));

                                m.SendMessage(53, "You are burning to death!");
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

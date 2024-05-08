using System.Collections.Generic;
using System.Linq;

namespace Server.Custom.UOSurvivalEvent.Islands
{
    internal class BloodIsland : DefaultIsland
    {
        public BloodIsland() : base(nameof(BloodIsland))
        {
        }

        public override (int ID, int Hue) GetRandomLandTile()
        {
            int hue = Utility.RandomList(Utility.RandomRedHue());

            return Utility.RandomList((0x0495, hue), (0x0496, hue), (0x0497, hue), (0x0498, hue), (0x07BD, 1175), (0x07BE, 1175)); // marble
        }

        public override int GetRandomDecorTile()
        {
            if (Utility.RandomDouble() < 0.95)
            {
                if (Utility.RandomDouble() < 0.4)
                {
                    return Utility.RandomList // splatter
                        (
                            0x122A, 0x122B,
                            0x122C, 0x122D,
                            0x122E, 0x122F
                        );
                }
                else
                {
                    return Utility.RandomList // blood decor
                        (
                            0x1CDD, 0x1CDE, 0x1CDF,
                            0x1CE0, 0x1CE1, 0x1CE2,
                            0x1CE3, 0x1CE4, 0x1CE5,
                            0x1CE6, 0x1CE7, 0x1CE8,
                            0x1CE9, 0x1CEA, 0x1CEB,
                            0x1CEC, 0x1CED, 0x1CEE,
                            0x1CEF, 0x1CF0, 0x1E90,
                            0x1E91, 0x1E88, 0x1E89
                        );
                }
            }
            else
            {
                return Utility.RandomList // Animations
                    (
                        0x374A, 0x42CF,
                        0x4AA4
                    );
            }
        }

        private readonly List<int> m_Splatter = new List<int>()
        {
            0x122A, 0x122B,
            0x122C, 0x122D,
            0x122E, 0x122F
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
                        if (m_Splatter.Contains(tiles[i].ItemID))
                        {
                            if (Utility.RandomDouble() > m.Skills.MagicResist.Value * 0.01)
                            {
                                SurvivalUtility.PlayBloodEffect(m.Map, m.Location);

                                m.Damage(Utility.Random(m.Hits / 2));

                                m.SendMessage(53, "You are bleeding to death!");
                            }
                        }
                        else
                        {
                            if (Utility.RandomDouble() < 0.5 && m.Z <= myTeam.HomeLocation.Z)
                            {
                                SurvivalUtility.PlayBloodEffect(m.Map, m.Location);

                                m.Damage(Utility.RandomMinMax(5, 15));

                                m.SendMessage(53, "You are bleeding to death!");
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

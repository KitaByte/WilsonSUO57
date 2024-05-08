using System.Linq;

namespace Server.Custom.UOSurvivalEvent.Islands
{
    internal class ForestIsland : DefaultIsland
    {
        public ForestIsland() : base(nameof(ForestIsland))
        {
        }

        public override (int ID, int Hue) GetRandomLandTile()
        {
            return (0x337A, 0); // forest
        }

        public override int GetRandomDecorTile() 
        {
            if (Utility.RandomDouble() < 0.5)
            {
                if (Utility.RandomDouble() < 0.2)
                {
                    return 0x1125; // killer mushroom
                }
                else
                {
                    return Utility.RandomList(0x0C9F, 0x0CA0, 0x0CA1, 0x0CA2, 0x0CA3, 0x0CA4); // trees
                }
            }
            else
            {
                if (Utility.RandomDouble() < 0.2)
                {
                    return 0x1125; // killer mushroom
                }
                else
                {
                    return Utility.RandomList(0xA63F, 0xA640, 0xA641, 0xA642, 0xA643, 0xA644); // ferns
                }
            }
        }

        public override void CheckIslandMove(SurvivalTeam myTeam, SurvivalTeam theirTeam, Mobile m)
        {
            SurvivalUtility.UpdateWeather(true);

            if (m.Alive && theirTeam.IslandHasPlayer(m))
            {
                theirTeam.TeamIsland.CheckTotem(myTeam, theirTeam, m);

                var tiles = theirTeam.IslandStatics?.FindAll(s => s.Location.X == m.Location.X && s.Location.Y == m.Location.Y)?.ToList();

                if (tiles != null && tiles.Count > 0)
                {
                    for (int i = 0; i < tiles.Count; i++)
                    {
                        if (tiles[i].ItemID == 0x1125)
                        {
                            if (Utility.RandomDouble() > (int)m.Skills.DetectHidden.Value * 0.01)
                            {
                                SurvivalUtility.PlayMushroomEffect(tiles[i], tiles[i].Map, tiles[i].Location);

                                m.Damage(Utility.RandomMinMax(1, m.Hits - 1));
                            }
                        }
                        else
                        {
                            if (Utility.RandomDouble() < 0.05)
                            {
                                m.BoltEffect(Utility.RandomBlueHue());

                                m.Damage(Utility.RandomMinMax(1, m.Hits - 1));
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

using Server.Items;
using System;

namespace Server.Custom.UOSurvivalEvent.Islands
{
    public class GrassIsland : DefaultIsland
    {
        public GrassIsland() : base(nameof(GrassIsland))
        {
        }

        public override (int ID, int Hue) GetRandomLandTile()
        {
            return Utility.RandomList((0x177D, 0), (0x177E, 0), (0x177F, 0), (0x1780, 0), (0x1781, 0));
        }

        public override int GetRandomDecorTile()
        {
            return Utility.RandomList(0x0CAC, 0x0CAD, 0x0CAE, 0x0CAF, 0x0CB0, 0x0CB1, 0x0CB2, 0x0CB3, 0x0CB4, 0x0CB5, 0x0CB6, 0x0CC5);
        }

        private bool inCallBack = false;

        public override void CheckIslandMove(SurvivalTeam myTeam, SurvivalTeam theirTeam, Mobile m)
        {
            if (m.Alive && theirTeam.IslandHasPlayer(m))
            {
                theirTeam.TeamIsland.CheckTotem(myTeam, theirTeam, m);

                if (!inCallBack && Utility.RandomDouble() > (int)m.Skills.RemoveTrap.Value * 0.01)
                {
                    inCallBack = true;

                    m.Frozen = true;

                    m.SendMessage(53, "You are snared by the sticky grass!");

                    Static grass = new Static(Utility.RandomList(0x0C55, 0x0C56, 0x0C57, 0x0C58, 0x0C59, 0x0C5A, 0x0C5B))
                    {
                        Hue = Utility.RandomList(2758, 2760)
                    };

                    grass.MoveToWorld(new Point3D(m.Location.X, m.Location.Y, m.Location.Z + 3), m.Map);

                    Effects.PlaySound(m.Location, m.Map, 0x4F);

                    Timer.DelayCall(TimeSpan.FromSeconds(3), () =>
                    {
                        inCallBack = false;

                        m.Frozen = false;

                        grass.Delete();

                        Effects.PlaySound(m.Location, m.Map, 0x4F);

                        if (Utility.RandomDouble() > (int)m.Skills.Wrestling.Value * 0.01)
                        {
                            var max = (int)m.Skills.Wrestling.Value > m.Hits - 1 ? m.Hits - 1 : (int)m.Skills.Wrestling.Value;

                            m.Damage(Utility.Random(1, max));

                            m.SendMessage(53, "You were damaged by the sticky grass!");
                        }
                        else
                        {
                            m.SendMessage(53, "You freed yourself from the sticky grass!");
                        }
                    });
                }
            }
            else
            {
                SurvivalUtility.TryResPlayer(m, myTeam);
            }
        }
    }
}

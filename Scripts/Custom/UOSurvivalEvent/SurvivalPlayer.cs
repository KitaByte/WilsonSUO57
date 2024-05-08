namespace Server.Custom.UOSurvivalEvent
{
    public class SurvivalPlayer
    {
        public Mobile S_Player { get; private set; }

        public int Kills { get; private set; } = 0;

        public int Deaths { get; private set; } = 0;

        public int Points { get; private set; } = 0;

        public (Point3D oldLocation, Map map) HomeLocation { get; private set; }

        public SurvivalPlayer(Mobile m)
        {
            S_Player = m;
        }

        public void UpdateStats(bool kill, bool death, bool point)
        {
            if (kill)
            {
                Kills++;
            }

            if (death)
            {
                Deaths++;
            }

            if (point)
            {
                Points++;
            }
        }

        public void MarkHomeLocation()
        {
            HomeLocation = (S_Player.Location, S_Player.Map);
        }
    }
}

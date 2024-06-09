using Server.Mobiles;

namespace Server.Custom.RumorMill.Rumors
{
    internal class SeaHorseRumor : IRumor
    {
        private SeaHorse seaHorse;

        public Map RumorMap { get; private set; } = Map.Trammel;

        public Point3D RumorLocation { get; private set; }

        public void InitRumor()
        {
            seaHorse = new SeaHorse();

            RumorLocation = RumorUtility.GetWaterLocation(RumorMap);

            seaHorse.MoveToWorld(RumorLocation, RumorMap);
        }

        public void SpreadRumor(Mobile from, Mobile to)
        {
            if (seaHorse != null && !seaHorse.Controlled && !seaHorse.Deleted)
            {
                RumorUtility.RunMessage(this, from, to, "a sea horse");
            }
            else
            {
                RumorCore.ResetRumor(this);
            }
        }

        public void EndRumor()
        {
            if (!seaHorse.Controlled && !seaHorse.Deleted)
            {
                seaHorse.Delete();
            }

            seaHorse = null;

            RumorLocation = Point3D.Zero;
        }
    }
}

using Server.Custom.RumorMill.Items;

namespace Server.Custom.RumorMill.Rumors
{
    internal class GoldenCompassRumor : IRumor
    {
        private GoldenCompass compass;

        public Map RumorMap { get; private set; }

        public Point3D RumorLocation { get; private set; }

        public void InitRumor()
        {
            compass = new GoldenCompass();

            RumorMap = RumorUtility.GetRandomMap();

            RumorLocation = RumorUtility.GetLandLocation(RumorMap);

            compass.MoveToWorld(RumorLocation, RumorMap);
        }

        public void SpreadRumor(Mobile from, Mobile to)
        {
            if (compass != null && compass.Location == RumorLocation)
            {
                RumorUtility.RunMessage(this, from, to, "a golden compass");
            }
            else
            {
                RumorCore.ResetRumor(this);
            }
        }

        public void EndRumor()
        {
            if (compass.Location == RumorLocation)
            {
                compass.Delete();
            }

            compass = null;

            RumorLocation = Point3D.Zero;
        }
    }
}

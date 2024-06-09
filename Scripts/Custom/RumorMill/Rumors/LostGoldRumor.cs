using Server.Items;

namespace Server.Custom.RumorMill.Rumors
{
    internal class LostGoldRumor : IRumor
    {
        private Gold gold;

        public Map RumorMap { get; private set; }

        public Point3D RumorLocation { get; private set; }

        public void InitRumor()
        {
            if (Utility.RandomDouble() < 0.05)
            {
                gold = new Gold(Utility.RandomMinMax(5000, 50000));
            }
            else
            {
                gold = new Gold(Utility.RandomMinMax(50, 5000));
            }

            RumorMap = RumorUtility.GetRandomMap();

            RumorLocation = RumorUtility.GetLandLocation(RumorMap);

            gold.MoveToWorld(RumorLocation, RumorMap);
        }

        public void SpreadRumor(Mobile from, Mobile to)
        {
            if (gold != null && gold.Location == RumorLocation)
            {
                RumorUtility.RunMessage(this, from, to, "lost gold");
            }
            else
            {
                RumorCore.ResetRumor(this);
            }
        }

        public void EndRumor()
        {
            if (gold.Location == RumorLocation)
            {
                gold.Delete();
            }

            gold = null;

            RumorLocation = Point3D.Zero;
        }
    }
}

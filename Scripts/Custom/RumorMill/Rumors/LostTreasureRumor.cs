using Server.Items;

namespace Server.Custom.RumorMill.Rumors
{
    internal class LostTreasureRumor : IRumor
    {
        private Item treasure;

        public Map RumorMap { get; private set; }

        public Point3D RumorLocation { get; private set; }

        public void InitRumor()
        {
            if (Utility.RandomDouble() < 0.05)
            {
                treasure = new LargeTreasureBag();
            }
            else
            {
                treasure = new TreasureBag();
            }

            RumorMap = RumorUtility.GetRandomMap();

            RumorLocation = RumorUtility.GetAnyLocation(RumorMap);

            treasure.MoveToWorld(RumorLocation, RumorMap);
        }

        public void SpreadRumor(Mobile from, Mobile to)
        {
            if (treasure != null && treasure.Location == RumorLocation)
            {
                RumorUtility.RunMessage(this, from, to, "lost treasure");
            }
            else
            {
                RumorCore.ResetRumor(this);
            }
        }

        public void EndRumor()
        {
            if (treasure.Location == RumorLocation)
            {
                treasure.Delete();
            }

            treasure = null;

            RumorLocation = Point3D.Zero;
        }
    }
}

using Server.Mobiles;

namespace Server.Custom.RumorMill.Rumors
{
    internal class PhoenixRumor : IRumor
    {
        private Phoenix phoenix;

        public Map RumorMap { get; private set; } = Map.Felucca;

        public Point3D RumorLocation { get; private set; }

        public void InitRumor()
        {
            phoenix = new Phoenix();

            RumorLocation = RumorUtility.GetLandLocation(RumorMap);

            phoenix.MoveToWorld(RumorLocation, RumorMap);
        }

        public void SpreadRumor(Mobile from, Mobile to)
        {
            if (phoenix != null && !phoenix.Controlled && !phoenix.Deleted)
            {
                RumorUtility.RunMessage(this, from, to, "a phoenix");
            }
            else
            {
                RumorCore.ResetRumor(this);
            }
        }

        public void EndRumor()
        {
            if (!phoenix.Controlled && !phoenix.Deleted)
            {
                phoenix.Delete();
            }

            phoenix = null;

            RumorLocation = Point3D.Zero;
        }
    }
}


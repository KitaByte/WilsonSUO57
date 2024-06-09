using Server.Mobiles;

namespace Server.Custom.RumorMill.Rumors
{
    internal class UnicornRumor : IRumor
    {
        private Unicorn unicorn;

        public Map RumorMap { get; private set; } = Map.Ilshenar;

        public Point3D RumorLocation { get; private set; }

        public void InitRumor()
        {
            unicorn = new Unicorn();

            RumorLocation = RumorUtility.GetLandLocation(RumorMap);

            unicorn.MoveToWorld(RumorLocation, RumorMap);
        }

        public void SpreadRumor(Mobile from, Mobile to)
        {
            if (unicorn != null && !unicorn.Controlled && !unicorn.Deleted)
            {
                RumorUtility.RunMessage(this, from, to, "a unicorn");
            }
            else
            {
                RumorCore.ResetRumor(this);
            }
        }

        public void EndRumor()
        {
            if (!unicorn.Controlled && !unicorn.Deleted)
            {
                unicorn.Delete();
            }

            unicorn = null;

            RumorLocation = Point3D.Zero;
        }
    }
}

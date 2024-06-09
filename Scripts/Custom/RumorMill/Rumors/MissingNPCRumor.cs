using Server.Custom.RumorMill.Mobiles;

namespace Server.Custom.RumorMill.Rumors
{
    internal class MissingNPCRumor : IRumor
    {
        private MissingNPC npc;

        public Map RumorMap { get; private set; }

        public Point3D RumorLocation { get; private set; }

        public void InitRumor()
        {
            npc = new MissingNPC();

            RumorMap = RumorUtility.GetRandomMap();

            RumorLocation = RumorUtility.GetLandLocation(RumorMap);

            npc.MoveToWorld(RumorLocation, RumorMap);
        }

        public void SpreadRumor(Mobile from, Mobile to)
        {
            if (npc != null && !npc.Deleted)
            {
                RumorUtility.RunMessage(this, from, to, "a lost person");
            }
            else
            {
                RumorCore.ResetRumor(this);
            }
        }

        public void EndRumor()
        {
            if (!npc.Deleted)
            {
                npc.Delete();
            }

            npc = null;

            RumorLocation = Point3D.Zero;
        }
    }
}

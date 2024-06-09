namespace Server.Custom.RumorMill
{
    public interface IRumor
    {
        Map RumorMap { get; }

        Point3D RumorLocation { get; }

        void InitRumor();

        void SpreadRumor(Mobile from, Mobile to);

        void EndRumor();
    }
}

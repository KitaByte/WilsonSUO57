namespace Server.Custom.UOStudio
{
    public class EffectInfo
    {
        public SETypes SE_Effect { get; set; }

        public Point3D Location { get; set; }

        public EffectInfo(SETypes type, Point3D location)
        {
            SE_Effect = type;

            Location = location;
        }
    }
}

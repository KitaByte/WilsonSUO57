using Server.Items;

namespace Server.Custom.UOStudio
{
    public enum SETypes
    {
        None,
        Lightning,
        Fire,
        FireBall,
        Explosion,
        Snow,
        Sparkle,
        RedSparkle,
        Smoke,
        SmokeCloud,
        Confetti
    }

    internal static class StudioEffects
    {
        internal static void PlayEffect(EffectInfo info, Map map)
        {
            Static entity = new Static(0xEED)
            {
                Visible = false
            };

            entity.MoveToWorld(info.Location, map);

            switch (info.SE_Effect)
            {
                case SETypes.None:
                    break;
                case SETypes.Lightning: PlayLightningEffect(entity);
                    break;
                case SETypes.Fire:
                    break;
                case SETypes.FireBall:
                    break;
                case SETypes.Explosion:
                    break;
                case SETypes.Snow:
                    break;
                case SETypes.Sparkle:
                    break;
                case SETypes.RedSparkle:
                    break;
                case SETypes.Smoke:
                    break;
                case SETypes.SmokeCloud:
                    break;
                case SETypes.Confetti:
                    break;
            }

            entity.Delete();
        }

        internal static void PlayLightningEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendBoltEffect(entity, true);
            }
        }
    }
}

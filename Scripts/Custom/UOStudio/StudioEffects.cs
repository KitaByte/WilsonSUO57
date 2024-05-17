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
        Bee,
        Sparkle,
        RedSparkle,
        Smoke,
        Gate,
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
                case SETypes.Fire:      PlayFireEffect(entity);
                    break;
                case SETypes.FireBall:  PlayFireBallEffect(entity);
                    break;
                case SETypes.Explosion: PlayExplosionEffect(entity);
                    break;
                case SETypes.Bee:       PlayBeeEffect(entity);
                    break;
                case SETypes.Sparkle:   PlaySparkleEffect(entity);
                    break;
                case SETypes.RedSparkle:PlayRedSparkleEffect(entity);
                    break;
                case SETypes.Smoke:     PlaySmokeEffect(entity);
                    break;
                case SETypes.Gate:      PlayGateEffect(entity);
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

        internal static void PlayFireEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendLocationEffect(entity.Location, entity.Map, 0x3709, 30);

                Effects.PlaySound(entity.Location, entity.Map, 0x225);
            }
        }

        internal static void PlayFireBallEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendLocationEffect(entity.Location, entity.Map, 0x36FE, 70);

                Effects.PlaySound(entity.Location, entity.Map, 0x1DD);
            }
        }

        internal static void PlayExplosionEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendLocationEffect(entity.Location, entity.Map, 0x36B0, 35);

                Effects.PlaySound(entity.Location, entity.Map, 0x207);
            }
        }

        internal static void PlayBeeEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendLocationEffect(entity.Location, entity.Map, 0x091B, 30);

                Effects.PlaySound(entity.Location, entity.Map, 0x56A);
            }
        }

        internal static void PlaySparkleEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendLocationEffect(entity.Location, entity.Map, 0x373A, 65);

                Effects.PlaySound(entity.Location, entity.Map, 0x215);
            }
        }

        internal static void PlayRedSparkleEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendLocationEffect(entity.Location, entity.Map, 0x374A, 30);

                Effects.PlaySound(entity.Location, entity.Map, 0x210);
            }
        }

        internal static void PlaySmokeEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendLocationEffect(entity.Location, entity.Map, 0x3728, 15);

                Effects.PlaySound(entity.Location, entity.Map, 0x228);
            }
        }

        internal static void PlayGateEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendLocationEffect(entity.Location, entity.Map, 0x1AF3, 45);

                Effects.PlaySound(entity.Location, entity.Map, 0x20E);
            }
        }

        internal static void PlayConfettiEffect(object obj)
        {
            if (obj is IEntity entity)
            {
                Effects.SendLocationEffect(entity.Location, entity.Map, 0x9F89, 25);

                Effects.PlaySound(entity.Location, entity.Map, 0x664);
            }
        }
    }
}

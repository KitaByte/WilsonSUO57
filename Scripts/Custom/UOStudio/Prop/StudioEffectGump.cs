using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.UOStudio
{
    internal class StudioEffectGump : BaseGump
    {
        private readonly StudioFilm _Film;

        public StudioEffectGump(PlayerMobile user, StudioFilm film, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)
        {
            _Film = film;
        }

        public override void AddGumpLayout()
        {
            Closable = true;
            Resizable = false;
            Dragable = true;

            AddBackground(X, Y, 115, 335, 40000);

            AddLabel(X + 40, Y + 12, 2720, "Effects");

            AddButton(X + 20, Y + 39, 2362, 2362, 1, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 35, 1153, $"{SETypes.Lightning}");

            AddButton(X + 20, Y + 69, 2362, 2362, 2, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 65, 1153, $"{SETypes.Fire}");

            AddButton(X + 20, Y + 99, 2362, 2362, 3, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 95, 1153, $"{SETypes.FireBall}");

            AddButton(X + 20, Y + 129, 2362, 2362, 4, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 125, 1153, $"{SETypes.Explosion}");

            AddButton(X + 20, Y + 159, 2362, 2362, 5, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 155, 1153, $"{SETypes.Bee}");

            AddButton(X + 20, Y + 189, 2362, 2362, 6, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 185, 1153, $"{SETypes.Sparkle}");

            AddButton(X + 20, Y + 219, 2362, 2362, 7, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 215, 1153, $"{SETypes.RedSparkle}");

            AddButton(X + 20, Y + 249, 2362, 2362, 8, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 245, 1153, $"{SETypes.Smoke}");

            AddButton(X + 20, Y + 279, 2362, 2362, 9, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 275, 1153, $"{SETypes.Gate}");

            AddButton(X + 20, Y + 309, 2362, 2362, 10, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 305, 1153, $"{SETypes.Confetti}");
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                User.Target = new StudioEffectTarget(_Film, info.ButtonID);

                Refresh(true, false);
            }
            else
            {
                Close();
            }
        }
    }

    internal class StudioEffectTarget : Target
    {
        private readonly StudioFilm _Film;

        private readonly int _ID;

        public StudioEffectTarget(StudioFilm film, int id) : base(20, true, TargetFlags.None)
        {
            _Film = film;

            _ID = id;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            bool deleteEntity = false;

            if (targeted is LandTarget lt)
            {
                var landTemp = new Static(0xEED)
                {
                    Visible = false
                };

                landTemp.MoveToWorld(lt.Location, from.Map);

                targeted = landTemp;

                deleteEntity = true;
            }

            if (targeted is IEntity entity)
            {
                switch (_ID)
                {
                    case 1:
                        {
                            StudioEffects.PlayLightningEffect(entity);

                            _Film.AddEffect(SETypes.Lightning, entity.Location);

                            break;
                        }
                    case 2:
                        {
                            StudioEffects.PlayFireEffect(entity);

                            _Film.AddEffect(SETypes.Fire, entity.Location);

                            break;
                        }
                    case 3:
                        {
                            StudioEffects.PlayFireBallEffect(entity);

                            _Film.AddEffect(SETypes.FireBall, entity.Location);

                            break;
                        }
                    case 4:
                        {
                            StudioEffects.PlayExplosionEffect(entity);

                            _Film.AddEffect(SETypes.Explosion, entity.Location);

                            break;
                        }
                    case 5:
                        {
                            StudioEffects.PlayBeeEffect(entity);

                            _Film.AddEffect(SETypes.Bee, entity.Location);

                            break;
                        }
                    case 6:
                        {
                            StudioEffects.PlaySparkleEffect(entity);

                            _Film.AddEffect(SETypes.Sparkle, entity.Location);

                            break;
                        }
                    case 7:
                        {
                            StudioEffects.PlayRedSparkleEffect(entity);

                            _Film.AddEffect(SETypes.RedSparkle, entity.Location);

                            break;
                        }
                    case 8:
                        {
                            StudioEffects.PlaySmokeEffect(entity);

                            _Film.AddEffect(SETypes.Smoke, entity.Location);

                            break;
                        }
                    case 9:
                        {
                            StudioEffects.PlayGateEffect(entity);

                            _Film.AddEffect(SETypes.Gate, entity.Location);

                            break;
                        }
                    case 10:
                        {
                            StudioEffects.PlayConfettiEffect(entity);

                            _Film.AddEffect(SETypes.Confetti, entity.Location);

                            break;
                        }
                }

                if (deleteEntity)
                {
                    entity.Delete();
                }
            }
        }
    }
}

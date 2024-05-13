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

            AddBackground(X, Y, 110, 100, 40000);

            AddLabel(X + 35, Y + 12, 2720, "Effects");

            AddButton(X + 20, Y + 39, 2362, 2362, 1, GumpButtonType.Reply, 0);

            AddLabel(X + 40, Y + 35, 1153, "Lightning");

            //AddButton(X + 45, Y + 70, 2361, 2361, 2, GumpButtonType.Reply, 0);

            //AddLabel(X + 60, Y + 40, 1153, "HUE");

            //AddButton(X + 70, Y + 70, 2360, 2360, 3, GumpButtonType.Reply, 0);

            //AddLabel(X + 60, Y + 40, 1153, "HUE");
        }

        public override void OnResponse(RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0:
                    {
                        Close();

                        break;
                    }

                case 1:
                    {
                        User.Target = new StudioEffectTarget(_Film, info.ButtonID);

                        break;
                    }
            }

            if (info.ButtonID > 0)
            {
                Refresh(true, false);
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
                }

                if (deleteEntity)
                {
                    entity.Delete();
                }
            }
        }
    }
}

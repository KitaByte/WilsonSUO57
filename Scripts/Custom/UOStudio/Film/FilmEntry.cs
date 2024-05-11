using Server.ContextMenus;
using Server.Targeting;

namespace Server.Custom.UOStudio
{
    internal class FilmEntry : ContextMenuEntry
    {
        public FilmEntry() : base(3000481) // Goto Scene
        {
        }

        public override void OnClick()
        {
            if (Owner.From.CheckAlive())
            {
                if (Owner.Target is StudioFilm film)
                {
                    Owner.From.Target = new FilmTarget(film);
                }
            }
        }

        private class FilmTarget : Target
        {
            private readonly StudioFilm Film;

            public FilmTarget(StudioFilm film) : base(20, false, TargetFlags.None)
            {
                Film = film;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is StudioFilm film)
                {
                    Film.LinkFilm(film.Name);

                    from.SendMessage(43, $"You linked to {film.Name}");
                }
                else
                {
                    Film.LinkFilm(null);

                    from.SendMessage(43, "Linked Cleared!");
                }
            }
        }
    }
}

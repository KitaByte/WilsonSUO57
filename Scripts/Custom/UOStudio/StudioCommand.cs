using Server.Commands;
using Server.Targeting;

namespace Server.Custom.UOStudio
{
    internal class StudioCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ExportFilm", AccessLevel.Administrator, new CommandEventHandler(ExportFilm_OnCommand));
            CommandSystem.Register("ImportFilm", AccessLevel.Administrator, new CommandEventHandler(ImportFilm_OnCommand));
        }

        [Usage("ExportFilm")]
        [Description("UO Studio - Export Film")]
        private static void ExportFilm_OnCommand(CommandEventArgs e)
        {
            e.Mobile.Target = new FilmExportTarget();
        }


        [Usage("ImportFilm")]
        [Description("UO Studio - Import All Film(s) in Data/UOS_DATA/IMPORT - Folder")]
        private static void ImportFilm_OnCommand(CommandEventArgs e)
        {
            StudioUtility.ImportFilm(e.Mobile);
        }

        private class FilmExportTarget : Target
        {
            public FilmExportTarget() : base(0, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is StudioFilm film)
                {
                    StudioUtility.ExportFilm(from, film);
                }
                else
                {
                    from.SendMessage(53, "That is not studio film!");
                }
            }
        }
    }
}

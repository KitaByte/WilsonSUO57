using System;
using System.IO;
using System.Linq;

namespace Server.Custom.UOStudio
{
    internal class StudioUtility
    {
        internal static void ExportFilm(Mobile from, StudioFilm film)
        {
            if (Directory.Exists(StudioEngine.Studio_DIR))
            {
                string filePath = Path.Combine(StudioEngine.Studio_DIR, $"{film.Name}.txt");

                try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        film.Export(writer);

                        from.SendMessage(53, $"{film.Name}.txt - Created in Data/UOS_DATA Folder!");
                    }
                }
                catch (Exception ex)
                {
                    from.SendMessage(53, $"Could not export film : {ex.Message}");
                }
            }
            else
            {
                from.SendMessage(53, "Directory does not exist!");
            }
        }

        internal static void ImportFilm(Mobile from)
        {
            foreach (string filePath in Directory.GetFiles(StudioEngine.ImportPath, "*.txt"))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        StudioFilm film = new StudioFilm
                        {
                            Name = filePath.Split('\\').Last().Split('.').First()
                        };

                        film.Import(reader);

                        from.AddToBackpack(film);

                        from.SendMessage(53, $"{film.Name} - Created in {from.Name}'s backpack!");
                    }
                }
                catch (Exception ex)
                {
                    from.SendMessage(53, $"Could not import film : {ex.Message}");
                }
            }
        }
    }
}

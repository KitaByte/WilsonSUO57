using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Server.Services.UOBlackBox
{
    public static class ArtCore
    {
        private static readonly string FilePath = Path.Combine(@"Saves\UOBlackBox", $"StaticArt.bin");

        public static List<ArtEntity> AllStaticArt;

        public static List<ArtEntity> SmallArt;

        public static List<ArtEntity> MedArt;

        public static List<ArtEntity> LargeArt;

        public static List<ArtEntity> XLargeArt;

        public static void Initialize()
        {
            EventSink.ServerStarted += LoadStaticArtInfo;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            SaveStaticArtInfo();
        }

        private static int maxArt;

        private static ArtEntity newEntity;

        private static void FetchArtInfo()
        {
            if (BoxCore.IsUserAdministrator())
            {
                if (maxArt == 0)
                    maxArt = UOArtHook.GetMaxID();

                for (var i = 0; i < maxArt; i++)
                {
                    if (IsValidStatic(i, out Bitmap image))
                    {
                        newEntity = new ArtEntity("", i, image.Width, image.Height, image);

                        AllStaticArt.Add(newEntity);

                        // Log Sizes
                        if (image.Width < 76 && image.Height < 101)
                        {
                            SmallArt.Add(newEntity);
                        }
                        else if (image.Width < 151 && image.Height < 151)
                        {
                            MedArt.Add(newEntity);
                        }
                        else if (image.Width < 181 && image.Height < 351)
                        {
                            LargeArt.Add(newEntity);
                        }
                        else
                        {
                            XLargeArt.Add(newEntity);
                        }
                    }
                }
            }
            else
            {
                BoxCore.LogConsole(ConsoleColor.DarkYellow, "requires server started as administrator to set up files");
            }
        }

        private static Bitmap image;

        public static string SetName(ArtEntity entity, int id)
        {
            var name = TileData.ItemTable[id].Name;

            if (name == "" || name == "Missing_Name")
            {
                if (image != null)
                {
                    image.Dispose();

                    image = null;
                }

                if (entity.Image == null)
                {
                    image = UOArtHook.GetArtImage(id);
                }
                else
                {
                    image = entity.Image;
                }

                var pass = id == 0 || name == "Missing_Name";

                if (image?.Width == 44 && image?.Height == 44 || pass)
                {
                    UOArtHook.GetRawSize(image, out int minX, out int minY, out int maxX, out int maxY);

                    if (minX == 0 && minY == 1 && maxX == 42 && maxY == 36 || pass)
                    {
                        return "UNUSED";
                    }
                }

                return "NoName";
            }

            return BoxCore.CapitalizeWords(name);
        }

        public static int GetTotalCount()
        {
            if (AllStaticArt != null)
            {
                return AllStaticArt.Count;
            }

            return 0;
        }

        private static bool IsValidStatic(int pos, out Bitmap image)
        {
            image = UOArtHook.GetArtImage(pos);

            if (image == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void InitiateLists()
        {
            if (AllStaticArt == null)
                AllStaticArt = new List<ArtEntity>();

            if (SmallArt == null)
                SmallArt = new List<ArtEntity>();

            if (MedArt == null)
                MedArt = new List<ArtEntity>();

            if (LargeArt == null)
                LargeArt = new List<ArtEntity>();

            if (XLargeArt == null)
                XLargeArt = new List<ArtEntity>();
        }

        public static List<ArtEntity> LoadArtList(int result)
        {
            switch (result)
            {
                case 0: return SmallArt;
                case 1: return MedArt;
                case 2: return LargeArt;
                case 3: return XLargeArt;
                default: return AllStaticArt;
            }
        }

        public static void ReloadArt()
        {
            BoxCore.LogConsole(ConsoleColor.DarkCyan, "Resetting UO Black Box Art");

            ClearArtLists();
        }

        private static void ClearArtLists()
        {
            AllStaticArt?.Clear();
            SmallArt?.Clear();
            MedArt?.Clear();
            LargeArt?.Clear();
            XLargeArt?.Clear();
        }

        public static string GetListName(int buttonID)
        {
            switch (buttonID)
            {
                case 1: return "Small";
                case 2: return "Medium";
                case 3: return "Large";
                case 4: return "XLarge";
                default:return "All";
            }
        }

        public static void SaveStaticArtInfo()
        {
            Persistence.Serialize(FilePath, OnSerialize);

            if (GetTotalCount() > 0)
            {
                BoxCore.LogConsole(ConsoleColor.DarkCyan, $"Static Art Saved");
            }
            else
            {
                BoxCore.LogConsole(ConsoleColor.DarkRed, $"Static Art Wiped");
            }
        }

        private static void OnSerialize(GenericWriter writer)
        {
            writer.Write(GetTotalCount());

            SaveArtEntities(writer, AllStaticArt);

            SaveArtEntities(writer, SmallArt);

            SaveArtEntities(writer, MedArt);

            SaveArtEntities(writer, LargeArt);

            SaveArtEntities(writer, XLargeArt);
        }

        private static void SaveArtEntities(GenericWriter writer, List<ArtEntity> artEntities)
        {
            writer.Write(artEntities.Count);

            foreach (var entity in artEntities)
            {
                entity.Save(writer);
            }
        }

        public static void LoadStaticArtInfo()
        {
            InitiateLists();

            if (File.Exists(FilePath))
            {
                Persistence.Deserialize(FilePath, OnDeserialize);
            }
            else
            {
                FetchArtInfo();
            }

            if (GetTotalCount() > 0)
            {
                BoxCore.CheckVersion();

                var color = BoxCore.NeedsUpdate ? ConsoleColor.DarkRed : ConsoleColor.DarkGray;

                BoxCore.LogConsole(color, LogoDesign.Banner, false);

                BoxCore.LogConsole(ConsoleColor.DarkCyan, $"Static Art Loaded = {GetTotalCount()} Images");

                var artCount = $"{SmallArt.Count} S : {MedArt.Count} M : {LargeArt.Count} L : {XLargeArt.Count} XL";

                BoxCore.LogConsole(ConsoleColor.Cyan, $"Art Sizes = {artCount}");
            }
            else
            {
                BoxCore.LogConsole(ConsoleColor.DarkRed, "Static Art Load Failed : Restart Server as Administrator!");
            }
        }

        private static void OnDeserialize(GenericReader reader)
        {
            ClearArtLists();

            var totalCount = reader.ReadInt();

            if (totalCount > 0)
            {
                LoadArtEntities(reader, AllStaticArt);
                LoadArtEntities(reader, SmallArt);
                LoadArtEntities(reader, MedArt);
                LoadArtEntities(reader, LargeArt);
                LoadArtEntities(reader, XLargeArt);
            }
            else
            {
                FetchArtInfo();
            }
        }

        private static void LoadArtEntities(GenericReader reader, List<ArtEntity> artEntities)
        {
            var entityCount = reader.ReadInt();

            for (int i = 0; i < entityCount; i++)
            {
                var entity = new ArtEntity();

                entity.Load(reader);

                artEntities.Add(entity);
            }
        }
    }
}

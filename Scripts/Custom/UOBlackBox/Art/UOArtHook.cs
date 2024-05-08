using System.Collections.Generic;
using System.Drawing;

using Ultima;

namespace Server.Services.UOBlackBox
{
    public static class UOArtHook
    {
        public static int GetMaxID()
        {
            try
            {
                return Art.GetMaxItemID();
            }
            catch
            {
                return 0;
            }
        }

        public static Bitmap GetArtImage(int id)
        {
            try
            {
                return Art.GetStatic(id);
            }
            catch
            {
                return null;
            }
        }

        public static bool GetRawSize(Bitmap image, out int minX, out int minY, out int maxX, out int maxY)
        {
            try
            {
                Art.Measure(image, out int minXX, out int minYY, out int maxXX, out int maxYY);

                minX = minXX;
                minY = minYY;
                maxX = maxXX;
                maxY = maxYY;

                return true;
            }
            catch
            {
                minX = 0;
                minY = 0;
                maxX = 0;
                maxY = 0;

                return false;
            }
        }

        public static Dictionary<int, Bitmap> AllGumps => GetGumpArt();

        private static Dictionary<int, Bitmap> GetGumpArt()
        {
            var tempDict = new Dictionary<int, Bitmap>();

            for (int i = 0; i < 65535; i++)
            {
                tempDict.Add(i, Ultima.Gumps.GetGump(i));
            }

            return tempDict;
        }
    }
}

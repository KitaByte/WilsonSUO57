namespace Server.Services.UOBlackBox.Tools
{
    public static class MapCore
    {

        public static float MapWidth(Map map)
        {
            if (map.MapID < 5)
            {
                return 600;
            }
            else
            {
                return 187;
            }
        }

        public static float MapHeight(Map map)
        {
            switch (map.MapID)
            {
                case 0:
                    {
                        return 343;
                    }
                case 1:
                    {
                        return 343;
                    }
                case 2:
                    {
                        return 417;
                    }
                case 3:
                    {
                        return 480;
                    }
                case 4:
                    {
                        return 600;
                    }
                case 5:
                    {
                        return 600;
                    }
                default: return 0;
            }
        }
    }
}

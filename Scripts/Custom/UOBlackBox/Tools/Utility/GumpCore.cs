using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Server.Gumps;
using Server.Mobiles;

namespace Server.Services.UOBlackBox.Tools
{
    public static class GumpCore
    {
        private static readonly string FilePath = Path.Combine(@"Saves\UOBlackBox", $"GumpAssets.bin");

        private const double CharOffsetL = 9.5;
        private const double CharOffsetM = 7.5;
        private const double CharOffsetS = 4.5;

        public const int MainBG = 39925;
        public const int SubBG = 40000;
        public const int BlackBG = 2620;
        public const int SBlackBG = 1755;

        public const int GoldText = 2720;
        public const int WhtText = 2499;

        // 16x16
        public const int PrevBtnUP = 5603;
        public const int PrevBtnDown = 5607;
        public const int NextBtnUP = 5601;
        public const int NextBtnDown = 5605;

        public const int MinBtn = 5401;
        public const int MaxBtn = 5402;

        // 22x22
        public const int CloseBtn = 40015;

        // 27x27
        public const int InfoBtn = 40014;

        // 11x11
        public const int RndBtnUp = 10006;
        public const int RndBtnDown = 2104;

        // 23x23
        public const int AddBtn = 1606;
        public const int MultiBtn = 1607;
        public const int TileBtn = 1608;

        // 11x11
        public const int RedUp = 2360;
        public const int RedDown = 5032;

        public const int GreenUp = 2361;
        public const int GreenDown = 5032;

        public const int BlueUp = 2362;
        public const int BlueDown = 5032;

        // 44x41
        public const int HueItem = 4033;

        // 50x50
        public const int ArrowUp = 4500;
        public const int ArrowNorth = 4501;
        public const int ArrowRight = 4502;
        public const int ArrowEast = 4503;
        public const int ArrowDown = 4504;
        public const int ArrowSouth = 4505;
        public const int ArrowLeft = 4506;
        public const int ArrowWest = 4507;

        // 16x16
        public const int ArrowRaise = 5600;
        public const int ArrowLower = 5602;

        // 15x15
        public const int Tool1 = 1653;
        public const int Tool2 = 1654;
        public const int Tool3 = 1655;

        public const int Tool4 = 1656;
        public const int Tool5 = 1657;
        public const int Tool6 = 1658;

        public const int Tool7 = 1659;
        public const int Tool8 = 1660;
        public const int Tool9 = 1661;

        public const int ToolDown = 1652;

        // 600*Max - (w/h)
        public const int MapFelucca = 65000;
        public const int MapTrammel = 65001;
        public const int MapIlshenar = 65002;
        public const int MapMalas = 65003;
        public const int MapTokuno = 65004;
        public const int MapTerMur = 65005;

        public const int MapFrame = 5599;

        // 7x7
        public const int SmBtnUp = 1672;
        public const int SmBtnDown = 1686;

        // 13x21
        public const int PinArt = 5020;

        // 5x9
        public const int SmPinArt = 9010;

        // 32x32
        public const int PlayerIcon = 30564;
        public const int StaffIcon = 30565;

        // 10x10
        public const int MeIcon = 9754;

        public static void Initialize()
        {
            EventSink.ServerStarted += StartGumpAssets;
        }

        private static void StartGumpAssets()
        {
            LoadGumpAssets();
        }

        public static int GetCentered(int lineWidth, string text, bool isStart)
        {
            double textWidth = 0.0;

            foreach (var letter in text)
            {
                if (letter.ToString() == letter.ToString().ToLower())
                {
                    if (letter == ' ')
                    {
                        textWidth += CharOffsetL;
                    }
                    else if (letter == 'w' || letter == 'm')
                    {
                        textWidth += CharOffsetM;
                    }
                    else
                    {
                        textWidth += CharOffsetS;
                    }
                }
                else
                {
                    if (letter == 'W' || letter == 'M')
                    {
                        textWidth += CharOffsetL;
                    }
                    else
                    {
                        textWidth += CharOffsetM;
                    }
                }
            }

            var start = (lineWidth / 2) - (textWidth / 2);

            var end = start + textWidth;

            if (isStart)
            {
                return Convert.ToInt32(start);
            }
            else
            {
                return Convert.ToInt32(end);
            }
        }

        // Close Button
        public static void SetGumpClose(BaseGump gump, int width)
        {
            gump.AddButton(gump.X + width - 19, gump.Y - 3, CloseBtn, CloseBtn, 0, GumpButtonType.Reply, 0);
        }

        // Info Button
        public static void SetGumpInfo(BaseGump gump)
        {
            gump.AddButton(gump.X - 4, gump.Y - 3, InfoBtn, InfoBtn, 123456, GumpButtonType.Reply, 0);

            gump.AddLabel(gump.X + 3, gump.Y - 1, GoldText, "?");
        }

        // Info Gump
        public static void SendInfoGump(BoxSession session, BaseGump gump)
        {
            if (gump.Children?.Count > 0)
            {
                var hasToolInfo = gump.Children.Find(g => g is ToolInfo);

                if (hasToolInfo != null)
                {
                    return;
                }
            }

            if (gump is IToolInfo info)
            {
                BaseGump.SendGump(new ToolInfo(session, info, gump.X, gump.Y, gump));
            }
        }

        // Map  Default Locations
        public static List<(Map BMap, Point3D BLoc)> BoxLocations { get; private set; }

        public static bool AddBoxLocation(Map map, Point3D location)
        {
            if (!BoxLocations.Contains((map, location)))
            {
                BoxLocations.Add((map, location));

                SaveGumpAssets();

                return true;
            }

            return false;
        }

        internal static bool RemoveBoxLocation(Map map, Point3D location)
        {
            if (BoxLocations.Contains((map, location)))
            {
                BoxLocations.Remove((map, location));

                SaveGumpAssets();

                return true;
            }

            return false;
        }

        // Map  Default Locations
        public static List<(PlayerMobile Staff, List<(Map BMap, Point3D BLoc)> SLoc)> StaffLocations { get; private set; }

        public static bool AddStaffLocation(PlayerMobile staff, Map map, Point3D location)
        {
            var getStaff = StaffLocations.Find(s => s.Staff == staff);

            if (getStaff.Staff == null)
            {
                getStaff = (staff, new List<(Map BMap, Point3D BLoc)>());

                StaffLocations.Add(getStaff);
            }

            if (!getStaff.SLoc.Contains((map, location)))
            {
                getStaff.SLoc.Add((map, location));

                SaveGumpAssets();

                return true;
            }

            return false;
        }

        internal static bool RemoveStaffLocation(PlayerMobile staff, Map map, Point3D location)
        {
            var (Staff, SLoc) = StaffLocations.Find(s => s.Staff == staff);

            if (Staff != null && SLoc.Contains((map, location)))
            {
                SLoc.Remove((map, location));

                SaveGumpAssets();

                return true;
            }

            return false;
        }

        // Save
        private static void SaveGumpAssets()
        {
            Persistence.Serialize(FilePath, OnSerialize);
        }

        private static void OnSerialize(GenericWriter writer)
        {
            writer.Write(BoxLocations.Count);

            if (BoxLocations.Count > 0)
            {
                foreach (var (BMap, BLoc) in BoxLocations)
                {
                    writer.Write(BMap.Name);
                    writer.Write(BLoc);
                }
            }

            writer.Write(StaffLocations.Count);

            if (StaffLocations.Count > 0)
            {
                foreach (var (Staff, SLoc) in StaffLocations)
                {
                    writer.Write(Staff);

                    writer.Write(SLoc.Count());

                    if (SLoc.Count() > 0)
                    {
                        foreach (var (BMap, BLoc) in SLoc)
                        {
                            writer.Write(BMap.Name);
                            writer.Write(BLoc);
                        }
                    }
                }
            }
        }

        // Load
        private static void LoadGumpAssets()
        {
            Persistence.Deserialize(FilePath, OnDeserialize);
        }

        private static void OnDeserialize(GenericReader reader)
        {
            BoxLocations = new List<(Map BMap, Point3D BLoc)>();

            try
            {
                var count = reader.ReadInt();

                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var location = (Map.Parse(reader.ReadString()), reader.ReadPoint3D());

                        BoxLocations.Add(location);
                    }
                }

                BoxCore.LogConsole(ConsoleColor.DarkCyan, $"Default Locations {BoxLocations.Count} - loaded");
            }
            catch
            {
                BoxCore.LogConsole(ConsoleColor.DarkCyan, $"Default Locations - Initialized");
            }

            StaffLocations = new List<(PlayerMobile Staff, List<(Map BMap, Point3D BLoc)> SLoc)>();

            try
            {
                var countStaff = reader.ReadInt();

                for (int j = 0; j < countStaff; j++)
                {
                    var staff = reader.ReadMobile() as PlayerMobile;

                    var countLoc = reader.ReadInt();

                    List<(Map, Point3D)> sLoc = new List<(Map BMap, Point3D BLoc)>();

                    for (int k = 0; k < countLoc; k++)
                    {
                        sLoc.Add((Map.Parse(reader.ReadString()), reader.ReadPoint3D()));
                    }

                    StaffLocations.Add((staff, sLoc));
                }

                BoxCore.LogConsole(ConsoleColor.DarkCyan, $"Staff Locations {StaffLocations.Count} - loaded");
            }
            catch
            {
                BoxCore.LogConsole(ConsoleColor.DarkCyan, $"Staff Locations - Initialized");
            }
        }
    }
}

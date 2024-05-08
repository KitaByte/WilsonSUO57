using System;
using System.IO;
using System.Linq;
using System.Text;
using Server.Mobiles;
using System.Collections;
using System.Collections.Generic;

namespace Server.Custom.UOBuilder
{
    internal static class UOBuilderCore
    {
        internal static bool UOBuilderActive { get; set; } = true;

        internal const int MaxBuilds = 10;

        internal static int ItemID_Max { get; private set; } = 44918;

        private static readonly string BuilderFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "UOBuilderData.CSV");

        private static readonly string BuilderArtFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "UOBuilderArtInfo.CSV");

        private static readonly Dictionary<Serial, List<UOBuilderEntity>> UOBuildsList = new Dictionary<Serial, List<UOBuilderEntity>>();

        internal static readonly ArrayList ItemWidths = new ArrayList();

        internal static bool HasBuilds()
        {
            return UOBuildsList.Count > 0;
        }

        internal static List<Serial> GetBuildList()
        {
            List<Serial> keys = new List<Serial>();

            foreach (var key in UOBuildsList.Keys)
            {
                keys.Add(key);
            }

            return keys;
        }

        internal static bool CanBuild(UOBuilderPermit permit)
        {
            if (UOBuildsList.ContainsKey(permit.BlessedFor.Serial))
            {
                return true;
            }
            else
            {
                if (UOBuildsList.Count >= MaxBuilds)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        internal static void AddStatic(UOBuilderPermit permit, UOBuilderStatic buildStatic)
        {
            UOBuilderEntity entity = new UOBuilderEntity()
            {
                E_ID = buildStatic.ItemID,
                E_Map = buildStatic.Map.ToString(),
                E_X = buildStatic.X,
                E_Y = buildStatic.Y,
                E_Z = buildStatic.Z,
                E_HUE = buildStatic.HUE
            };

            if (!UOBuildsList.ContainsKey(permit.BlessedFor.Serial))
            {
                UOBuildsList.Add(permit.BlessedFor.Serial, new List<UOBuilderEntity>() { entity });
            }
            else
            {
                UOBuildsList[permit.BlessedFor.Serial].Add(entity);
            }

            if (UOBuildsList[permit.BlessedFor.Serial].Count == 1)
            {
                permit.Center = buildStatic.Location;
            }
        }

        internal static void RemoveStatic(UOBuilderPermit permit, UOBuilderStatic buildStatic)
        {
            if (UOBuildsList.ContainsKey(permit.BlessedFor.Serial))
            {
                int entityToRemove = -1;

                foreach (var entity in UOBuildsList[permit.BlessedFor.Serial])
                {
                    if (entity is UOBuilderEntity uobe)
                    {
                        if (uobe.E_ID == buildStatic.ItemID && uobe.E_X == buildStatic.X && uobe.E_Y == buildStatic.Y && uobe.E_Z == buildStatic.Z)
                        {
                            if (uobe.E_Map == buildStatic.Map.ToString())
                            {
                                entityToRemove = UOBuildsList[permit.BlessedFor.Serial].IndexOf(uobe);

                                break;
                            }
                        }
                    }
                }

                if (entityToRemove > -1)
                {
                    UOBuildsList[permit.BlessedFor.Serial].RemoveAt(entityToRemove);
                }
            }
        }

        internal static void PlaceBuild(UOBuilderPermit permit)
        {
            if (permit.Center == Point3D.Zero && UOBuildsList.ContainsKey(permit.BlessedFor.Serial))
            {
                foreach (var entity in UOBuildsList[permit.BlessedFor.Serial])
                {
                    UOBuilderStatic uobs = new UOBuilderStatic();

                    uobs.AddPermit(permit, false);

                    uobs.UpdateStats(entity);

                    if (UOBuildsList[permit.BlessedFor.Serial].Count == 1)
                    {
                        permit.Center = uobs.Location;
                    }
                }
            }
        }

        internal static Serial LastSerialPlaced = 0;

        internal static void PlaceBuild(PlayerMobile staff, Serial serial)
        {
            if (UOBuildsList.ContainsKey(serial) && serial != LastSerialPlaced)
            {
                LastSerialPlaced = serial;

                bool isMoved = false;

                foreach (var entity in UOBuildsList[serial])
                {
                    UOBuilderStatic uobs = new UOBuilderStatic();

                    uobs.AddStaff(staff);

                    uobs.UpdateStats(entity);

                    if (!isMoved)
                    {
                        isMoved = true;

                        staff.MoveToWorld(uobs.Location, uobs.Map);
                    }
                }
            }
            else
            {
                staff.SendMessage(53, "Either the build has already been placed or the serial is missing!");
            }
        }

        internal static void CleanBuild(Mobile from)
        {
            var cleanupList = World.Items.Values.Where(i => i is UOBuilderStatic && i.BlessedFor == from)?.ToList();

            if (cleanupList != null && cleanupList.Count > 0)
            {
                for (int i = cleanupList.Count - 1; i >= 0; i--)
                {
                    cleanupList[i].Delete();
                }
            }

            if (from.AccessLevel > AccessLevel.Player)
            {
                LastSerialPlaced = 0;
            }
        }

        internal static void RemoveBuild(Serial serial)
        {
            if (UOBuildsList.ContainsKey(serial))
            {
                UOBuildsList.Remove(serial);
            }
        }

        public static void Initialize()
        {
            EventSink.ServerStarted += EventSink_ServerStarted;

            EventSink.Logout += EventSink_Logout;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;
        }

        private static void EventSink_ServerStarted()
        {
            var cleanupList = World.Items.Values.Where(i => i is UOBuilderStatic)?.ToList();

            if (cleanupList != null && cleanupList.Count > 0)
            {
                for (int i = cleanupList.Count - 1; i >= 0; i--)
                {
                    cleanupList[i].Delete();
                }
            }

            LoadUOBuilder();
        }

        private static void EventSink_Logout(LogoutEventArgs e)
        {
            if (e.Mobile is PlayerMobile)
            {
                CleanBuild(e.Mobile);
            }
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(UOBuilderActive.ToString());

            if (UOBuildsList.Count > 0)
            {
                foreach (var user in UOBuildsList.Keys)
                {
                    sb.AppendLine(user.ToString());

                    if (UOBuildsList[user].Count > 0)
                    {
                        sb.AppendLine(UOBuildsList[user].Count.ToString());

                        foreach (var item in UOBuildsList[user])
                        {
                            if (item is UOBuilderEntity entity)
                            {
                                entity.SaveEntity(sb);
                            }
                        }
                    }
                    else
                    {
                        sb.AppendLine("Empty");
                    }
                }

                File.WriteAllText(BuilderFile, sb.ToString());
            }
        }

        private static void LoadUOBuilder()
        {
            if (File.Exists(BuilderFile))
            {
                using (StreamReader reader = new StreamReader(BuilderFile))
                {
                    string line;

                    Serial currentUserSerial = Serial.Zero;

                    int itemCount = 0;

                    UOBuilderActive = bool.Parse(reader.ReadLine());

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("0x"))
                        {
                            currentUserSerial = (int)(Convert.ToInt32(line, 16) & 0xFFFFFFFF);

                            if (!UOBuildsList.ContainsKey(currentUserSerial))
                            {
                                UOBuildsList[currentUserSerial] = new List<UOBuilderEntity>();
                            }
                        }
                        else if (int.TryParse(line, out itemCount))
                        {
                            if (itemCount > 0 && currentUserSerial != Serial.Zero)
                            {
                                for (int i = 0; i < itemCount; i++)
                                {
                                    UOBuilderEntity entity = new UOBuilderEntity();

                                    entity.LoadEntity(reader);

                                    UOBuildsList[currentUserSerial].Add(entity);
                                }
                            }
                        }
                    }
                }

                if (File.Exists(BuilderArtFile))
                {
                    var lines = File.ReadAllLines(BuilderArtFile);

                    if (lines.Length > 0)
                    {
                        ItemWidths.AddRange(lines);
                    }
                }
            }
        }
    }
}

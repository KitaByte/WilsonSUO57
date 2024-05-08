using System;
using System.IO;
using System.Linq;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Custom.SpawnSystem
{
    internal static class SpawnSysCore
    {
        private static readonly string STAT_DIR = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "UOR_Stats");

        private static SpawnSysTimer _SpawnTimer;

        internal static Dictionary<PlayerMobile, Queue<(string mob, Point3D loc)>> _SpawnQueue;

        internal static List<PlayerMobile> _Players;

        private static readonly object _lockObject = new object();

        private static readonly int spawn_Marker = SpawnSysSettings.MARKER;

        private static readonly int spawn_MinQueued = SpawnSysSettings.MIN_QUE;

        private static List<Mobile> _SpawnedList;

        private static List<Mobile> _CleanUpList;

        internal static bool HasChanged { get; set; } = false;

        private static void UpdateSpawnedList(Mobile m)
        {
            if (_SpawnedList.Contains(m))
            {
                _SpawnedList.Remove(m);
            }
            else if (_CleanUpList.Contains(m))
            {
                _CleanUpList.Remove(m);
            }
        }

        internal static void EnqueueSpawn(PlayerMobile pm, string mob, Point3D loc)
        {
            lock (_lockObject)
            {
                if (_SpawnQueue.ContainsKey(pm))
                {
                    _SpawnQueue[pm].Enqueue((mob, loc));
                }
            }
        }

        private static (string mob, Point3D loc) DequeueSpawn(PlayerMobile pm)
        {
            lock (_lockObject)
            {
                if (_SpawnQueue.ContainsKey(pm) && _SpawnQueue[pm].Count > 0)
                {
                    (string mob, Point3D loc) spawnInfo = (string.Empty, SpawnSysUtility.Default_Point);

                    bool tooFar = true;

                    do
                    {
                        if (_SpawnQueue[pm].Count > 0)
                        {
                            spawnInfo = _SpawnQueue[pm].Dequeue();

                            tooFar = IsTooFar(pm, spawnInfo.loc);
                        }
                        else
                        {
                            tooFar = false;

                            spawnInfo = (string.Empty, SpawnSysUtility.Default_Point);
                        }
                    }
                    while (tooFar);

                    return spawnInfo;
                }
                else
                {
                    return (string.Empty, SpawnSysUtility.Default_Point);
                }
            }
        }

        public static void Initialize()
        {
            SpawnSysUtility.SendConsoleMsg(ConsoleColor.DarkYellow, $"UORespawn: Startup Time => {DateTime.Now.ToShortTimeString()}");

            LoadLogo();

            SpawnSysDataBase.LoadSpawns();

            InitializeLists();

            SubscribeEvents();

            StartTimer();

            SpawnSysUtility.SendConsoleMsg(ConsoleColor.Green, $"UORespawn: Ready to Spawn...");
        }

        private static void LoadLogo()
        {
            SpawnSysUtility.SendConsoleMsg(ConsoleColor.DarkCyan, "*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*");
            SpawnSysUtility.SendConsoleMsg(ConsoleColor.Cyan, "|-|-|-|-|-|-|-| UORespawn |-|-|-|-|-|-|-|");
            SpawnSysUtility.SendConsoleMsg(ConsoleColor.Cyan, "|-|-|-|-|-|-|-|   ~*~*~   |-|-|-|-|-|-|-|");
            SpawnSysUtility.SendConsoleMsg(ConsoleColor.DarkCyan, "*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*");
        }

        private static void InitializeLists()
        {
            _SpawnQueue = new Dictionary<PlayerMobile, Queue<(string mob, Point3D loc)>>();

            _Players = new List<PlayerMobile>();

            _SpawnedList = new List<Mobile>();

            _CleanUpList = new List<Mobile>();

            SpawnSysUtility.SendConsoleMsg(ConsoleColor.Yellow, "UORespawn: Lists Initialized...}");
        }

        private static void SubscribeEvents()
        {
            EventSink.ServerStarted += EventSink_ServerStarted;

            EventSink.TameCreature += EventSink_TameCreature;

            EventSink.CreatureDeath += EventSink_CreatureDeath;

            EventSink.MobileDeleted += EventSink_MobileDeleted;

            EventSink.BeforeWorldSave += EventSink_BeforeWorldSave;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;

            EventSink.Login += EventSink_Login;

            EventSink.Logout += EventSink_Logout;

            EventSink.Shutdown += EventSink_Shutdown;

            EventSink.Crashed += EventSink_Crashed;

            SpawnSysUtility.SendConsoleMsg(ConsoleColor.Yellow, "UORespawn: Events Subscribed...}");
        }

        private static void StartTimer()
        {
            _SpawnTimer = new SpawnSysTimer();

            _SpawnTimer.Start();

            SpawnSysUtility.SendConsoleMsg(ConsoleColor.Yellow, "UORespawn: Timer Started...}");
        }

        private static void EventSink_ServerStarted()
        {
            var mobList = World.Mobiles.Values.Where(m => m.Deaths == spawn_Marker).ToList();

            int cleaned = 0;

            int mobTotal = mobList.Count;

            while (mobTotal > 0)
            {
                mobList.Last().Delete();

                mobList.Remove(mobList.Last());

                mobTotal = mobList.Count;

                cleaned++;
            }

            if (cleaned > 0)
            {
                SpawnSysUtility.SendConsoleMsg(ConsoleColor.DarkYellow, $"UORespawn: Cleaned {cleaned} mobiles!");
            }
        }

        private static void EventSink_TameCreature(TameCreatureEventArgs e)
        {
            if (e.Creature is BaseCreature bc && bc.Controlled)
            {
                UpdateSpawnedList(e.Creature);
            }
        }

        private static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
        {
            UpdateSpawnedList(e.Creature);
        }

        private static void EventSink_MobileDeleted(MobileDeletedEventArgs e)
        {
            UpdateSpawnedList(e.Mobile);
        }

        private static void EventSink_BeforeWorldSave(BeforeWorldSaveEventArgs e)
        {
            if (_SpawnTimer.Running)
            {
                _SpawnTimer.Stop();
            }
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            ClearSpawnSystem();

            _SpawnTimer.Start();

            try
            {
                if (!Directory.Exists(STAT_DIR))
                {
                    Directory.CreateDirectory(STAT_DIR);
                }

                foreach (var spawn in SpawnSysFactory.SpawnStats)
                {
                    string converted = $"{spawn.Item1.ToShortTimeString()}|{spawn.Item2.Name}|{spawn.Item3}|{spawn.Item4.X}|{spawn.Item4.Y}|{spawn.Item5.X}|{spawn.Item5.Y}";

                    File.AppendAllText(Path.Combine(STAT_DIR, $"{DateTime.Now.Year}_{DateTime.Now.DayOfYear}.txt"), converted + Environment.NewLine);
                }

                SpawnSysFactory.SpawnStats.Clear();

                SpawnSysUtility.CleanUpStatFiles(STAT_DIR);
            }
            catch(Exception ex)
            {
                SpawnSysUtility.SendConsoleMsg(ConsoleColor.DarkRed, $"UORspawn: Stat File Error - {ex.Message}");
            }
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm)
            {
                if (!_SpawnQueue.ContainsKey(pm))
                {
                    _SpawnQueue.Add(pm, new Queue<(string mob, Point3D loc)>());

                    _Players.Add(pm);

                    HasChanged = true;
                }
            }
        }

        private static void EventSink_Logout(LogoutEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm)
            {
                if (_SpawnQueue.Count > 0 && _SpawnQueue.ContainsKey(pm))
                {
                    _SpawnQueue.Remove(pm);
                }

                if (_Players.Count > 0 && _Players.Contains(pm))
                {
                    _Players.Remove(pm);

                    HasChanged = true;
                }
            }

            ClearSpawnSystem();
        }

        private static void EventSink_Shutdown(ShutdownEventArgs e)
        {
            ClearSpawnSystem();
        }

        private static void EventSink_Crashed(CrashedEventArgs e)
        {
            ClearSpawnSystem();
        }

        //Override Method : Dev Only
        internal static void UpdateWorldSpawn()
        {
            if (_SpawnQueue.Count > 0)
            {
                foreach (PlayerMobile pm in _SpawnQueue.Keys)
                {
                    UpdatePlayerSpawn(pm, pm.Map, pm.Location, pm == _SpawnQueue.Keys.Last());
                }
            }
        }

        internal static void UpdatePlayerSpawn(PlayerMobile pm, Map map, Point3D location, bool isLast)
        {
            try
            {
                if (_CleanUpList.Count > (SpawnSysUtility.Max_Mobs * _SpawnQueue.Count) && isLast)
                {
                    RunSpawnCleanUp();
                }

                if (IsValidPlayer(pm))
                {
                    if (SpawnSysSettings.SCALE_SPAWN)
                    {
                        var players = pm.GetClientsInRange(SpawnSysSettings.MAX_RANGE);

                        int playerCount = players.Count();

                        players.Free();

                        if (playerCount > 0)
                        {
                            SpawnSysSettings.UpdateStats(0.1 * playerCount);
                        }
                    }

                    if (_SpawnQueue[pm].Count > spawn_MinQueued)
                    {
                        var spawnInfo = DequeueSpawn(pm);

                        if (!string.IsNullOrEmpty(spawnInfo.mob))
                        {
                            Mobile mob = SpawnSysUtility.GetSpawn(ref _CleanUpList, spawnInfo.mob);

                            if (mob != null)
                            {
                                mob.Deaths = spawn_Marker;

                                mob.OnBeforeSpawn(spawnInfo.loc, map);

                                mob.MoveToWorld(spawnInfo.loc, map);

                                Effects.SendLocationEffect(mob.Location, mob.Map, 0x375A, 15, 0, 0);

                                mob.OnAfterSpawn();

                                if (!_SpawnedList.Contains(mob))
                                {
                                    _SpawnedList.Add(mob);
                                }

                                if ((map == Map.Trammel || map == Map.Felucca) && !mob.CanSwim)
                                {
                                    if (map == Map.Felucca)
                                    {
                                        if (!pm.Criminal && !pm.Murderer)
                                        {
                                            mob.Combatant = pm;
                                        }
                                    }
                                    else
                                    {
                                        if (pm.Criminal || pm.Murderer)
                                        {
                                            mob.Combatant = pm;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (isLast)
                    {
                        GetSpawnCleanUp();
                    }
                }

                if (map.Width > location.X && map.Height > location.Y)
                {
                    _ = SpawnSysUtility.LoadSpawn(pm, map, location);
                }
            }
            catch (Exception ex)
            {
                SpawnSysUtility.SendConsoleMsg(ConsoleColor.DarkRed, $"UORespawn - Update Error: {ex.Message}");
            }
        }

        private static void GetSpawnCleanUp()
        {
            foreach (var mobile in _SpawnedList)
            {
                if (mobile.Alive)
                {
                    if (IsTooFar(mobile) && !_CleanUpList.Contains(mobile))
                    {
                        _CleanUpList.Add(mobile);
                    }
                }
            }
        }

        private static bool IsTooFar(Mobile mobile)
        {
            foreach (PlayerMobile pm in _SpawnQueue.Keys)
            {
                if (IsValidPlayer(pm) && mobile.InRange(pm, (int)(SpawnSysUtility.Max_Range * 1.5)))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsTooFar(PlayerMobile pm, Point3D location)
        {
            if (IsValidPlayer(pm) && pm.InRange(location, (int)(SpawnSysUtility.Max_Range * 1.5)))
            {
                return false;
            }

            return true;
        }

        private static void RunSpawnCleanUp()
        {
            if (_CleanUpList.Count > 0)
            {
                for (int i = _CleanUpList.Count - 1; i >= 0; i--)
                {
                    if (IsTooFar(_CleanUpList[i]))
                    {
                        if (_SpawnedList.Contains(_CleanUpList[i]))
                        {
                            _SpawnedList.Remove(_CleanUpList[i]);
                        }

                        if (_CleanUpList[i] is BaseCreature bc && (bc.Controlled || bc.IsStabled))
                        {
                            bc.Deaths = 0;
                        }
                        else
                        { 
                            if (_CleanUpList[i].Alive && _CleanUpList[i].Map != Map.Internal)
                            {
                                _CleanUpList[i].Delete();
                            }
                            else
                            {
                                _CleanUpList[i].Deaths = 0;
                            }
                        }
                    }
                }

                _CleanUpList.Clear();
            }
        }

        private static bool IsValidPlayer(PlayerMobile pm)
        {
            return pm.Map != Map.Internal;
        }

        private static void ClearSpawnSystem()
        {
            GetSpawnCleanUp();

            RunSpawnCleanUp();
        }
    }
}

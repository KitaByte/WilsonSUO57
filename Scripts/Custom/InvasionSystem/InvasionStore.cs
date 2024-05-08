using System.Linq;
using System.Collections.Generic;

namespace Server.Custom.InvasionSystem
{
    internal static class InvasionStore
    {
        private static Dictionary<Map, List<(string name, int count)>> MapStores;

        private static InvasionLedger _Ledger = null;

        internal static bool IsLedgerInitialized = false;

        public static void Initialize()
        {
            MapStores = new Dictionary<Map, List<(string, int)>>();

            foreach (var map in Map.Maps)
            {
                if (map != null && map != Map.Internal)
                {
                    MapStores.Add(map, new List<(string, int)>());
                }
            }

            EventSink.Crashed += EventSink_Crashed;

            EventSink.Shutdown += EventSink_Shutdown;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            SaveToLedger();
        }

        private static void EventSink_Shutdown(ShutdownEventArgs e)
        {
            SaveToLedger();

            InvasionEngine.StopInvasionTimer();
        }

        private static void EventSink_Crashed(CrashedEventArgs e)
        {
            SaveToLedger();

            InvasionEngine.StopInvasionTimer();
        }

        internal static void InitializeLedger()
        {
            if (!IsLedgerInitialized)
            {
                IsLedgerInitialized = true;

                if (!World.Items.Values.Any(i => i is InvasionLedger))
                {
                    _Ledger = new InvasionLedger();

                    _Ledger.AddLedgerToWorld();
                }
                else
                {
                    var foundLedger = World.Items.Values.Where(l => l is InvasionLedger).ToList().First();

                    if (foundLedger is InvasionLedger fl)
                    {
                        _Ledger = fl;

                        var tempMap = Map.Felucca;

                        int counter = 0;

                        while (tempMap != Map.Internal)
                        {
                            var data = fl.GetData(counter);

                            tempMap = data.map;

                            if (tempMap != Map.Internal)
                            {
                                AddMobToMap(data.map, data.name);
                            }

                            counter++;
                        }
                    }
                }
            }
        }

        private static void SaveToLedger()
        {
            if (_Ledger != null)
            {
                _Ledger.ResetData();

                foreach (var list in MapStores)
                {
                    foreach (var data in list.Value)
                    {
                        _Ledger.AddData(list.Key, data.name);
                    }
                }
            }
        }

        internal static bool GetMapList(Map map, out List<(string, int)> list)
        {
            if (MapStores != null && MapStores.TryGetValue(map, out List<(string, int)> mobList))
            {
                list = mobList;

                return true;
            }

            list = new List<(string, int)>();

            return false;
        }

        internal static bool AddMobToMap(Map map, string name)
        {
            if (MapStores != null && MapStores.TryGetValue(map, out List<(string name, int count)> mobList))
            {
                if (mobList.Any(i => i.name == name))
                {
                    var info = mobList.Find(i => i.name == name);

                    var pos = mobList.IndexOf(info);

                    if (mobList[pos].count >= InvasionSettings.UNREST_LIMIT)
                    {
                        if (InvasionSettings.InvasionSysDEBUG)
                        {
                            World.Broadcast(53, false, $"Invasion System - {mobList[pos].name} x {mobList[pos].count}");
                        }

                        return false;
                    }
                    else
                    {
                        if (InvasionSettings.InvasionSysDEBUG)
                        {
                            World.Broadcast(53, false, $"Invasion System - {mobList[pos].name} x {mobList[pos].count}");
                        }

                        mobList[pos] = (name, info.count + 1);

                        return true;
                    }
                }

                mobList.Add((name, 1));

                if (InvasionSettings.InvasionSysDEBUG)
                {
                    World.Broadcast(53, false, $"Invasion System - {name} x 1");
                }

                return true;
            }

            return false;
        }
    }
}

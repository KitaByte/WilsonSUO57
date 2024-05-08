using System.Collections.Generic;
using System.IO;

namespace Server.Services.UOBlackBox
{
    public static class GameInfo
    {
        private static readonly string FilePath = Path.Combine(@"Saves\UOBlackBox", $"GameData.bin");

        private const int MaxDataStored = 25000;

        public static void Initialize()
        {
            if (CraftData == null)
            {
                InitiateData();
            }

            EventSink.CraftSuccess += EventSink_CraftSuccess;

            EventSink.CreatureDeath += EventSink_CreatureDeath;

            EventSink.PlayerDeath += EventSink_PlayerDeath;

            EventSink.QuestComplete += EventSink_QuestComplete;

            EventSink.ResourceHarvestSuccess += EventSink_ResourceHarvestSuccess;

            EventSink.TameCreature += EventSink_TameCreature;

            EventSink.ValidVendorPurchase += EventSink_ValidVendorPurchase;

            EventSink.ValidVendorSell += EventSink_ValidVendorSell;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;

            EventSink.ServerStarted += EventSink_ServerStarted;
        }

        public static void InitiateData()
        {
            CraftData = new List<(Map HeatMap, Point3D Location)>();
            CDeathData = new List<(Map HeatMap, Point3D Location)>();
            PDeathData = new List<(Map HeatMap, Point3D Location)>();
            QuestData = new List<(Map HeatMap, Point3D Location)>();
            HarvestData = new List<(Map HeatMap, Point3D Location)>();
            TameData = new List<(Map HeatMap, Point3D Location)>();
            BuyData = new List<(Map HeatMap, Point3D Location)>();
            SellData = new List<(Map HeatMap, Point3D Location)>();
        }

        public static void ClearAllData()
        {
            CraftData?.Clear();
            CDeathData?.Clear();
            PDeathData?.Clear();
            QuestData?.Clear();
            HarvestData?.Clear();
            TameData?.Clear();
            BuyData?.Clear();
            SellData?.Clear();
        }

        public static List<(Map HeatMap, Point3D Location)> CraftData { get; private set; }

        private static void EventSink_CraftSuccess(CraftSuccessEventArgs e)
        {
            if (CraftData == null)
            {
                CraftData = new List<(Map HeatMap, Point3D Location)> ();
            }

            while (CraftData.Count > MaxDataStored)
            {
                CraftData.RemoveAt(0);
            }

            if (!CraftData.Contains((e.Crafter.Map, e.Crafter.Location)))
            {
                CraftData.Add((e.Crafter.Map, e.Crafter.Location));
            }
        }

        public static List<(Map HeatMap, Point3D Location)> CDeathData { get; private set; }

        private static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
        {
            if (CDeathData == null)
            {
                CDeathData = new List<(Map HeatMap, Point3D Location)>();
            }

            while (CDeathData.Count > MaxDataStored)
            {
                CDeathData.RemoveAt(0);
            }

            if (!CDeathData.Contains((e.Creature.Map, e.Creature.Location)))
            {
                CDeathData.Add((e.Creature.Map, e.Creature.Location));
            }
        }

        public static List<(Map HeatMap, Point3D Location)> PDeathData { get; private set; }

        private static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
        {
            if (PDeathData == null)
            {
                PDeathData = new List<(Map HeatMap, Point3D Location)>();
            }

            while (PDeathData.Count > MaxDataStored)
            {
                PDeathData.RemoveAt(0);
            }

            if (!PDeathData.Contains((e.Mobile.Map, e.Mobile.Location)))
            {
                PDeathData.Add((e.Mobile.Map, e.Mobile.Location));
            }
        }

        public static List<(Map HeatMap, Point3D Location)> QuestData { get; private set; }

        private static void EventSink_QuestComplete(QuestCompleteEventArgs e)
        {
            if (QuestData == null)
            {
                QuestData = new List<(Map HeatMap, Point3D Location)>();
            }

            while (QuestData.Count > MaxDataStored)
            {
                QuestData.RemoveAt(0);
            }

            if (!QuestData.Contains((e.Mobile.Map, e.Mobile.Location)))
            {
                QuestData.Add((e.Mobile.Map, e.Mobile.Location));
            }
        }

        public static List<(Map HeatMap, Point3D Location)> HarvestData { get; private set; }

        private static void EventSink_ResourceHarvestSuccess(ResourceHarvestSuccessEventArgs e)
        {
            if (HarvestData == null)
            {
                HarvestData = new List<(Map HeatMap, Point3D Location)>();
            }

            while (HarvestData.Count > MaxDataStored)
            {
                HarvestData.RemoveAt(0);
            }

            if (!HarvestData.Contains((e.Harvester.Map, e.Harvester.Location)))
            {
                HarvestData.Add((e.Harvester.Map, e.Harvester.Location));
            }
        }

        public static List<(Map HeatMap, Point3D Location)> TameData { get; private set; }

        private static void EventSink_TameCreature(TameCreatureEventArgs e)
        {
            if (TameData == null)
            {
                TameData = new List<(Map HeatMap, Point3D Location)>();
            }

            while (TameData.Count > MaxDataStored)
            {
                TameData.RemoveAt(0);
            }

            if (!TameData.Contains((e.Mobile.Map, e.Mobile.Location)))
            {
                TameData.Add((e.Mobile.Map, e.Mobile.Location));
            }
        }

        public static List<(Map HeatMap, Point3D Location)> BuyData { get; private set; }

        private static void EventSink_ValidVendorPurchase(ValidVendorPurchaseEventArgs e)
        {
            if (BuyData == null)
            {
                BuyData = new List<(Map HeatMap, Point3D Location)>();
            }

            while (BuyData.Count > MaxDataStored)
            {
                BuyData.RemoveAt(0);
            }

            if (!BuyData.Contains((e.Mobile.Map, e.Mobile.Location)))
            {
                BuyData.Add((e.Mobile.Map, e.Mobile.Location));
            }
        }

        public static List<(Map HeatMap, Point3D Location)> SellData { get; private set; }

        private static void EventSink_ValidVendorSell(ValidVendorSellEventArgs e)
        {
            if (SellData == null)
            {
                SellData = new List<(Map HeatMap, Point3D Location)>();
            }

            while (SellData.Count > MaxDataStored)
            {
                SellData.RemoveAt(0);
            }

            if (!SellData.Contains((e.Mobile.Map, e.Mobile.Location)))
            {
                SellData.Add((e.Mobile.Map, e.Mobile.Location));
            }
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            Persistence.Serialize(FilePath, OnSerialize);
        }

        private static void OnSerialize(GenericWriter writer)
        {
            // CraftData
            writer.Write(CraftData?.Count ?? 0);

            if (CraftData != null)
            {
                foreach (var (map, location) in CraftData)
                {
                    writer.Write(map);
                    writer.Write(location);
                }
            }

            // CDeathData
            writer.Write(CDeathData?.Count ?? 0);

            if (CDeathData != null)
            {
                foreach (var (map, location) in CDeathData)
                {
                    writer.Write(map);
                    writer.Write(location);
                }
            }

            // PDeathData
            writer.Write(PDeathData?.Count ?? 0);

            if (PDeathData != null)
            {
                foreach (var (map, location) in PDeathData)
                {
                    writer.Write(map);
                    writer.Write(location);
                }
            }

            // QuestData
            writer.Write(QuestData?.Count ?? 0);

            if (QuestData != null)
            {
                foreach (var (map, location) in QuestData)
                {
                    writer.Write(map);
                    writer.Write(location);
                }
            }

            // HarvestData
            writer.Write(HarvestData?.Count ?? 0);

            if (HarvestData != null)
            {
                foreach (var (map, location) in HarvestData)
                {
                    writer.Write(map);
                    writer.Write(location);
                }
            }

            // TameData
            writer.Write(TameData?.Count ?? 0);

            if (TameData != null)
            {
                foreach (var (map, location) in TameData)
                {
                    writer.Write(map);
                    writer.Write(location);
                }
            }

            // BuyData
            writer.Write(BuyData?.Count ?? 0);

            if (BuyData != null)
            {
                foreach (var (map, location) in BuyData)
                {
                    writer.Write(map);
                    writer.Write(location);
                }
            }

            // SellData
            writer.Write(SellData?.Count ?? 0);

            if (SellData != null)
            {
                foreach (var (map, location) in SellData)
                {
                    writer.Write(map);
                    writer.Write(location);
                }
            }
        }

        private static void EventSink_ServerStarted()
        {
            Persistence.Deserialize(FilePath, OnDeserialize);
        }

        private static void OnDeserialize(GenericReader reader)
        {
            if (CraftData == null)
            {
                InitiateData();
            }

            // CraftData
            var craftCount = reader.ReadInt();

            if (craftCount > 0)
            {
                for (var i = 0; i < craftCount; i++)
                {
                    CraftData.Add((reader.ReadMap(), reader.ReadPoint3D()));
                }
            }

            // CDeathData
            var cDeathCount = reader.ReadInt();

            if (cDeathCount > 0)
            {
                for (var i = 0; i < cDeathCount; i++)
                {
                    CDeathData.Add((reader.ReadMap(), reader.ReadPoint3D()));
                }
            }

            // PDeathData
            var pDeathCount = reader.ReadInt();

            if (pDeathCount > 0)
            {
                for (var i = 0; i < pDeathCount; i++)
                {
                    PDeathData.Add((reader.ReadMap(), reader.ReadPoint3D()));
                }
            }

            // QuestData
            var questCount = reader.ReadInt();

            if (questCount > 0)
            {
                for (var i = 0; i < questCount; i++)
                {
                    QuestData.Add((reader.ReadMap(), reader.ReadPoint3D()));
                }
            }

            // HarvestData
            var harvestCount = reader.ReadInt();

            if (harvestCount > 0)
            {
                for (var i = 0; i < harvestCount; i++)
                {
                    HarvestData.Add((reader.ReadMap(), reader.ReadPoint3D()));
                }
            }

            // TameData
            var tameCount = reader.ReadInt();

            if (tameCount > 0)
            {
                for (var i = 0; i < tameCount; i++)
                {
                    TameData.Add((reader.ReadMap(), reader.ReadPoint3D()));
                }
            }

            // BuyData
            var buyCount = reader.ReadInt();

            if (buyCount > 0)
            {
                for (var i = 0; i < buyCount; i++)
                {
                    BuyData.Add((reader.ReadMap(), reader.ReadPoint3D()));
                }
            }

            // SellData
            var sellCount = reader.ReadInt();

            if (sellCount > 0)
            {
                for (var i = 0; i < sellCount; i++)
                {
                    SellData.Add((reader.ReadMap(), reader.ReadPoint3D()));
                }
            }
        }
    }
}

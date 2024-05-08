using Server.Items;
using System.Collections.Generic;

namespace Server.Custom.InvasionSystem
{
    internal class InvasionLedger : BlankScroll
    {
        private List<(Map map, string name)> InvasionData;

        public InvasionLedger()
        {
            Name = "Invasion Ledge";

            Hue = 0x4001;

            Movable = false;

            InvasionData = new List<(Map map, string name)>();

            if (InvasionSettings.InvasionSysDEBUG)
            {
                World.Broadcast(53, false, "Ledger Added!");
            }
        }

        public void AddLedgerToWorld()
        {
            Point3D location = new Point3D(3706, 1226, 3);

            base.OnBeforeSpawn(location, Map.Trammel);

            MoveToWorld(location, Map.Trammel);

            base.OnAfterSpawn();
        }

        public void ResetData()
        {
            InvasionData.Clear();
        }

        public void AddData(Map map, string name)
        {
            InvasionData.Add((map, name));
        }

        public (Map map, string name) GetData(int position)
        {
            if (InvasionData.Count > position)
            {
                return InvasionData[position];
            }

            if (InvasionSettings.InvasionSysDEBUG)
            {
                World.Broadcast(53, false, $"Ledger Loaded : [{position}]");
            }

            return (Map.Internal, string.Empty);
        }

        public override void OnDelete()
        {
            InvasionStore.IsLedgerInitialized = false;

            InvasionStore.InitializeLedger();

            base.OnDelete();
        }

        public InvasionLedger(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(InvasionData.Count);

            if (InvasionData != null && InvasionData.Count > 0)
            {
                foreach (var data in InvasionData)
                {
                    writer.Write(data.map);
                    writer.Write(data.name);
                }
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int count = reader.ReadInt();

            InvasionData = new List<(Map map, string name)>();

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    InvasionData.Add((reader.ReadMap(), reader.ReadString()));
                }
            }
        }
    }
}

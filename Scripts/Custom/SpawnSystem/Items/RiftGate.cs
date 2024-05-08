using Server.Items;
using System;

namespace Server.Custom.SpawnSystem.Items
{
    internal class RiftGate : Moongate
    {
        public override bool ShowFeluccaWarning { get { return false; } }

        public override bool TeleportPets { get { return true; } }

        [Constructable]
        public RiftGate(Point3D location, Map map)
        {
            Dispellable = false;

            Target = location;

            if (map == Map.Trammel)
            {
                TargetMap = Map.Felucca;

                Hue = 2750;
            }

            if (map == Map.Felucca)
            {
                TargetMap = Map.Trammel;

                Hue = 2728;
            }

            Movable = false;

            Light = LightType.Circle300;

            InternalTimer t = new InternalTimer(this);

            t.Start();
        }

        public RiftGate(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Delete();
        }

        //Deletion Timer
        private class InternalTimer : Timer
        {
            private readonly Item m_Item;

            public InternalTimer(Item item) : base(TimeSpan.FromSeconds(30.0))
            {
                Priority = TimerPriority.OneSecond;

                m_Item = item;
            }

            protected override void OnTick()
            {
                if (!m_Item.Deleted)
                {
                    m_Item.Delete();
                }
            }
        }
    }
}

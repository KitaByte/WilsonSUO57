using System;
using System.Collections.Generic;
using Server.Commands;
using Server.Gumps;
using Server.Items;

namespace Server.Custom.Misc
{
    public class PrizeMoongate : Moongate
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public int Duration { get; set; } = 60; // Seconds

        [CommandProperty(AccessLevel.GameMaster)]
        public Map DestMap { get { return TargetMap; } set { TargetMap = value; StartGate(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Destination { get { return Target; } set { Target = value; StartGate(); } }

        [CommandProperty(AccessLevel.GameMaster)]
        public string PrizeBagName { get; set; } = "Prize Bag";

        [CommandProperty(AccessLevel.GameMaster)]
        public Bag PrizeBag { get { return prizeBag; } set { prizeBag = value; StartGate(); } }

        private Bag prizeBag;

        private static Point3D defaultDest = new Point3D(0, 0, 0);

        private readonly List<Mobile> PassengerList;

        [Constructable]
        public PrizeMoongate() : base(defaultDest, Map.Internal)
        {
            PassengerList = new List<Mobile>();

            Hue = 0x4001;
        }

        private bool IsReady()
        {
            return DestMap != null && Destination != defaultDest && prizeBag != null;
        }

        public PrizeMoongate(Serial serial) : base(serial)
        {
        }

        private void StartGate()
        {
            if (IsReady())
            {
                Hue = Utility.RandomBrightHue();

                new InternalTimer(this, Duration).Start();

                Effects.PlaySound(Location, Map, 0x20E);

                Effects.SendLocationEffect(Location, Map, 0x3789, 15, Hue, 0);
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.AccessLevel >= AccessLevel.GameMaster && !IsReady())
            {
                from.SendGump(new PropertiesGump(from, this));

                from.SendMessage(53, "Quick Props : by Wilson!");
            }
            else
            {
                base.OnDoubleClick(from);
            }
        }

        public override void UseGate(Mobile m)
        {
            if (IsReady())
            {
                if (PassengerList.Contains(m))
                {
                    m.SendMessage(53, "Sure, go again, but no prize this time!");

                    Effects.PlaySound(Location, Map, 0x543);

                    base.UseGate(m);
                }
                else
                {
                    var bagCopy = Dupe.DupeItem(prizeBag);

                    bagCopy.Name = PrizeBagName;

                    bagCopy.Hue = Utility.RandomBrightHue();

                    m.AddToBackpack(bagCopy);

                    m.SendMessage(53, "Enjoy your prize bag!");

                    Effects.PlaySound(Location, Map, 0x5A7);

                    PassengerList.Add(m);

                    base.UseGate(m);
                }
            }
            else
            {
                m.SendMessage(53, "Please wait for the gate to start!");
            }
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

        private class InternalTimer : Timer
        {
            private readonly PrizeMoongate m_Gate;

            private int m_Duration;

            public InternalTimer(PrizeMoongate gate, int duration): base(TimeSpan.FromSeconds(1.0))
            {
                m_Gate = gate;

                m_Duration = duration;

                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Duration > 0)
                {
                    if (m_Duration == 10)
                    {
                        Effects.PlaySound(m_Gate.Location, m_Gate.Map, 0x507);
                    }

                    if (m_Duration == 5)
                    {
                        m_Gate.Hue = Utility.RandomRedHue();

                        m_Gate.PublicOverheadMessage(Network.MessageType.Yell, m_Gate.Hue, false, "!-HURRY-!");
                    }

                    if (m_Duration < 10)
                    {
                        m_Gate.PublicOverheadMessage(Network.MessageType.Yell, m_Gate.Hue, false, m_Duration.ToString());
                    }

                    m_Duration--;

                    Start();
                }
                else
                {
                    Effects.PlaySound(m_Gate.Location, m_Gate.Map, Utility.RandomList(0x3E, 0x3F));

                    Effects.SendLocationEffect(m_Gate.Location, m_Gate.Map, 0x9F89, 15, Utility.RandomBrightHue(), 0);

                    m_Gate.Delete();
                }
            }
        }
    }
}

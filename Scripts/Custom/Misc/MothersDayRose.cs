using System;
using Server.Engines.Quests;
using Server.Gumps;
using Server.Items;
using Server.Multis;

namespace Server.Custom.Misc
{
    [Flipable(0x234C, 0x234D)]
    public class MothersDayRose : Item, ISecurable
    {
        private SecureLevel m_Level;

        [CommandProperty(AccessLevel.GameMaster)]
        public SecureLevel Level
        {
            get
            {
                return m_Level;
            }
            set
            {
                m_Level = value;
            }
        }

        [Constructable]
        public MothersDayRose() : base(0x234D)
        {
            Name = $"Mother's Day Rose - {DateTime.Now.Year}";

            Hue = Utility.RandomRedHue();
        }

        private bool inUse = false;

        public override void OnDoubleClick(Mobile from)
        {
            if (!inUse && RootParent == null && IsLockedDown)
            {
                inUse = true;

                var hearts = new Static(0x4AA4);

                hearts.MoveToWorld(Location, Map);

                if (from.Female)
                {
                    from.PlaySound(0x430);
                }
                else
                {
                    from.PlaySound(0x320);
                }

                from.SendMessage(Utility.RandomBirdHue(), "Happy Mother's Day!");

                Timer.DelayCall(TimeSpan.FromSeconds(5), () =>
                {
                    hearts.Delete();

                    inUse = false;
                });
            }

            base.OnDoubleClick(from);
        }

        public MothersDayRose(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version

            writer.WriteEncodedInt((int)m_Level);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            _ = reader.ReadEncodedInt();

            m_Level = (SecureLevel)reader.ReadEncodedInt();
        }
    }
}

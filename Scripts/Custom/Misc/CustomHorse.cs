using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Custom
{
    [CorpseName("a horse corpse")]
    [TypeAlias("Server.Mobiles.BrownHorse", "Server.Mobiles.DirtyHorse", "Server.Mobiles.GrayHorse", "Server.Mobiles.TanHorse")]
    internal class CustomHorse : BaseMount
    {
        private static readonly int[] m_IDs = new int[]
        {
            0xC8, 0x3E9F,
            0xE2, 0x3EA0,
            0xE4, 0x3EA1,
            0xCC, 0x3EA2
        };

        List<HorsePoo> pooList;

        [Constructable]
        public CustomHorse()  : this("a horse")
        {
            pooList = new List<HorsePoo>();
        }

        [Constructable]
        public CustomHorse(string name) : base(name, 0xE2, 0x3EA0, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            int random = Utility.Random(4);

            Body = m_IDs[random * 2];
            ItemID = m_IDs[random * 2 + 1];
            BaseSoundID = 0xA8;

            SetStr(22, 98);
            SetDex(56, 75);
            SetInt(6, 10);

            SetHits(28, 45);
            SetMana(0);

            SetDamage(3, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);

            SetSkill(SkillName.MagicResist, 25.1, 30.0);
            SetSkill(SkillName.Tactics, 29.3, 44.0);
            SetSkill(SkillName.Wrestling, 29.3, 44.0);

            Fame = 300;
            Karma = 300;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 29.1;
        }

        public CustomHorse(Serial serial) : base(serial)
        {
        }

        public override void OnAfterMove(Point3D oldLocation)
        {
            if (Utility.RandomDouble() < 0.05)
            {
                HorsePoo poo = new HorsePoo();

                pooList.Add(poo);

                poo.MoveToWorld(oldLocation, Map);

                Say("farts");
            }

            if (pooList.Count > 3)
            {
                HorsePoo poo = pooList.Last();

                pooList.Remove(poo);

                poo.Delete();
            }

            base.OnAfterMove(oldLocation);
        }

        public override void OnDeath(Container c)
        {
            RemovePoo();

            base.OnDeath(c);
        }

        public override void OnDelete()
        {
            RemovePoo();

            base.OnDelete();
        }

        private void RemovePoo()
        {
            for (int i = 0; i < pooList.Count; i++)
            {
                pooList[i].Delete();
            }

            pooList.Clear();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        private class HorsePoo : Item
        {
            public override TimeSpan DecayTime => TimeSpan.FromSeconds(3);

            public override bool Decays => true;

            [Constructable]
            public HorsePoo() : base(0x0913)
            {
                Name = "poo";

                Hue = 1175;

                Movable = false;
            }

            public HorsePoo(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                int version = reader.ReadInt();
            }
        }
    }
}

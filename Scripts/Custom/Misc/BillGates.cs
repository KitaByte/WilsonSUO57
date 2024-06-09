using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Corpse of Bill Gates")]
    public class BillGates : BaseCreature
    {
        private DateTime m_TalkedLast;

        [Constructable]
        public BillGates() : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.8, 3.0)
        {
            Body = 0x190;

            Name = "Bill Gates";

            Hue = Utility.RandomSnakeHue(); // lol

            SetStr(55);
            SetDex(33);
            SetInt(20);

            Fame = 20000;
            Karma = -20000;
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            SayRandom(from);

            return base.HandlesOnSpeech(from);
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            SayRandom(m);

            base.OnMovement(m, oldLocation);
        }

        private void SayRandom(Mobile m)
        {
            if (m.Player && m_TalkedLast < DateTime.Now - TimeSpan.FromSeconds(5) && InRange(m, 5))
            {
                m_TalkedLast = DateTime.Now;

                Say(GetRandomSpeach());

                SpawnGold(Location, Map);

                MovingParticles(m, 0x36D4, 7, 0, false, false, 9502, 4019, 0x160);
            }
        }

        private static string GetRandomSpeach()
        {
            return Utility.RandomList
            (
                "Hey, let's make a deal - you get vaccinated and I'll give you Windows 11 for free!",
                "I am the richest man in the world, so listen to my advice on vaccination!",
                "Trust me, I am an expert when it comes to computer viruses and vaccines!",
                "My vaccine will save the world, but first, you need to enter your credit card details!",
                "With my vaccine, you will not only get healthy, but also receive a subscription for Microsoft Office!",
                "I developed this vaccine with a lot of dedication, just like I used to work on software!",
                "Accept the vaccination, or I'll buy your city and have you thrown out!",
                "Trust me, I am your best friend from afar - get vaccinated!"
            );
        }

        private static void SpawnGold(Point3D location, Map map)
        {
            int goldCount = Utility.RandomMinMax(1, 5);

            for (int i = 0; i < goldCount; i++)
            {
                Gold gold = new Gold(Utility.RandomMinMax(1, 1000));

                int dist = Utility.RandomMinMax(0, 2);

                if (Utility.RandomDouble() < 0.5)
                {
                    gold.MoveToWorld(new Point3D(location.X + dist, location.Y + dist, location.Z), map);
                }
                else
                {
                    gold.MoveToWorld(new Point3D(location.X - dist, location.Y - dist, location.Z), map);
                }

                Effects.PlaySound(location, map, gold.GetDropSound());
            }
        }

        public BillGates(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}

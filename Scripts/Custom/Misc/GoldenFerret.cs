using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Misc
{
    public class GoldenFerret : BaseCreature
    {
        public static void Initialize()
        {
            EventSink.CreatureDeath += EventSink_CreatureDeath;
        }

        private static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
        {
            if (e.Killer is PlayerMobile pm && e.Creature is BaseCreature bc && bc.Karma < -10000)
            {
                if (pm.SkillsTotal > 699 && Utility.RandomDouble() < 0.01)
                {
                    GoldenFerret ferret = new GoldenFerret();

                    ferret.MoveToWorld(e.Corpse.Location, e.Corpse.Map);

                    ferret.BoltEffect(2734);
                }
            }
        }

        internal int Counter { get; set; }

        private readonly Timer _CountTimer;

        [Constructable]
        public GoldenFerret() : base(AIType.AI_Animal, FightMode.Aggressor, 16, 1, 0.1, 0.4)
        {
            Name = "a Golden Ferret";

            Body = 0x117;

            Hue = 2734;

            SetStr(90);
            SetDex(125);
            SetInt(125);

            SetHits(1000, 5000);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 10);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 80);

            SetSkill(SkillName.MagicResist, 80.0);
            SetSkill(SkillName.Tactics, 80.0);
            SetSkill(SkillName.Wrestling, 80.0);

            Fame = 20000;
            Karma = 0;

            Tamable = false;

            _CountTimer = new CountTimer(this);

            Counter = Utility.RandomMinMax(15, 59);
        }

        public override bool CanFlee => true;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (!_CountTimer.Running && InRange(m, 10))
            {
                _CountTimer.Start();
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnDeath(Container c)
        {
            if (Counter > 0)
            {
                c.AddItem(new Gold(Utility.RandomMinMax(500, 1000 * Counter)));
            }
            else
            {
                c.AddItem(new Gold(500));
            }

            base.OnDeath(c);
        }

        public override void OnThink()
        {
            if (Combatant != null && InRange(Combatant, Counter))
            {
                Combatant = null;

                if (Utility.RandomDouble() < 0.5)
                {
                    Point3D loc = Map.GetRandomSpawnPoint(new Rectangle2D(Location.X - 20, Location.Y - 20, 40, 40));

                    MoveToWorld(loc, Map);

                    BoltEffect(2734);
                }
            }
            else
            {
                base.OnThink();
            }
        }

        public GoldenFerret(Serial serial) : base(serial)
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

    internal class CountTimer : Timer
    {
        private readonly GoldenFerret Ferret;

        public CountTimer(GoldenFerret ferret) : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
        {
            Priority = TimerPriority.OneSecond;

            Ferret = ferret;
        }

        protected override void OnTick()
        {
            if (Ferret.Counter > 0)
            {
                if (Ferret.Combatant == null)
                {
                    Ferret.Counter--;

                    Ferret.Say($"{Ferret.Counter}", Ferret.Counter + 32, true);
                }
            }
            else
            {
                if (Ferret.Alive)
                {
                    Ferret.BoltEffect(2734);

                    Ferret.Delete();
                }

                Stop();
            }
        }
    }
}

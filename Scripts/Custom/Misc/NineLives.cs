using System.Linq;
using Server.Mobiles;

namespace Server.Custom.Misc
{
    public static class NineLives
    {
        private static bool NL_Active = false;

        public static bool IsActive()
        {
            return NL_Active;
        }

        public static void Initialize()
        {
            EventSink.ServerStarted += EventSink_ServerStarted;

            EventSink.CreatureDeath += EventSink_CreatureDeath;

            NL_Active = true;
        }

        private static void EventSink_ServerStarted()
        {
            var cats = World.Mobiles.Values.Where(c => c is Cat && c.Deaths > 0).ToList();

            if (cats != null && cats.Count > 0)
            {
                for (int i = 0; i < cats.Count; i++)
                {
                    cats[i].Delete();
                }
            }
        }

        private static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
        {
            if (NL_Active && e.Creature is BaseCreature bc && bc is Cat c && !c.Controlled)
            {
                if (c.Deaths < 9)
                {
                    c.Deaths++;

                    Cat cat = new Cat()
                    {
                        Name = c.Name,
                        Hue = c.Hue,
                        Deaths = c.Deaths,
                        Tamable = false,
                        Home = c.Home,
                        RangeHome = c.RangeHome
                    };

                    if (e.Corpse != null && !e.Corpse.Deleted)
                    {
                        Effects.SendLocationEffect(e.Corpse.Location, e.Corpse.Map, 0x3789, 20);

                        Effects.PlaySound(e.Corpse.Location, e.Corpse.Map, Utility.RandomList(0x69, 0x6A, 0x6B, 0x6C, 0x6D));

                        cat.MoveToWorld(e.Corpse.Location, e.Corpse.Map);

                        e.Corpse.Delete();
                    }
                }
            }
        }
    }
}

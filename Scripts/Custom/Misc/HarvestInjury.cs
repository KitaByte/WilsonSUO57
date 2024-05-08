using System.Collections.Concurrent;
using Server.Mobiles;

namespace Server.Custom.Misc
{
    internal static class HarvestInjury
    {
        private const double ChanceRate = 0.001; // Chance added per harvest to total! (Random Double < Chance Total = Injury)

        private const int DamageMod = 2; // Damage modifier! (Hits/Modifier = Max Damage)

        private static readonly ConcurrentDictionary<Mobile, double> WorkingList = new ConcurrentDictionary<Mobile, double>();

        public static void Initialize()
        {
            EventSink.ResourceHarvestAttempt += EventSink_ResourceHarvestAttempt;
        }

        private static void EventSink_ResourceHarvestAttempt(ResourceHarvestAttemptEventArgs e)
        {
            if (e.Harvester is PlayerMobile pm && IsWorking(pm))
            {
                if (Utility.RandomDouble() < GetInjuryChance(pm))
                {
                    ApplyInjury(pm);
                }
            }
        }

        private static bool IsWorking(PlayerMobile pm)
        {
            lock (WorkingList)
            {
                if (WorkingList.ContainsKey(pm))
                {
                    WorkingList[pm] += ChanceRate;

                    return true;
                }

                WorkingList.TryAdd(pm, 0);

                return false;
            }
        }

        private static double GetInjuryChance(PlayerMobile pm)
        {
            lock (WorkingList)
            {
                return WorkingList[pm];
            }
        }

        private static void ApplyInjury(PlayerMobile pm)
        {
            if (pm.Alive)
            {
                int damage;

                if (pm.Hits / DamageMod > 1)
                {
                    damage = Utility.RandomMinMax(1, pm.Hits / DamageMod);
                }
                else
                {
                    damage = 1;
                }

                pm.Damage(damage);

                pm.SendMessage(42, $"You suffered a {GetRandomInjury()}!");

                if (pm.Female)
                {
                    pm.PlaySound(Utility.RandomList(0x53C, 0x544));
                }
                else
                {
                    pm.PlaySound(Utility.RandomList(0x53F, 0x547));
                }

                lock (WorkingList)
                {
                    WorkingList[pm] /= 2;
                }
            }
        }

        private static string GetRandomInjury()
        {
            string[] injuries =
                {
                    "twisted ankle",
                    "strained muscle",
                    "deep cut",
                    "bruised hand",
                    "sprained wrist",
                    "fractured finger",
                    "pulled shoulder",
                    "torn ligament",
                    "blistered palm",
                    "cut forearm",
                    "sore back",
                    "strained neck",
                    "sprained ankle",
                    "cracked rib",
                    "splintered finger",
                    "bruised shin",
                    "strained tendon",
                    "torn muscle",
                    "sliced hand",
                    "wrenched knee",
                    "strained elbow",
                    "bruised hip",
                    "tweaked shoulder",
                    "pulled hamstring",
                    "swollen thumb",
                    "scraped knee",
                    "strained calf",
                    "torn skin",
                    "sore wrist",
                    "stiff neck",
                    "cut finger",
                    "blistered heel",
                    "sore palm",
                    "cramped hand",
                    "twisted wrist",
                    "strained thumb",
                    "bruised knuckles",
                    "pulled back",
                    "torn ligament",
                    "sliced palm",
                    "wrenched ankle",
                    "strained shoulder"
                };

            return injuries[Utility.Random(injuries.Length)];
        }
    }
}

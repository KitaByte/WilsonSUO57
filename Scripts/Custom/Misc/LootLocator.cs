using System;
using System.Linq;
using Server.Items;
using Server.Commands;
using Server.Mobiles;
using Server.Regions;
using System.Reflection;
using System.Collections.Generic;
using CPA = Server.CommandPropertyAttribute;

namespace Server.Custom.Commands
{
    internal static class LootLocator
    {
        private const int Timed_Usage = 1;

        private static List<string> RestrictedLoot = new List<string>()
        {
            //nameof(HatOfTheMagi),
            //nameof(AxeOfTheHeavens),
            //nameof(SwordOfJustice)
        };

        private static readonly List<(DateTime lastCheck, Mobile player)> UsageList = new List<(DateTime lastCheck, Mobile player)>();

        private static List<Mobile> MobList = new List<Mobile>();

        private static bool listInitialized = false;

        public static void Initialize()
        {
            CommandSystem.Register("LocateLoot", AccessLevel.Player, new CommandEventHandler(LocateLoot_OnCommand));

            EventSink.Login += EventSink_Login;

            EventSink.MobileCreated += EventSink_MobileCreated;

            EventSink.MobileDeleted += EventSink_MobileDeleted;
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            if (!listInitialized || MobList.Count == 0)
            {
                listInitialized = true;

                MobList = World.Mobiles.Values.Where(m => IsValidMob(m)).ToList();
            }
        }

        private static void EventSink_MobileCreated(MobileCreatedEventArgs e)
        {
            if (listInitialized && IsValidMob(e.Mobile) && !MobList.Contains(e.Mobile))
            {
                MobList.Add(e.Mobile);
            }
        }

        private static void EventSink_MobileDeleted(MobileDeletedEventArgs e)
        {
            if (MobList.Contains(e.Mobile))
            {
                MobList.Remove(e.Mobile);
            }
        }

        private static bool IsValidMob(Mobile m)
        {
            if (m.Map == Map.Internal) return false;

            if (m.Backpack == null) return false;

            if (m.Backpack.Items.Count == 0) return false;

            if (m.Karma >= 0) return false;

            if (m is BaseCreature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Locate Loot
        [Usage("LocateLoot")]
        [Description("LocateLoot <Loot Name>")]
        public static void LocateLoot_OnCommand(CommandEventArgs e)
        {
            bool isValid = true;

            int position = -1;

            int timeLeft = 0;

            try
            {
                foreach (var log in UsageList)
                {
                    if (log.player == e.Mobile)
                    {
                        position = UsageList.IndexOf(log);

                        if (log.lastCheck > DateTime.Now - TimeSpan.FromMinutes(Timed_Usage))
                        {
                            isValid = false;

                            TimeSpan elapsedTime = DateTime.Now - log.lastCheck;

                            int totalElapsedMinutes = (int)elapsedTime.TotalMinutes;

                            timeLeft = Timed_Usage - (totalElapsedMinutes % Timed_Usage);
                        }
                    }
                }

                if (e.Arguments.Length > 0 && isValid)
                {
                    if (position != -1)
                    {
                        UsageList.RemoveAt(position);
                    }

                    string name = e.Arguments[0];

                    if (!string.IsNullOrEmpty(name) && !RestrictedLoot.Contains(name))
                    {
                        if (MobList.Count > 0)
                        {
                            bool found = false;

                            var refinedMobs = new List<Mobile>();

                            foreach (var mobile in MobList)
                            {
                                if (mobile.Backpack.Items.Find(i => i.GetType().Name.ToLower() == name) != null)
                                {
                                    found = true;

                                    refinedMobs.Add(mobile);
                                }
                            }

                            if (found && refinedMobs.Count > 0)
                            {
                                Mobile closestMob = null;

                                foreach (var refinedMob in refinedMobs)
                                {
                                    if (closestMob == null || closestMob.GetDistanceToSqrt(e.Mobile) > refinedMob.GetDistanceToSqrt(e.Mobile))
                                    {
                                        closestMob = refinedMob;
                                    }
                                }

                                SetMob(e, name, closestMob);
                            }
                            else
                            {
                                try
                                {
                                    var strongerMobList = MobList.Where(sm => sm.Hits > 450 && !sm.CanSwim).ToList();

                                    if (strongerMobList != null && strongerMobList.Count > 0)
                                    {
                                        var mob = MobList[Utility.RandomMinMax(0, MobList.Count - 1)];

                                        if (mob is BaseCreature bc)
                                        {
                                            bc.HitsMaxSeed = bc.HitsMax * 3;

                                            bc.Hits = bc.HitsMaxSeed;
                                        }

                                        Type item_Type = ScriptCompiler.FindTypeByName(Spawner.ParseType(name)) ?? typeof(DirtPatch);

                                        if (item_Type != typeof(DirtPatch))
                                        {
                                            var item = Build(item_Type, CommandSystem.Split(item_Type.Name)) as Item;

                                            mob.Backpack.AddItem(item);

                                            SetMob(e, name, mob);
                                        }
                                        else
                                        {
                                            e.Mobile.SendMessage(42, $"{name} was not found!");
                                        }
                                    }
                                    else
                                    {
                                        e.Mobile.SendMessage(42, "Creature was not found!");
                                    }
                                }
                                catch
                                {
                                    e.Mobile.SendMessage(42, $"{name} was not found!");
                                }
                            }
                        }
                        else
                        {
                            e.Mobile.SendMessage(42, "Creature was not found!");
                        }
                    }
                    else
                    {
                        e.Mobile.SendMessage(42, $"{name} was not found!");
                    }
                }
                else
                {
                    if (isValid)
                    {
                        e.Mobile.SendMessage(42, "Invalid Command: LocateLoot <Loot Name>");
                    }
                    else
                    {
                        e.Mobile.SendMessage(42, $"Limited Usage: wait {timeLeft} mins!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Locate Loot [ERROR]: {ex.Message}");
            }
        }

        private static void SetMob(CommandEventArgs e, string name, Mobile mob)
        {
            if (mob != null)
            {
                UsageList.Add((DateTime.Now, e.Mobile));

                e.Mobile.SendMessage(53, $"Found a {name} on a {mob.Name}!");

                if (mob.Region != null && mob.Region.IsPartOf(typeof(DungeonRegion)))
                {
                    e.Mobile.SendMessage(63, $"You can find them in {mob.Region}!");
                }
                else
                {
                    e.Mobile.SendMessage(63, $"Map added to your backpack!");

                    var lootMap = new SimpleMap()
                    {
                        Name = $"Map for {mob.Name}'s location",
                        NewPin = new Point2D(mob.Location.X, mob.Location.Y),
                        Facet = mob.Map,
                        BlessedFor = e.Mobile
                    };

                    e.Mobile.AddToBackpack(lootMap);
                }
            }
            else
            {
                e.Mobile.SendMessage(42, "Couldn't locate creature!");
            }
        }

        private static ISpawnable Build(Type type, string[] args)
        {
            bool isISpawnable = typeof(ISpawnable).IsAssignableFrom(type);

            if (!isISpawnable)
            {
                return null;
            }

            Add.FixArgs(ref args);

            string[,] props = null;

            for (int i = 0; i < args.Length; ++i)
            {
                if (Insensitive.Equals(args[i], "set"))
                {
                    int remains = args.Length - i - 1;

                    if (remains >= 2)
                    {
                        props = new string[remains / 2, 2];

                        remains /= 2;

                        for (int j = 0; j < remains; ++j)
                        {
                            props[j, 0] = args[i + (j * 2) + 1];
                            props[j, 1] = args[i + (j * 2) + 2];
                        }

                        Add.FixSetString(ref args, i);
                    }

                    break;
                }
            }

            PropertyInfo[] realProps = null;

            if (props != null)
            {
                realProps = new PropertyInfo[props.GetLength(0)];

                PropertyInfo[] allProps = type.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);

                for (int i = 0; i < realProps.Length; ++i)
                {
                    PropertyInfo thisProp = null;

                    string propName = props[i, 0];

                    for (int j = 0; thisProp == null && j < allProps.Length; ++j)
                    {
                        if (Insensitive.Equals(propName, allProps[j].Name))
                            thisProp = allProps[j];
                    }

                    if (thisProp != null)
                    {
                        CPA attr = Properties.GetCPA(thisProp);

                        if (attr != null && AccessLevel.Spawner >= attr.WriteLevel && thisProp.CanWrite && !attr.ReadOnly)
                            realProps[i] = thisProp;
                    }
                }
            }

            ConstructorInfo[] ctors = type.GetConstructors();

            for (int i = 0; i < ctors.Length; ++i)
            {
                ConstructorInfo ctor = ctors[i];

                if (!Add.IsConstructable(ctor, AccessLevel.Spawner))
                    continue;

                ParameterInfo[] paramList = ctor.GetParameters();

                if (args.Length == paramList.Length)
                {
                    object[] paramValues = Add.ParseValues(paramList, args);

                    if (paramValues == null)
                        continue;

                    object built = ctor.Invoke(paramValues);

                    if (built != null && realProps != null)
                    {
                        for (int j = 0; j < realProps.Length; ++j)
                        {
                            if (realProps[j] == null)
                                continue;

                            Properties.InternalSetValue(built, realProps[j], props[j, 1]);
                        }
                    }

                    return (ISpawnable)built;
                }
            }

            return null;
        }
    }
}

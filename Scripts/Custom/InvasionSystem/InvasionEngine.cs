using System;
using System.Linq;
using Server.Mobiles;
using Server.Commands;
using System.Reflection;
using System.Collections.Generic;
using CPA = Server.CommandPropertyAttribute;

namespace Server.Custom.InvasionSystem
{
    internal static class InvasionEngine
    {
        private static InvasionTimer _InvasionTimer;

        internal static void StartInvasionTimer()
        {
            if (_InvasionTimer == null)
            {
                _InvasionTimer = new InvasionTimer();
            }

            _InvasionTimer.Start();
        }

        internal static void StopInvasionTimer()
        {
            if (_InvasionTimer != null && _InvasionTimer.Running)
            {
                _InvasionTimer.Stop();
            }

            CleanMobs();
        }

        internal static bool InvasionActive { get; private set; } = false;

        internal static string InvasionMobType { get; private set; } = string.Empty;

        internal static void ResetInvasion()
        {
            InvasionActive = false;

            InvasionMobType = string.Empty;
        }

        public static void Initialize()
        {
            EventSink.ServerStarted += EventSink_ServerStarted;

            EventSink.BeforeWorldSave += EventSink_BeforeWorldSave;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;

            EventSink.Login += EventSink_Login;

            EventSink.Logout += EventSink_Logout;

            EventSink.CreatureDeath += EventSink_CreatureDeath;

            _InvasionTimer = new InvasionTimer();
        }

        private static void EventSink_ServerStarted()
        {
            CleanMobs();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Invasion System : Running = {InvasionSettings.InvasionSysEnabled}");
            Console.ResetColor();
        }

        private static void EventSink_BeforeWorldSave(BeforeWorldSaveEventArgs e)
        {
            StopInvasionTimer();

            if (InvasionSettings.InvasionSysDEBUG)
            {
                World.Broadcast(53, false, "Invasion System - Paused!");
            }
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            if (InvasionSettings.InvasionSysEnabled)
            {
                StartInvasionTimer();

                if (InvasionSettings.InvasionSysDEBUG)
                {
                    World.Broadcast(53, false, "Invasion System - Resumed!");
                }
            }
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            if (InvasionSettings.InvasionSysEnabled)
            {
                StartInvasionTimer();

                if (InvasionSettings.InvasionSysDEBUG)
                {
                    World.Broadcast(53, false, "Invasion System - Running!");
                }
            }
        }

        private static void EventSink_Logout(LogoutEventArgs e)
        {
            if (!World.Mobiles.Values.Any(m => m.IsPlayer()))
            {
                StopInvasionTimer();

                if (InvasionSettings.InvasionSysDEBUG)
                {
                    World.Broadcast(53, false, "Invasion System - Paused!");
                }
            }
        }

        private static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
        {
            if (InvasionSettings.InvasionSysEnabled && !InvasionActive)
            {
                if (e.Creature is BaseCreature bc && !bc.CanSwim && bc.Deaths != InvasionSettings.SYS_MARK)
                {
                    if (bc.GetType().GetConstructors().Any(c => c.GetParameters().Length == 0))
                    {
                        if (InvasionStore.AddMobToMap(bc.Map, bc.GetType().Name))
                        {
                            if (Utility.RandomDouble() < InvasionSettings.INVADE_CHANCE)
                            {
                                World.Broadcast(Utility.RandomRedHue(), false, $"The death of {bc.Name}, is causing unrest!");
                            }
                        }
                        else
                        {
                            if (InvasionSettings.InvasionSysDEBUG)
                            {
                                World.Broadcast(53, false, $"Invasion System - Invading Soon!");
                            }

                            if (Utility.RandomDouble() < InvasionSettings.INVADE_CHANCE)
                            {
                                if (InvasionStore.GetMapList(bc.Map, out List<(string name, int count)> list))
                                {
                                    if (list.Any(i => i.count == InvasionSettings.UNREST_LIMIT))
                                    {
                                        var info = list.First(o => o.count == InvasionSettings.UNREST_LIMIT);

                                        InvasionMobType = info.name;

                                        InvasionActive = true;

                                        list.Remove(info);

                                        World.Broadcast(Utility.RandomRedHue(), false, $"The death of {bc.Name}, has caused unrest!");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (InvasionSettings.InvasionSysDEBUG)
                {
                    World.Broadcast(53, false, $"Invasion System - Enabled => {InvasionSettings.InvasionSysEnabled} : Active => {InvasionActive}");
                }
            }
        }

        internal static Mobile GetSpawn()
        {
            try
            {
                Type mob_Type = ScriptCompiler.FindTypeByName(Spawner.ParseType(InvasionMobType)) ?? typeof(Rat);

                return Build(mob_Type, CommandSystem.Split(mob_Type.Name)) as Mobile;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"Invasion System - Build Error: {ex.Message}");
                Console.ResetColor();
            }

            return null;
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

        internal static List<Mobile> I_Players { get; private set; } = new List<Mobile>();

        internal static void CreatePlayerList()
        {
            I_Players.Clear();

            I_Players = World.Mobiles.Values.Where(m => m is PlayerMobile && m.Map != Map.Internal).ToList();
        }

        internal static void CleanMobs()
        {
            var mobs = World.Mobiles.Values.Where(m => m.Deaths == InvasionSettings.SYS_MARK).ToList();

            if (mobs != null && mobs.Count > 0)
            {
                var count = mobs.Count;

                for (int i = 0; i < count; i++)
                {
                    if (mobs.Last().Alive)
                    {
                        mobs.Last().Kill();
                    }
                }
            }

            if (InvasionSettings.InvasionSysDEBUG)
            {
                World.Broadcast(53, false, "Invasion System - Spawn Cleaned!");
            }
        }
    }
}

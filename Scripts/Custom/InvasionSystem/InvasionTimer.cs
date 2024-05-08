using System;
using System.Linq;
using Server.Mobiles;
using Server.Regions;

namespace Server.Custom.InvasionSystem
{
    internal class InvasionTimer : Timer
    {
        private TimeSpan InvasionDuration = TimeSpan.Zero;

        private DateTime EndTime= DateTime.Now;

        public InvasionTimer() : base(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(1))
        {
            Priority = TimerPriority.OneSecond;
        }

        protected override void OnTick()
        {
            if (InvasionSettings.InvasionSysDEBUG)
            {
                World.Broadcast(53, false, "Invasion System - Timer Tick!");
            }

            InvasionEngine.CreatePlayerList();

            InvasionStore.InitializeLedger();

            if (Priority != TimerPriority.OneMinute)
            {
                Priority = TimerPriority.OneMinute;
            }

            if (InvasionEngine.I_Players.Count > 0 && InvasionEngine.InvasionActive)
            {
                if (InvasionDuration == TimeSpan.Zero)
                {
                    if (InvasionEngine.I_Players.Count > 0)
                    {
                        InvasionDuration = Utility.RandomList(InvasionSettings.MIN_INVADE, InvasionSettings.BASE_INVADE, InvasionSettings.MAX_INVADE);
                    }

                    EndTime = DateTime.Now + InvasionDuration;

                    if (InvasionSettings.InvasionSysDEBUG)
                    {
                        World.Broadcast(53, false, "Invasion System - Activated!");
                    }
                }

                if (EndTime < DateTime.Now)
                {
                    InvasionDuration = TimeSpan.Zero;

                    EndTime = DateTime.Now;

                    InvasionEngine.ResetInvasion();

                    if (InvasionSettings.InvasionSysDEBUG)
                    {
                        World.Broadcast(53, false, "Invasion System - Ended!");
                    }

                    return;
                }

                foreach (var player in InvasionEngine.I_Players)
                {
                    if (!player.Region.IsPartOf(typeof(TownRegion)) && (!player.IsStaff() || InvasionSettings.InvasionSysDEBUG))
                    {
                        var count = player.GetMobilesInRange(InvasionSettings.CROWD_RANGE).ToList().Count;

                        if (count < InvasionSettings.CROWD_LIMIT && Utility.RandomDouble() < InvasionSettings.SPAWN_CHANCE)
                        {
                            var spawn = InvasionEngine.GetSpawn();

                            if (spawn != null && spawn is BaseCreature bc)
                            {
                                var locX = player.Location.X - 10;
                                var locY = player.Location.Y - 10;

                                var rect = new Rectangle2D(locX, locY, 20, 20);

                                Point3D spawnLocation;

                                try
                                {
                                    spawnLocation = player.Map.GetRandomSpawnPoint(rect);
                                }
                                catch
                                {
                                    spawnLocation = player.Location;
                                }

                                bc.IsParagon = true;

                                bc.Name = $"{bc.Name} the Fallen";

                                bc.Hue = 0x4001;

                                bc.Deaths = InvasionSettings.SYS_MARK;

                                bc.OnBeforeSpawn(spawnLocation, player.Map);

                                bc.MoveToWorld(spawnLocation, player.Map);

                                Effects.SendLocationEffect(bc.Location, bc.Map, 0x375A, 15, Utility.RandomRedHue(), 0);

                                bc.OnAfterSpawn();

                                bc.Say($"Death to {player.Name}, and their kind!");

                                bc.FightMode = FightMode.Closest;

                                bc.Combatant = player;

                                if (InvasionSettings.InvasionSysDEBUG)
                                {
                                    World.Broadcast(53, false, $"Invasion System - {bc.Name} Spawned!");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (!InvasionSettings.InvasionSysEnabled)
                {
                    InvasionEngine.CleanMobs();

                    Stop();
                }
            }
        }
    }
}

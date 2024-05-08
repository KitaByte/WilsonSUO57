using System;
using Server.Mobiles;
using Server.Commands;
using System.Collections.Generic;
using System.Linq;

namespace Server.Custom.Misc
{
    internal static class PowerHourCommand
    {
        internal static List<PowerHourEntry> PowerPlayers;

        public static void Initialize()
        {
            if (PowerPlayers == null)
            {
                PowerPlayers = new List<PowerHourEntry>();
            }

            CommandSystem.Register("PowerHour", AccessLevel.Player, new CommandEventHandler(PowerHour_OnCommand));
        }

        [Usage("PowerHour")]
        [Description("Power Hour Command")]
        private static void PowerHour_OnCommand(CommandEventArgs e)
        {
            RunCleanUp();

            if (e.Mobile.Alive)
            {
                PlayerMobile pm = e.Mobile as PlayerMobile;

                if (PowerPlayers.Any(p => p.PoweredPlayer == pm))
                {
                    var entry = PowerPlayers.First(p => p.PoweredPlayer == pm);

                    var time = DateTime.Now;

                    if (entry.LastMoved + TimeSpan.FromDays(1) < DateTime.Now)
                    {
                        PowerPlayers.Remove(entry);

                        entry.Delete();
                    }
                    else
                    {
                        TimeSpan timeLeft = entry.LastMoved + TimeSpan.FromDays(1) - DateTime.Now;

                        string timeLeftString = string.Format("{0} hours, {1} minutes", timeLeft.Hours, timeLeft.Minutes);

                        pm.SendMessage("Power Hour on Cool Down: {0} left", timeLeftString);
                    }
                }

                if (!PowerPlayers.Any(p => p.PoweredPlayer == pm))
                {
                    PowerPlayers.Add(new PowerHourEntry(pm) { LastMoved = DateTime.Now });

                    pm.AcceleratedStart = DateTime.Now + TimeSpan.FromHours(1);

                    pm.SendMessage("You have activated Power Hour! Enjoy your hour of advantage");

                    PlaySoundEffect(pm);

                    Timer.DelayCall(TimeSpan.FromHours(1), () =>
                    {
                        if (pm != null && !pm.Deleted)
                        {
                            pm.AcceleratedStart = DateTime.MinValue;

                            pm.SendMessage("Your Power Hour has ended. We hope you enjoyed it!");

                            PlaySoundEffect(pm);
                        }
                    });
                }
            }
        }

        private static void RunCleanUp()
        {
            var entryList = World.Items.Values.Where(i => i is PowerHourEntry)?.ToList();

            if (entryList != null && entryList.Count > 0)
            {
                List<PowerHourEntry> oldEntries = null;

                foreach (var item in entryList)
                {
                    if (!PowerPlayers.Contains(item))
                    {
                        if (oldEntries == null)
                        {
                            oldEntries = new List<PowerHourEntry>();
                        }

                        oldEntries.Add(item as PowerHourEntry);
                    }
                }

                if (oldEntries != null && oldEntries.Count > 0)
                {
                    var count = oldEntries.Count;

                    for (int i = 0; i < count; i++)
                    {
                        oldEntries[i].Delete();
                    }
                }
            }
        }

        private static void PlaySoundEffect(PlayerMobile pm)
        {
            pm.PlaySound(0x5C3);

            pm.BoltEffect(1153);
        }
    }

    public class PowerHourEntry : Item
    {
        public Mobile PoweredPlayer { get; private set; }

        public PowerHourEntry()
        {
        }

        public PowerHourEntry(Mobile player)
        {
            PoweredPlayer = player;
        }

        public PowerHourEntry(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteMobile(PoweredPlayer);

            writer.Write(LastMoved.Day);

            writer.Write(LastMoved.Hour);

            writer.Write(LastMoved.Minute);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            if (PowerHourCommand.PowerPlayers == null)
            {
                PowerHourCommand.PowerPlayers = new List<PowerHourEntry>();
            }

            PoweredPlayer = reader.ReadMobile();

            int savedDay = reader.ReadInt();
            int savedHour = reader.ReadInt();
            int savedMinute = reader.ReadInt();

            this.LastMoved = new DateTime(
                LastMoved.Year,
                LastMoved.Month,
                savedDay,
                savedHour,
                savedMinute,
                0); 

            PowerHourCommand.PowerPlayers.Add(this);
        }
    }
}

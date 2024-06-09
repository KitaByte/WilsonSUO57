using System.Collections;
using Server.Mobiles;

namespace Server.Custom.DanceSystem
{
    public static class DanceEngine
    {
        public static ArrayList PlayerDanceList { get; private set; } = new ArrayList();

        private static Timer PlayerDanceTimer;

        private static void ValidateTimer()
        {
            if (PlayerDanceTimer == null)
            {
                PlayerDanceTimer = new DanceTimer();
            }
        }

        public static void Initialize()
        {
            EventSink.ServerStarted += EventSink_ServerStarted;

            EventSink.BeforeWorldSave += EventSink_BeforeWorldSave;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;

            EventSink.Movement += EventSink_Movement;
        }

        private static void EventSink_ServerStarted()
        {
            ValidateTimer();

            PlayerDanceTimer.Start();
        }

        private static void EventSink_BeforeWorldSave(BeforeWorldSaveEventArgs e)
        {
            ValidateTimer();

            if (PlayerDanceTimer.Running)
            {
                PlayerDanceTimer.Stop();
            }
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            ValidateTimer();

            if (!PlayerDanceTimer.Running)
            {
                PlayerDanceTimer.Start();
            }
        }

        private static void EventSink_Movement(MovementEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm)
            {
                RemovePlayer(pm);
            }
        }

        public static void AddPlayer(PlayerMobile pm)
        {
            if (!PlayerDanceList.Contains(pm))
            {
                PlayerDanceList.Add(pm);

                pm.SendAsciiMessage(Utility.RandomBrightHue(), "You start dancing!");
            }
        }

        public static void RemovePlayer(PlayerMobile pm)
        {
            if (PlayerDanceList.Contains(pm))
            {
                PlayerDanceList.Remove(pm);

                pm.SendAsciiMessage(Utility.RandomBrightHue(), "You stop dancing!");
            }
        }

        private static ArrayList tempList;

        internal static void ValidatePlayerList()
        {
            if (PlayerDanceList.Count > 0)
            {
                if (tempList == null)
                {
                    tempList = new ArrayList();
                }
                else
                {
                    tempList.Clear();
                }

                for (int i = 0; i < PlayerDanceList.Count; i++)
                {
                    if (PlayerDanceList[i] is PlayerMobile pm)
                    {
                        if (IsValidConditions(pm))
                        {
                            if (pm.Map != Map.Internal)
                            {
                                tempList.Add(pm);
                            }
                        }
                    }
                }

                PlayerDanceList.Clear();

                PlayerDanceList.AddRange(tempList);
            }
        }

        internal static bool IsValidConditions(PlayerMobile pm)
        {
            return pm.Alive && pm.Combatant == null && !pm.Mounted;
        }
    }
}

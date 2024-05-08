using Server.Targeting;
using System;
using System.Collections.Generic;

namespace Server.Custom.MovementSystem
{
    public static class MovementCore
    {
        internal static readonly List<Mobile> MovePlayers = new List<Mobile>();

        private static readonly Dictionary<Mobile, Point3D> PlayerStartLocations = new Dictionary<Mobile, Point3D>();

        internal static void AddPlayerLoc(Mobile mobile)
        {
            if (!PlayerStartLocations.ContainsKey(mobile))
            {
                PlayerStartLocations.Add(mobile, mobile.Location);
            }
        }

        private static void SendPlayerToStart(Mobile from, bool withCorpse = false)
        {
            if (SwimUtility.HasWater(from.Location, from.Map))
            {
                if (PlayerStartLocations.TryGetValue(from, out Point3D location))
                {
                    from.MoveToWorld(location, from.Map);

                    if (withCorpse)
                    {
                        from.Corpse.MoveToWorld(location, from.Map);

                        from.SendMessage(53, "You and your corpse are washed up where you started!");
                    }
                    else
                    {
                        from.SendMessage(53, "You washed up where you started!");
                    }
                }

                SwimUtility.StopSwimming(from);

                return;
            }

            if (ClimbUtility.HasRock(from.Location, from.Map))
            {
                if (PlayerStartLocations.TryGetValue(from, out Point3D location))
                {
                    from.MoveToWorld(location, from.Map);

                    if (withCorpse)
                    {
                        from.Corpse.MoveToWorld(location, from.Map);

                        from.SendMessage(53, "You and your corpse tumbled down to where you started!");
                    }
                    else
                    {
                        from.SendMessage(53, "You tumbled down to where you started!");
                    }
                }

                return;
            }
        }

        internal static void RemovePlayerLoc(Mobile mobile)
        {
            if (PlayerStartLocations.ContainsKey(mobile))
            {
                PlayerStartLocations.Remove(mobile);
            }
        }

        private static MoveTimer Move_Timer;

        public static void Initialize()
        {
            EventSink.Login += EventSink_Login;

            EventSink.Logout += EventSink_Logout;

            EventSink.BeforeWorldSave += EventSink_BeforeWorldSave;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;

            EventSink.PlayerDeath += EventSink_PlayerDeath;

            EventSink.CastSpellRequest += EventSink_CastSpellRequest;

            Move_Timer = new MoveTimer();

            Move_Timer.Start();
        }

        private static void EventSink_CastSpellRequest(CastSpellRequestEventArgs e)
        {
            if (!e.Mobile.Flying && MovePlayers.Contains(e.Mobile))
            {
                if ((!MoveSettings.SwimCasting && SwimUtility.HasWater(e.Mobile.Location, e.Mobile.Map)))
                {
                    e.Mobile.Mana = 0;

                    Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                    {
                        Target.Cancel(e.Mobile);

                        e.Mobile.SendMessage(43, "Hands are busy swimming, mana drained!");
                    });
                }

                if ((!MoveSettings.ClimbCasting && ClimbUtility.HasRock(e.Mobile.Location, e.Mobile.Map)))
                {
                    e.Mobile.Mana = 0;

                    Timer.DelayCall(TimeSpan.FromSeconds(2), () =>
                    {
                        Target.Cancel(e.Mobile);

                        e.Mobile.SendMessage(43, "Hands are busy climbing, mana drained!");
                    });
                }
            }
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            if (SwimUtility.HasWater(e.Mobile.Location, e.Mobile.Map))
            {
                SwimUtility.StartSwimming(e.Mobile);
            }

            if (!MovePlayers.Contains(e.Mobile) && MoveSettings.AutoLogPlayer)
            {
                MovePlayers.Add(e.Mobile);
            }
        }

        private static void EventSink_Logout(LogoutEventArgs e)
        {
            if (MovePlayers.Contains(e.Mobile))
            {
                MovePlayers.Remove(e.Mobile);
            }
        }

        private static void EventSink_BeforeWorldSave(BeforeWorldSaveEventArgs e)
        {
            Move_Timer.Stop();
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            Move_Timer.Start();
        }

        private static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
        {
            SendPlayerToStart(e.Mobile, true);
        }
    }
}

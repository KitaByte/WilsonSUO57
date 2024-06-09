using System;
using System.Linq;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Engines.PartySystem;

namespace Server.Custom.LFGSystem
{
    internal static class LFGCore
    {
        private static readonly List<Mobile> LFGList = new List<Mobile>();

        private static readonly List<Mobile> DNDList = new List<Mobile>();

        private static bool _IsActive = false;

        private static bool _IsStarting = false;

        internal static int GetCount()
        {
            return LFGList.Count;
        }

        internal static bool HasPlayer(Mobile from)
        {
            return LFGList.Contains(from);
        }

        internal static void UpdateWaitingPlayers(Mobile from)
        {
            if (GetCount() > 1)
            {
                foreach (var player in LFGList)
                {
                    if (player != from)
                    {
                        player.SendMessage(53, $"Player joined, group now at {GetCount()} players!");

                        player.SendMessage(53, "Use [StartLFG when two or more players join!");

                        player.SendMessage(53, "LFG auto starts when eight players have joined!");
                    }
                }
            }
        }

        internal static bool AddParty(Mobile from)
        {
            if (!_IsStarting && !LFGList.Contains(from))
            {
                LFGList.Add(from);

                if (LFGList.Count == 1)
                {
                    UpdateBroadcast();
                }

                CheckFull();

                return true;
            }

            return false;
        }

        private static void CheckFull()
        {
            if (LFGList.Count == 8)
            {
                StartParty();
            }
        }

        internal static bool RemoveParty(Mobile from)
        {
            if (!_IsStarting && LFGList.Contains(from))
            {
                LFGList.Remove(from);

                if (LFGList.Count == 0)
                {
                    UpdateBroadcast();
                }

                return true;
            }

            return false;
        }

        private static void UpdateBroadcast()
        {
            if (LFGList.Count > 0)
            {
                _IsActive = true;

                Timer.DelayCall(TimeSpan.FromMinutes(5), () =>
                {
                    if (!_IsStarting && _IsActive)
                    {
                        var playersList = World.Mobiles.Values.Where(m => m is PlayerMobile pm && IsValid(pm)).ToList();

                        if (playersList != null && playersList.Count > 0)
                        {
                            foreach (var player in playersList)
                            {
                                player.SendMessage(Utility.RandomBrightHue(), "Players looking for group, use [LFG to join!");
                            }
                        }

                        UpdateBroadcast();
                    }
                });
            }
            else
            {
                _IsActive = false;
            }
        }

        private static bool IsValid(PlayerMobile pm)
        {
            if (HasPlayer(pm))
            {
                return false;
            }

            if (DNDList.Contains(pm))
            {
                return false;
            }

            return true;
        }

        internal static void StartParty()
        {
            if (LFGList.Count > 1)
            {
                _IsStarting = true;

                Party party = new Party(LFGList[0]);

                LFGList[0].SendMessage(63, "You will lead the group!");

                for (int i = 0; i < 8; i++)
                {
                    if (LFGList.Count > i)
                    {
                        LFGList[i].MoveToWorld(new Point3D(1432, 1718, 20), Map.Trammel);

                        if (LFGList[i] is PlayerMobile pm && pm.Followers > 0)
                        {
                            pm.AutoStablePets();

                            pm.SendMessage(53, "Your pets were stabled!");
                        }

                        if (i > 0)
                        {
                            Party.Invite(LFGList[0], LFGList[i]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                foreach (var info in party.Members)
                {
                    info.Mobile.SendMessage(63, "Have Fun!");
                }

                LFGList.Clear();

                UpdateBroadcast();
            }
            else
            {
                if (LFGList.Count == 1)
                {
                    LFGList[0].SendMessage(43, "Need at least two players before LFG starts!");
                }
            }

            _IsStarting = false;
        }

        internal static void AddRemoveDNDList(Mobile from)
        {
            if (!DNDList.Contains(from))
            {
                DNDList.Add(from);

                from.SendMessage(63, "You were added to the 'Do Not Disturb' list!");
            }
            else
            {
                DNDList.Remove(from);

                from.SendMessage(63, "You were removed from the 'Do Not Disturb' list!");
            }
        }
    }
}

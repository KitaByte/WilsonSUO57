using System;
using System.Linq;
using Server.Items;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Custom.UOSurvivalEvent.Islands;
using Server.Custom.UOSurvivalEvent.Commands;
using Server.Gumps;

namespace Server.Custom.UOSurvivalEvent
{
    public enum SurvivalGameStage
    {
        Ready,
        Waiting,
        Running,
        Paused,
        Ending,
        Resetting
    }

    internal static class SurvivalCore
    {
        internal static SurvivalGameStage GameStage { get; private set; } = SurvivalGameStage.Ready;

        internal static List<SurvivalPlayer> PlayerLobby { get; private set; } = new List<SurvivalPlayer>();

        internal static bool GameRunning { get { return GameStage == SurvivalGameStage.Running; } }

        internal static Rectangle2D GameBounds { get; set; }

        internal static List<Static> BoundryList { get; set; } = new List<Static>();

        internal static SurvivalTeam TeamOne { get; set; }

        internal static SurvivalTeam TeamTwo { get; set; }

        internal static SurvivalTeam Winner { get; set; }

        internal const int TotemDeactivation = 10;

        internal static List<int> MaskTiles = new List<int>() { 0x050D, 0x050E, 0x050F, 0x0510, 0x0511, 0x0512, 0x0513, 0x0514 };

        public static void Initialize()
        {
            EventSink.Movement += EventSink_Movement;

            EventSink.PlayerDeath += EventSink_PlayerDeath;

            EventSink.Logout += EventSink_Logout;

            EventSink.BeforeWorldSave += EventSink_BeforeWorldSave;

            EventSink.AfterWorldSave += EventSink_AfterWorldSave;

            EventSink.Crashed += EventSink_Crashed;

            EventSink.Shutdown += EventSink_Shutdown;
        }

        internal static void AddToLobby(Mobile m)
        {
            if (PlayerLobby != null)
            {
                if (GameStage == SurvivalGameStage.Ready)
                {
                    GameStage = SurvivalGameStage.Waiting;
                }

                PlayerLobby.Add(new SurvivalPlayer(m));

                GiveInstructions(m);
            }
        }

        internal static void RemoveFromLobby(PlayerMobile user)
        {
            var playerInfo = PlayerLobby.Find(i => i.S_Player == user);

            if (playerInfo != null)
            {
                PlayerLobby.Remove(playerInfo);

                playerInfo.S_Player.SendMessage(53, "You were removed from the next match!");
            }

            if (PlayerLobby.Count == 0)
            {
                EndMatch();
            }
        }

        internal static void SendGump()
        {
            foreach (var playerInfo in PlayerLobby)
            {
                if (playerInfo.S_Player.HasGump(typeof(SurvivalGump)))
                {
                    playerInfo.S_Player.CloseGump(typeof(SurvivalGump));
                }

                BaseGump.SendGump(new SurvivalGump(playerInfo.S_Player as PlayerMobile));
            }
        }

        private static void GiveInstructions(Mobile m)
        {
            m.SendMessage(53, "Welcome to the Survival Island Event!");
            m.SendMessage(53, "This is a pvp style event!");
            m.SendMessage(53, "You'll be given a boat deed!");
            m.SendMessage(53, "Use boat to travel to other teams island!");
            m.SendMessage(53, "Win by deactivating the other teams Totem!");
            m.SendMessage(53, "Stand next to the Totem until it deactivates!");
            m.SendMessage(53, "Have Fun!");
        }

        internal static void SetUpMatch()
        {

            TeamOne = new SurvivalTeam();

            TeamTwo = new SurvivalTeam();

            for (int i = 0; i < PlayerLobby.Count; i++)
            {
                if (TeamTwo.TeamPlayers.Count >= TeamOne.TeamPlayers.Count)
                {
                    TeamOne.AddPlayer(PlayerLobby[i]);
                }
                else
                {
                    TeamTwo.AddPlayer(PlayerLobby[i]);
                }

                PlayerLobby[i].S_Player.SendMessage(53, "Game Starting!!!");

                PlayerLobby[i].S_Player.PlaySound(0x5B4);
            }

            var sizeBase = 10 * (TeamOne.TeamPlayers.Count > 5 ? 5 : TeamOne.TeamPlayers.Count) + 1;

            int StartPoint = 3450;

            Point2D teamOneStart = new Point2D(StartPoint, StartPoint + 300);

            Point2D teamTwoStart = new Point2D(StartPoint + 100, StartPoint + 300);

            GameBounds = new Rectangle2D
                (
                    new Point2D(teamOneStart.X - sizeBase, teamOneStart.Y - sizeBase),
                    new Point2D(teamTwoStart.X + (sizeBase * 2), teamTwoStart.Y + (sizeBase * 2))
                );

            SurvivalUtility.SpawnGameBoundry();

            IslandType biome = (IslandType)Utility.Random(Enum.GetValues(typeof(IslandType)).Length - 1);

            // Team One Island
            TeamOne.AddIslandLocation(new Rectangle2D(teamOneStart, new Point2D(teamOneStart.X + sizeBase, teamOneStart.Y + sizeBase)));

            TeamOne.SpawnIsland(Map.Felucca, biome);

            // Team Two Island
            TeamTwo.AddIslandLocation(new Rectangle2D(teamTwoStart, new Point2D(teamTwoStart.X + sizeBase, teamTwoStart.Y + sizeBase)));

            if (Utility.RandomDouble() < 0.1)
            {
                TeamTwo.SpawnIsland(Map.Felucca, (IslandType)Utility.Random(Enum.GetValues(typeof(IslandType)).Length - 1));
            }
            else
            {
                TeamTwo.SpawnIsland(Map.Felucca, biome);
            }

            StartMatch();
        }

        internal static void StartMatch()
        {
            if (TeamOne != null && TeamTwo != null)
            {
                TeamOne.MoveToIsland();

                TeamTwo.MoveToIsland();

                PlayerLobby.Clear();

                GameStage = SurvivalGameStage.Running;

                Console.WriteLine($"Survival Event - Started : {DateTime.Now.ToShortTimeString()}");
            }
        }

        internal static void JoinLate(Mobile m)
        {
            SurvivalPlayer sp = new SurvivalPlayer(m);

            if (TeamOne.TeamPlayers.Count > TeamTwo.TeamPlayers.Count)
            {
                TeamTwo.AddPlayer(sp);

                TeamTwo.LateMove(sp);
            }
            else
            {
                TeamOne.AddPlayer(new SurvivalPlayer(m));

                TeamOne.LateMove(sp);
            }
        }

        private static void EventSink_Movement(MovementEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm && GameRunning)
            {
                if (TeamOne.HasPlayer(pm))
                {
                    if (GameBounds.Contains(pm.Location))
                    {
                        Timer.DelayCall(TimeSpan.FromMilliseconds(100), () =>
                        {
                            if (GameRunning && TeamTwo != null && TeamTwo.TeamIsland != null && TeamTwo.TeamIsland.IslandEntity != null)
                            {
                                TeamTwo.TeamIsland.IslandEntity.CheckIslandMove(TeamOne, TeamTwo, pm);
                            }
                        });
                    }
                    else
                    {
                        TeamOne.RemovePlayer(pm);

                        pm.SendMessage(53, "You were removed from the game!");
                    }
                }
                else if (TeamTwo.HasPlayer(pm))
                {
                    if (GameBounds.Contains(pm.Location))
                    {
                        Timer.DelayCall(TimeSpan.FromMilliseconds(100), () =>
                        {
                            if (GameRunning && TeamOne != null && TeamOne.TeamIsland != null && TeamOne.TeamIsland.IslandEntity != null)
                            {
                                TeamOne.TeamIsland.IslandEntity.CheckIslandMove(TeamTwo, TeamOne, pm);
                            }
                        });
                    }
                    else
                    {
                        TeamTwo.RemovePlayer(pm);

                        pm.SendMessage(53, "You were removed from the game!");
                    }
                };
            }
        }

        private static void EventSink_PlayerDeath(PlayerDeathEventArgs e)
        {
            if (GameRunning)
            {
                var killer = TeamOne.GetPlayerInfo(e.Killer);

                if (killer != null)
                {
                    var victim = TeamTwo.GetPlayerInfo(e.Mobile);

                    if (victim != null)
                    {
                        UpdatePlayerStats(killer, victim);
                    }
                }
                else
                {
                    killer = TeamTwo.GetPlayerInfo(e.Killer);

                    if (killer != null)
                    {
                        var victim = TeamOne.GetPlayerInfo(e.Mobile);

                        if (victim != null)
                        {
                            UpdatePlayerStats(killer, victim);
                        }
                    }
                }
            }
        }

        private static void EventSink_Logout(LogoutEventArgs e)
        {
            if (TeamOne != null && TeamOne.HasPlayer(e.Mobile))
            {
                TeamOne.RemovePlayer(e.Mobile);

                RemoveBoat(e.Mobile);
            }
            else if (TeamTwo != null && TeamTwo.HasPlayer(e.Mobile))
            {
                TeamOne.RemovePlayer(e.Mobile);

                RemoveBoat(e.Mobile);
            }

            if (GameRunning && TeamOne?.TeamPlayers.Count == 0 && TeamTwo?.TeamPlayers.Count == 0)
            {
                EndMatch();
            }
        }

        private static void EventSink_BeforeWorldSave(BeforeWorldSaveEventArgs e)
        {
            if (GameRunning)
            {
                GameStage = SurvivalGameStage.Paused;

                TeamOne?.FreezeAllPlayers();

                TeamTwo?.FreezeAllPlayers();
            }
        }

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            if (GameStage == SurvivalGameStage.Paused)
            {
                TeamOne?.UnFreezeAllPlayers();

                TeamTwo?.UnFreezeAllPlayers();

                GameStage = SurvivalGameStage.Running;
            }
        }

        private static void EventSink_Crashed(CrashedEventArgs e)
        {
            if (GameRunning)
            {
                EndMatch();
            }
        }

        private static void EventSink_Shutdown(ShutdownEventArgs e)
        {
            if (GameRunning)
            {
                EndMatch();
            }
        }

        private static void UpdatePlayerStats(SurvivalPlayer killer, SurvivalPlayer victim, bool isEnd = false)
        {
            if (isEnd)
            {
                // Kill Bonus
                for (int i = 0; i < killer.Kills; i++) 
                {
                    killer.UpdateStats(false, false, true);
                }

                killer.UpdateStats(false, false, true);
            }
            else
            {
                killer.UpdateStats(true, false, true);

                victim.UpdateStats(false, true, false);
            }
        }

        internal static void RemoveBoat(Mobile m)
        {
            var boat = m.Backpack.FindItemByType(typeof(SmallBoatDeed));

            boat?.Delete();
        }

        internal static void SendTotemMsg(SurvivalTeam team, string message)
        {
            team.TeamTotem.PublicOverheadMessage(MessageType.Yell, 53, false, message);

            SurvivalUtility.PlayTotemEffect(team.TeamTotem.Map, team.TeamTotem.Location, team == TeamOne);
        }

        private static void DespawnBoundry()
        {
            if (BoundryList.Count > 0)
            {
                for (int i = 0; i < BoundryList.Count; i++)
                {
                    BoundryList[i].Delete();
                }

                BoundryList.Clear();
            }
        }

        internal static void EndMatch()
        {
            GameStage = SurvivalGameStage.Ending;

            TeamOne?.PlayEndGameSound();

            TeamTwo?.PlayEndGameSound();

            if (Winner != null)
            {
                for (int i = 0; i < Winner.TeamPlayers.Count; i++)
                {
                    UpdatePlayerStats(Winner.TeamPlayers[i], null, true);
                }

                Winner.IsWinner = true;
            }

            TeamOne?.MoveToHome();

            TeamTwo?.MoveToHome();

            TeamOne?.DespawnIsland();

            TeamOne?.TeamTotem.Delete();

            TeamTwo?.DespawnIsland();

            TeamTwo?.TeamTotem.Delete();

            DespawnBoundry();

            CleanSurvivalZone();

            GameStage = SurvivalGameStage.Resetting;

            if (SurvivalSettings.IsAutoOff)
            {
                SurvivalSettings.SurvivalEnabled = false;
            }

            TeamOne = null;

            TeamTwo = null;

            Winner = null;

            Timer.DelayCall(TimeSpan.FromSeconds(SurvivalSettings.MatchDelay), () =>
            {
                ResetGame();

                Console.WriteLine($"Survival Event - Reset : {DateTime.Now.ToShortTimeString()}");
            });
        }

        private static void CleanSurvivalZone()
        {
            if (GameBounds.Start != Point2D.Zero && GameBounds.End != Point2D.Zero)
            {
                var garbage = Map.Felucca.GetObjectsInBounds(GameBounds).ToList();

                for (int i = 0; i < garbage.Count; i++)
                {
                    if (garbage[i] is BaseBoat boat)
                    {
                        if (boat.IsClassicBoat)
                        {
                            boat.RemoveKeys(boat.Owner);
                        }
                    }

                    if (garbage[i] is Item)
                    {
                        garbage[i].Delete();
                    }
                }
            }
        }

        private static void ResetGame()
        {
            GameStage = SurvivalGameStage.Ready;
        }
    }
}

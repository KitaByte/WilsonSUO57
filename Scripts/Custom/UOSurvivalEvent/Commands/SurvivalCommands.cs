using Server.Commands;
using Server.Custom.UOSurvivalEvent.Islands;

namespace Server.Custom.UOSurvivalEvent.Commands
{
    internal class SurvivalCommands
    {

        public static void Initialize()
        {
            CommandSystem.Register("TestSurvival", AccessLevel.Administrator, new CommandEventHandler(TestSurvival_OnCommand));
            CommandSystem.Register("ToggleSurvival", AccessLevel.GameMaster, new CommandEventHandler(ToggleSurvival_OnCommand));
            CommandSystem.Register("StaffStartSurvival", AccessLevel.GameMaster, new CommandEventHandler(StaffStartSurvival_OnCommand));
            CommandSystem.Register("StartSurvival", AccessLevel.Player, new CommandEventHandler(StartSurvival_OnCommand));
        }

        [Usage("TestSurvival <type>")]
        [Description("Test Survival: ON/OFF")]
        private static void TestSurvival_OnCommand(CommandEventArgs e)
        {
            if (SurvivalCore.GameRunning)
            {
                SurvivalCore.EndMatch();

                e.Mobile.SendMessage(53, "Test Game => Stopped!");
            }
            else
            {
                var biome = IslandType.Default;

                if (e.Arguments.Length > 0)
                {
                    if (!string.IsNullOrEmpty(e.Arguments[0]))
                    {
                        switch (e.Arguments[0].ToLower())
                        {
                            case "grass":
                                {
                                    biome = IslandType.Grass;

                                    break;
                                }

                            case "forest":
                                {
                                    biome = IslandType.Forest;

                                    break;
                                }

                            case "desert":
                                {
                                    biome = IslandType.Desert;

                                    break;
                                }

                            case "snow":
                                {
                                    biome = IslandType.Snow;

                                    break;
                                }

                            case "swamp":
                                {
                                    biome = IslandType.Swamp;

                                    break;
                                }

                            case "fire":
                                {
                                    biome = IslandType.Fire;

                                    break;
                                }

                            case "blood":
                                {
                                    biome = IslandType.Blood;

                                    break;
                                }
                        }
                    }
                }

                var loc = e.Mobile.Location;

                SurvivalCore.GameBounds = new Rectangle2D(new Point2D(loc.X - 40, loc.Y - 40), new Point2D(loc.X + 40, loc.Y + 40));

                SurvivalCore.TeamOne = new SurvivalTeam();

                SurvivalCore.TeamOne.AddPlayer(new SurvivalPlayer(e.Mobile));

                SurvivalCore.TeamOne.AddIslandLocation(new Rectangle2D(new Point2D(loc.X - 40, loc.Y - 40), loc));

                SurvivalCore.TeamOne.SpawnIsland(e.Mobile.Map, biome);

                SurvivalCore.TeamTwo = new SurvivalTeam();

                SurvivalCore.TeamTwo.AddIslandLocation(new Rectangle2D(loc, new Point2D(loc.X + 40, loc.Y + 40)));

                SurvivalCore.TeamTwo.SpawnIsland(e.Mobile.Map, biome);

                SurvivalCore.StartMatch();

                e.Mobile.SendMessage(62, "Test Game => Started!");
            }
        }

        [Usage("ToggleSurvival <bool>?")]
        [Description("Toggle Survival Event On/Off : Optional AutoOff On/Off")]
        private static void ToggleSurvival_OnCommand(CommandEventArgs e)
        {
            if (e.Arguments.Length > 0)
            {
                if (bool.TryParse(e.Arguments[0], out bool isAuto))
                {
                    SurvivalSettings.IsAutoOff = isAuto;

                    SendSurvivalMessage(e.Mobile);
                }
                else
                {
                    e.Mobile.SendMessage(53, "ToggleSurvival <bool>");
                }
            }
            else
            {
                if (SurvivalCore.GameStage == SurvivalGameStage.Ready)
                {
                    SurvivalSettings.SurvivalEnabled = !SurvivalSettings.SurvivalEnabled;

                    SendSurvivalMessage(e.Mobile);

                    if (SurvivalSettings.SurvivalEnabled)
                    {
                        BroadCastEvent();
                    }
                }
                else
                {
                    e.Mobile.SendMessage(53, "Please wait for event to end!");
                }
            }
        }

        [Usage("StaffStartSurvival")]
        [Description("Staff Start Survival Event")]
        private static void StaffStartSurvival_OnCommand(CommandEventArgs e)
        {
            if (SurvivalSettings.StaffOnlyStart && SurvivalCore.GameStage == SurvivalGameStage.Waiting)
            {
                if (SurvivalCore.PlayerLobby.Count >= SurvivalSettings.MinPlayers)
                {
                    SurvivalCore.SetUpMatch();

                    e.Mobile.SendMessage(53, "Match Started!");
                }
                else
                {
                    e.Mobile.SendMessage(53, "Not enough players to start!");
                }
            }
        }

        [Usage("StartSurvival")]
        [Description("Start Survival Event")]
        private static void StartSurvival_OnCommand(CommandEventArgs e)
        {
            if (SurvivalSettings.SurvivalEnabled)
            {
                switch (SurvivalCore.GameStage)
                {
                    case SurvivalGameStage.Ready:
                        {
                            e.Mobile.SendMessage(53, $"Starting Survival Setup, waiting for players, min {SurvivalSettings.MinPlayers} needed!");

                            SurvivalCore.AddToLobby(e.Mobile);

                            SurvivalCore.SendGump();

                            BroadCastEvent();

                            break;
                        }
                    case SurvivalGameStage.Waiting:
                        {
                            e.Mobile.SendMessage(53, "You have been added to the event starting soon!");

                            SurvivalCore.AddToLobby(e.Mobile);

                            SurvivalCore.SendGump();

                            break;
                        }
                    case SurvivalGameStage.Running:
                        {
                            if (SurvivalSettings.CanJoinLate)
                            {
                                e.Mobile.SendMessage(53, "There is a match in progress, you'll be added to a team!");

                                SurvivalCore.JoinLate(e.Mobile);

                                break;
                            }
                            else
                            {
                                e.Mobile.SendMessage(53, "There is a match in progress, please try again later!"); break;
                            }
                        }
                    case SurvivalGameStage.Ending:
                        {
                            e.Mobile.SendMessage(53, "There is a match ending, please try again later!"); break;
                        }
                    case SurvivalGameStage.Resetting:
                        {
                            e.Mobile.SendMessage(53, "The last match is just cleaning up, try in a few minutes!"); break;
                        }
                }
            }
            else
            {
                e.Mobile.SendMessage(53, "Survival Event is not enabled at this time!");
            }
        }

        private static void SendSurvivalMessage(Mobile from)
        {
            from.SendMessage(53, $"Survival Event Enabled = {SurvivalSettings.SurvivalEnabled} : AutoOff = {SurvivalSettings.IsAutoOff}");
        }

        private static void BroadCastEvent()
        {
            World.Broadcast(53, true, "Survival Event Activated - Use [StartSurvival to join!");
        }
    }
}

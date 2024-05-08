using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.UOSurvivalEvent
{
    internal class SurvivalGump : BaseGump
    {
        public SurvivalGump(PlayerMobile user) : base(user, 50, 50, null)
        {
        }

        public override void AddGumpLayout()
        {
            Closable = false;
            Resizable = false;
            Dragable = true;

            AddBackground(X, Y, 150, SurvivalCore.PlayerLobby.Count * 25 + 130, 40000);

            AddLabel(X + 30, Y + 12, 2720, "Survival Islands");

            AddButton(X + 25, Y + 41, 2360, 2361, 1, GumpButtonType.Reply, 0);
            AddLabel(X + 45, Y + 37, 2499, "Start Match");

            AddButton(X + 25, Y + 71, 2361, 2360, 2, GumpButtonType.Reply, 0);
            AddLabel(X + 45, Y + 67, 2499, "Leave Match");

            AddLabel(X + 25, Y + 97, 43, "Current Players");

            int y = Y + 97;

            foreach (var player in SurvivalCore.PlayerLobby)
            {
                y += 25;

                AddLabel(X + 25, y, 62, player.S_Player.Name);
            }
        }

        public override void OnResponse(RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 1: // Start
                    {
                        if (SurvivalCore.PlayerLobby.Count >= SurvivalSettings.MinPlayers)
                        {
                            if (!SurvivalSettings.StaffOnlyStart)
                            {
                                SurvivalCore.SetUpMatch();
                            }
                        }
                        else
                        {
                            User.SendMessage(53, "Not enough players to start match!");

                            Refresh(true, false);
                        }

                        break;
                    }

                case 2: // Cancel
                    {
                        SurvivalCore.RemoveFromLobby(User);

                        Close();

                        break;
                    }

                default:
                    {
                        Close();

                        break;
                    }
            }
        }
    }
}

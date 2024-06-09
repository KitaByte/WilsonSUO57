using Server.Commands;

namespace Server.Custom.LFGSystem
{
    public static class LFGCommands
    {
        public static void Initialize()
        {
            CommandSystem.Register("LFG", AccessLevel.Administrator, new CommandEventHandler(LFG_OnCommand));
            CommandSystem.Register("StartLFG", AccessLevel.Administrator, new CommandEventHandler(StartLFG_OnCommand));
            CommandSystem.Register("LFGDND", AccessLevel.Administrator, new CommandEventHandler(LFGDND_OnCommand));
        }

        [Usage("LFG")]
        [Description("LFG: Add/Remove")]
        private static void LFG_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile.Party != null)
            {
                e.Mobile.SendMessage(33, "You are already in a party!");

                return;
            }

            if (LFGCore.AddParty(e.Mobile))
            {
                e.Mobile.SendMessage(53, "You are added to LFG list!");

                if (LFGCore.GetCount() == 1)
                {
                    e.Mobile.SendMessage(53, $"You are the only player waiting!");
                }
                else
                {
                    e.Mobile.SendMessage(53, $"There are currently {LFGCore.GetCount()} players waiting!");

                    e.Mobile.SendMessage(53, "Use [StartLFG when two or more players join!");

                    e.Mobile.SendMessage(53, "LFG auto starts when eight players have joined!");
                }

                LFGCore.UpdateWaitingPlayers(e.Mobile);
            }
            else
            {
                if (LFGCore.RemoveParty(e.Mobile))
                {
                    e.Mobile.SendMessage(53, "You are removed from the LFG list!");
                }
            }
        }

        [Usage("StartLFG")]
        [Description("LFG: Start")]
        private static void StartLFG_OnCommand(CommandEventArgs e)
        {
            if (LFGCore.HasPlayer(e.Mobile))
            {
                LFGCore.StartParty();
            }
        }

        [Usage("LFGDND")]
        [Description("LFG DND: Add/Remove")]
        private static void LFGDND_OnCommand(CommandEventArgs e)
        {
            LFGCore.AddRemoveDNDList(e.Mobile);
        }
    }
}

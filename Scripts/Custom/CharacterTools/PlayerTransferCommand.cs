using Server.Misc;

namespace Server.Commands
{
    internal class PlayerTransferCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("TransferChar", AccessLevel.Administrator, new CommandEventHandler(MoveChar_OnCommand));
        }

        [Usage("TransferChar <charName> <sourceAccount> <targetAccount>")]
        [Description("Transfers a character from one account to another.")]
        public static void MoveChar_OnCommand(CommandEventArgs e)
        {
            if (e.Arguments.Length != 3)
            {
                e.Mobile.SendMessage("Usage: TransferChar <charName> <sourceAccount> <targetAccount>");

                return;
            }

            string charName = e.Arguments[0];

            string sourceAcc = e.Arguments[1];

            string targetAcc = e.Arguments[2];

            if (PlayerTransferCore.ExecuteTransfer(e.Mobile, charName, sourceAcc, targetAcc))
            {
                e.Mobile.SendMessage(63, $"Character {charName} moved from {sourceAcc} to {targetAcc}.");
            }
        }
    }
}

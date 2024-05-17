using System.Collections.Generic;
using Server.Accounting;
using Server.Mobiles;
using System.Linq;

namespace Server.Misc
{
    public static class PlayerTransferCore
    {
        public static bool ExecuteTransfer(Mobile from, string charName, string sourceAcc, string targetAcc)
        {
            var source = Accounts.GetAccount(sourceAcc);

            var target = Accounts.GetAccount(targetAcc);

            if (target.Count > 5)
            {
                from.SendMessage(33, "Target is at the maximum number of characters and cannot transfer more.");

                return false;
            }

            if (CharacterOnline(sourceAcc, charName))
            {
                from.SendMessage(53, $"{charName} must be logged out before transfer is completed.");

                return false;
            }

            if (source != null && target != null)
            {
                for (int i = 0; i < source.Count; i++)
                {
                    if (source[i].Name == charName)
                    {
                        if (target.Count < 6)
                        {
                            target[target.Count] = source[i];

                            source[i] = null;

                            ReorganizeCharacters(source);

                            return true;
                        }
                    }
                }
            }

            from.SendMessage(53, $"Error : {charName} was not transfered.");

            return false;
        }

        private static bool CharacterOnline(string account, string name)
        {
            var playerList = World.Mobiles.Values.OfType<PlayerMobile>().ToList();

            return playerList.Any(p => p.Account.Username == account && p.Name == name && p.Map != Map.Internal);
        }

        private static void ReorganizeCharacters(IAccount account)
        {
            var mobs = new List<Mobile>();

            for (int i = 0; i < 6; i++)
            {
                if (account[i] != null)
                {
                    mobs.Add(account[i]);
                }
            }

            for (int j = 0; j < mobs.Count; j++)
            {
                if (j < mobs.Count)
                {
                    account[j] = mobs[j];
                }
                else
                {
                    account[j] = null;
                }
            }
        }
    }
}

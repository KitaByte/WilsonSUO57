using Server.Commands;
using Server.Items;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Services.UOBlackBox
{
    public static class BoxControl
    {
        public static void Initialize()
        {
            CommandSystem.Register("ReloadBoxArt", AccessLevel.Owner, ReloadArt_OnCommand);
            CommandSystem.Register("ResetBoxData", AccessLevel.Owner, ResetData_OnCommand);
            CommandSystem.Register("BoxDebug", AccessLevel.Owner, Debug_OnCommand);
        }

        [Usage("ReloadBoxArt")]
        [Description("Reloads UO Black Box Art")]
        private static void ReloadArt_OnCommand(CommandEventArgs e)
        {
            if (e.Mobile != null && e.Mobile is PlayerMobile pm && ArtCore.GetTotalCount() > 0)
            {
                pm.CloseAllGumps();

                pm.SendMessage(62, "Started Reload Art Process...");

                ArtCore.ReloadArt();

                CommandSystem.Handle(pm, $"{CommandSystem.Prefix}Restart");
            }
        }

        [Usage("ResetBoxData")]
        [Description("UO Black Box Data Reset")]
        private static void ResetData_OnCommand(CommandEventArgs e)
        {
            GameInfo.ClearAllData();

            e.Mobile.SendMessage(62, "Data Cleared!");
        }

        [Usage("BoxDebug <bool>")]
        [Description("UO Black Box Art Debug")]
        private static void Debug_OnCommand(CommandEventArgs e)
        {
            if (e.Arguments.Length > 0)
            {
                if (e.Arguments[0].ToLower() == "true")
                {
                    BoxCore.ShowInfo = true;
                }
                else
                {
                    BoxCore.ShowInfo = false;
                }
            }
        }
    }
}

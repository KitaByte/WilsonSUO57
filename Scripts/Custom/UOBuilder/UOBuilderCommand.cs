using System.Linq;
using Server.Gumps;
using Server.Mobiles;
using Server.Commands;
using Server.Commands.Generic;
using System;

namespace Server.Custom.UOBuilder
{
    internal class UOBuilderCommand : BaseCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ToggleUOBuild", AccessLevel.Administrator, new CommandEventHandler(ToggleUOBuilder_OnCommand));
            CommandSystem.Register("ViewUOBuilds", AccessLevel.Administrator, new CommandEventHandler(ViewUOBuilder_OnCommand));
            CommandSystem.Register("CommitUOBuild", AccessLevel.Administrator, new CommandEventHandler(CommitUOBuilder_OnCommand));
            CommandSystem.Register("RemoveUOBuild", AccessLevel.Administrator, new CommandEventHandler(RemoveUOBuilder_OnCommand));
            CommandSystem.Register("ResetUOBuild", AccessLevel.Player, new CommandEventHandler(ResetUOBuilder_OnCommand));
        }

        [Usage("ToggleUOBuild")]
        [Description("UO Builder - Toggle On/Off")]
        private static void ToggleUOBuilder_OnCommand(CommandEventArgs e)
        {
            UOBuilderCore.UOBuilderActive = !UOBuilderCore.UOBuilderActive;

            e.Mobile.SendMessage(53, $"UO Builder Active = {UOBuilderCore.UOBuilderActive}");
        }

        [Usage("ViewUOBuilds")]
        [Description("UO Builder - View Active Builds")]
        private static void ViewUOBuilder_OnCommand(CommandEventArgs e)
        {
            if (UOBuilderCore.HasBuilds())
            {
                BaseGump.SendGump(new UOBuilderAdminGump(e.Mobile as PlayerMobile));
            }
        }

        [Usage("CommitUOBuild")]
        [Description("UO Builder - Commit Active Build")]
        private static void CommitUOBuilder_OnCommand(CommandEventArgs e)
        {
            var convertList = World.Items.Values.Where(i => i is UOBuilderStatic && i.BlessedFor == e.Mobile)?.ToList();

            if (convertList != null && convertList.Count > 0)
            {
                for (int i = convertList.Count - 1; i >= 0; i--)
                {
                    if (convertList[i] is UOBuilderStatic uobs)
                    {
                        uobs.ConvertStatic();
                    }
                }

                UOBuilderCore.RemoveBuild(UOBuilderCore.LastSerialPlaced);

                UOBuilderCore.CleanBuild(e.Mobile);

                e.Mobile.SendMessage(53, "Build Converted, [Freeze to commit to Map File!");
            }
            else
            {
                e.Mobile.SendMessage(53, "Build Not Converted!");
            }
        }

        [Usage("RemoveUOBuild")]
        [Description("UO Builder - Remove Active Build")]
        private static void RemoveUOBuilder_OnCommand(CommandEventArgs e)
        {
            if (UOBuilderCore.LastSerialPlaced != 0)
            {
                UOBuilderCore.RemoveBuild(UOBuilderCore.LastSerialPlaced);

                UOBuilderCore.CleanBuild(e.Mobile);

                e.Mobile.SendMessage(53, "Build was removed!");
            }
            else
            {
                e.Mobile.SendMessage(53, "No Build Active!");
            }
        }

        [Usage("ResetUOBuild")]
        [Description("UO Builder - Reset Active Build")]
        private static void ResetUOBuilder_OnCommand(CommandEventArgs e)
        {
            var permit = e.Mobile.Backpack.FindItemByType(typeof(UOBuilderPermit));

            if (permit != null && permit is UOBuilderPermit uobp)
            {
                uobp.Center = Point3D.Zero;

                UOBuilderCore.RemoveBuild(e.Mobile.Serial);

                UOBuilderCore.CleanBuild(e.Mobile);

                e.Mobile.SendMessage(53, "Your build was reset!");
            }
            else
            {
                e.Mobile.SendMessage(53, "Building Permit must be in pack in order to reset build!");
            }
        }
    }
}

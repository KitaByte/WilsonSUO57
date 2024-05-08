using System.Collections.Generic;
using Server.Mobiles;
using Server.Gumps;

namespace Server.Custom.UOBuilder
{
    internal class UOBuilderAdminGump : BaseGump
    {
        private List<Serial> PlayerSerials;

        public UOBuilderAdminGump(PlayerMobile user) : base(user, 50, 50, null)
        {
            user.SendMessage(53, "UOBuilder - While Viewing Build!");
            user.SendMessage(43, "Use [CommitUOBuild - to commit build!");
            user.SendMessage(33, "Use [RemoveUOBuild - to delete build!");
        }

        public override void AddGumpLayout()
        {
            Closable = true;
            Dragable = true;
            Resizable = false;

            if (UOBuilderCore.HasBuilds())
            {
                PlayerSerials = UOBuilderCore.GetBuildList();

                int count = PlayerSerials.Count;

                int offset = 35;

                AddBackground(X, Y, 200, 50 + (offset * count), 40000);

                AddLabel(X + 75, Y + 13, 53, "UO Builder");

                int y = Y + 5;

                for (int i = 1; i <= count; i++)
                {
                    AddButton(X + 25, y + (offset * i), 5538, 5538, i, GumpButtonType.Reply, 0);

                    var pos = i - 1;

                    AddLabel(X + 65, y + (offset * i) + 2, 1153, PlayerSerials[pos].ToString());

                    var btnPos = i + 1;

                    AddButton(X + 160, y + (offset * i), 5541, 5541, btnPos, GumpButtonType.Reply, 1);
                }
            }
            else
            {
                Close();
            }
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID == 0)
            {
                Close();
            }
            else
            {
                UOBuilderCore.CleanBuild(User);

                if (info.ButtonID % 2 == 0)
                {
                    var pos = info.ButtonID / 2 - 1;

                    if (PlayerSerials != null)
                    {
                        UOBuilderCore.PlaceBuild(User, PlayerSerials[pos]);
                    }
                }

                Refresh(true, false);
            }
        }
    }
}

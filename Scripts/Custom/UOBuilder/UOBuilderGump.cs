using System.Text;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.UOBuilder
{
    internal class UOBuilderGump : BaseGump
    {
        private readonly UOBuilderPermit Permit;

        public UOBuilderGump(PlayerMobile user, UOBuilderPermit permit) : base(user, 50, 50, null)
        {
            Permit = permit;
        }

        public override void AddGumpLayout()
        {
            Closable = false;
            Dragable = true;
            Resizable = false;

            // top menu
            AddButton(X + 104, Y - 20, 22300, 22301, 3, GumpButtonType.Reply, 0);

            AddButton(X + 155, Y - 21, 22121, 22122, 4, GumpButtonType.Reply, 0);

            AddButton(X + 190, Y - 20, 22306, 22307, 5, GumpButtonType.Reply, 0);

            // main
            AddBackground(X, Y, 250, 50, 40000);

            AddLabel(X + 20, Y + 17, 53, "UO Builder |");

            // search
            AddButton(X + 102, Y + 15, 9910, 9909, 1, GumpButtonType.Reply, 0);

            AddTextEntry(X + 132, Y + 17, 45, 16, 1153, 1, Permit.LastID.ToString());

            AddButton(X + 182, Y + 15, 9904, 9903, 2, GumpButtonType.Reply, 0);

            // close
            AddButton(X + 213, Y + 15, 40015, 40015, 6, GumpButtonType.Reply, 0);

            int widthMod = -1;

            try
            {
                if (UOBuilderCore.ItemWidths.Count > 0 && Permit.LastID < UOBuilderCore.ItemWidths.Count)
                {
                    widthMod = int.Parse(UOBuilderCore.ItemWidths[Permit.LastID].ToString());

                    if (widthMod != -1)
                    {
                        widthMod = 250 - widthMod;

                        if (widthMod != 0)
                        {
                            widthMod /= 2;
                        }
                    }
                }
            }
            catch
            {
                widthMod = 15;
            }

            // art
            AddItem(X + widthMod, Y + 55, Permit.LastID);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID != 6 && int.TryParse(info.GetTextEntry(1).Text, out int idMod) && idMod != Permit.LastID)
            {
                if (idMod > 0 && idMod < UOBuilderCore.ItemID_Max)
                {
                    Permit.LastID = idMod;
                }

                Refresh(true, false);
            }
            else
            {

                switch (info.ButtonID)
                {
                    case 0:
                        {
                            Refresh(true, false);

                            break;
                        }

                    case 1:
                        {
                            if (Permit.LastID > 0)
                            {
                                Permit.LastID--;
                            }
                            else
                            {
                                Permit.LastID = UOBuilderCore.ItemID_Max;
                            }

                            Refresh(true, false);

                            break;
                        }

                    case 2:
                        {
                            if (Permit.LastID < UOBuilderCore.ItemID_Max)
                            {
                                Permit.LastID++;
                            }
                            else
                            {
                                Permit.LastID = 0;
                            }

                            Refresh(true, false);

                            break;
                        }

                    case 3:
                        {
                            User.SendMessage("Add Static");

                            User.Target = new UOBuilderTarget(Permit, true);

                            Refresh(true, false);

                            break;
                        }

                    case 4:
                        {
                            User.SendMessage("Get Static");

                            User.Target = new UOBuilderTarget(Permit, true, true);

                            Close();

                            break;
                        }

                    case 5:
                        {
                            User.SendMessage("Remove Static");

                            User.Target = new UOBuilderTarget(Permit, false);

                            Refresh(true, false);

                            break;
                        }

                    case 6:
                        {
                            User.SendMessage("Close");

                            UOBuilderCore.CleanBuild(User);

                            Permit.Center = Point3D.Zero;

                            Close();

                            break;
                        }
                }
            }
        }
    }
}

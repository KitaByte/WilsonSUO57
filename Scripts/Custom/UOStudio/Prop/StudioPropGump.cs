using Server.Gumps;
using Server.Mobiles;
using Server.Commands;

namespace Server.Custom.UOStudio
{
    internal class StudioPropGump : BaseGump
    {
        private int ID;

        private int HUE = 0;

        public StudioPropGump(PlayerMobile user, int id = 42, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)
        {
            ID = id;
        }

        public override void AddGumpLayout()
        {
            Closable = true;
            Resizable = false;
            Dragable = true;

            AddBackground(X, Y, 100, 100, 40000);

            AddTextEntry(X + 15, Y + 10, 60, 25, 53, 0, ID.ToString());

            AddLabel(X + 60, Y + 10, 1153, "ID");

            AddTextEntry(X + 15, Y + 40, 60, 25, 53, 1, HUE.ToString());

            AddLabel(X + 60, Y + 40, 1153, "HUE");

            AddButton(X + 20, Y + 70, 2362, 2362, 1, GumpButtonType.Reply, 0);

            AddButton(X + 45, Y + 70, 2361, 2361, 2, GumpButtonType.Reply, 0);

            AddButton(X + 70, Y + 70, 2360, 2360, 3, GumpButtonType.Reply, 0);

            AddItem(X + 10, Y + 100, ID, HUE);
        }

        public override void OnResponse(RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 0:
                    {
                        Close();

                        break;
                    }

                case 1:
                    {
                        if (int.TryParse(info.GetTextEntry(0).Text, out int id))
                        {
                            if (id > 0 && id < 44919)
                            {
                                ID = id;

                                User.SendMessage(53, $"Loaded ID : {id}");

                                if (int.TryParse(info.GetTextEntry(1).Text, out int hue))
                                {
                                    HUE = hue;

                                    User.SendMessage(53, $"Loaded HUE : {hue}");
                                }
                            }
                        }

                        Refresh(true, false);

                        break;
                    }

                case 2:
                    {
                        User.SendMessage(53, $"Add : {ID} [ Hue = {HUE} ]");

                        CommandSystem.Handle(User, $"{CommandSystem.Prefix}m Add StudioProp {ID} {HUE}");

                        Refresh(true, false);

                        break;
                    }

                case 3:
                    {
                        User.SendMessage(53, $"Delete Prop");

                        CommandSystem.Handle(User, $"{CommandSystem.Prefix}m Delete");

                        Refresh(true, false);

                        break;
                    }
            }
        }
    }
}

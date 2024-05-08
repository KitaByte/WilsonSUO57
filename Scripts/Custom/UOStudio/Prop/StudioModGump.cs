using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.UOStudio
{
    internal class StudioModGump : BaseGump
    {
        public StudioModGump(PlayerMobile user, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)
        {
        }

        public override void AddGumpLayout()
        {
            Closable = true;
            Resizable = false;
            Dragable = true;

            AddBackground(X, Y, 150, 130, 40000);

            AddTextEntry(X + 15, Y + 10, 60, 25, 53, 0, User.Name);

            AddLabel(X + 100, Y + 10, 1153, "Name");

            AddTextEntry(X + 15, Y + 40, 60, 25, 53, 1, User.Body.ToString());

            AddLabel(X + 100, Y + 40, 1153, "Body");

            AddTextEntry(X + 15, Y + 70, 60, 25, 53, 2, User.Hue.ToString());

            AddLabel(X + 100, Y + 70, 1153, "Hue");

            AddButton(X + 40, Y + 100, 2361, 2361, 1, GumpButtonType.Reply, 0);

            AddButton(X + 95, Y + 100, 2360, 2360, 2, GumpButtonType.Reply, 0);
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
                        if (!string.IsNullOrEmpty(info.GetTextEntry(0).Text))
                        {
                            User.NameMod = info.GetTextEntry(0).Text;
                        }

                        if (StudioEngine.TryHexToInt(info.GetTextEntry(1).Text, out int body) && body > 0)
                        {
                            User.BodyMod = body;
                        }

                        if (int.TryParse(info.GetTextEntry(2).Text, out int hue) && hue > -1)
                        {
                            User.HueMod = hue;
                        }

                        User.InvalidateProperties();

                        User.SendMessage(53, "Mods were applied!");

                        Refresh(true, false);

                        break;
                    }

                case 2:
                    {
                        StudioEngine.ResetMods(User);

                        User.SendMessage(53, "Mods were removed!");

                        Refresh(true, false);

                        break;
                    }
            }
        }
    }
}

using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.UOStudio
{
    internal class StudioSoundGump : BaseGump
    {
        private StudioFilm _Film;

        private int ID = 0;

        public StudioSoundGump(PlayerMobile user, StudioFilm film, int x = 0, int y = 0, BaseGump parent = null) : base(user, x, y, parent)
        {
            _Film = film;
        }

        public override void AddGumpLayout()
        {
            Closable = true;
            Dragable = true;
            Resizable = false;

            AddBackground(X, Y, 100, 70, 40000);

            AddTextEntry(X + 15, Y + 10, 60, 25, 53, 0, ID.ToString());

            AddLabel(X + 50, Y + 10, 1153, "SFX");

            AddButton(X + 20, Y + 40, 2362, 2362, 1, GumpButtonType.Reply, 0);

            AddButton(X + 45, Y + 40, 2361, 2361, 2, GumpButtonType.Reply, 0);

            AddButton(X + 70, Y + 40, 2360, 2360, 3, GumpButtonType.Reply, 0);
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
                            if (id > -1 && id < 0x683)
                            {
                                ID = id;

                                User.SendMessage(id, $"Preview SFX : {id}");

                                User.PlaySound(id);
                            }
                        }

                        Refresh(true, false);

                        break;
                    }

                case 2:
                    {
                        if (int.TryParse(info.GetTextEntry(0).Text, out int id))
                        {
                            if (id > -1 && id < 0x683)
                            {
                                ID = id;

                                User.SendMessage(id, $"SFX - Recorded! : {id}");

                                User.PlaySound(id);

                                _Film.AddSound(ID);
                            }
                        }

                        break;
                    }

                case 3:
                    {
                        _Film.AddSound(-1);

                        User.SendMessage(43, "Removed SFX");

                        break;
                    }
            }
        }
    }
}

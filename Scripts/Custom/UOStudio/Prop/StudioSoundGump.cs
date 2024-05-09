using Server.Gumps;
using Server.Mobiles;
using System.Globalization;

namespace Server.Custom.UOStudio
{
    internal class StudioSoundGump : BaseGump
    {
        private readonly StudioFilm _Film;

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

            AddTextEntry(X + 15, Y + 10, 60, 25, 53, 0, _Film._SoundID.ToString());

            AddLabel(X + 50, Y + 10, 1153, "SFX");

            AddButton(X + 20, Y + 40, 2362, 2362, 1, GumpButtonType.Reply, 0);

            AddButton(X + 45, Y + 40, 2361, 2361, 2, GumpButtonType.Reply, 0);

            AddButton(X + 70, Y + 40, 2360, 2360, 3, GumpButtonType.Reply, 0);
        }

        public override void OnResponse(RelayInfo info)
        {
            string inputText = info.GetTextEntry(0).Text;

            switch (info.ButtonID)
            {
                case 0:
                    {
                        Close();

                        break;
                    }

                case 1:
                    {
                        _Film._SoundID = GetSoundID(inputText);

                        if (_Film._SoundID > -1)
                        {
                            User.SendMessage(_Film._SoundID, $"Preview SFX : {_Film._SoundID}");

                            User.PlaySound(_Film._SoundID);
                        }

                        Refresh(true, false);

                        break;
                    }

                case 2:
                    {
                        _Film._SoundID = GetSoundID(inputText);

                        if (_Film._SoundID > -1)
                        {
                            User.SendMessage(_Film._SoundID, $"SFX - Recorded! : {_Film._SoundID}");

                            User.PlaySound(_Film._SoundID);

                            _Film.AddSound(_Film._SoundID);
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

        private int GetSoundID(string inputText)
        {
            if (inputText.StartsWith("0x") && inputText.Length > 2)
            {
                if (int.TryParse(inputText.Substring(2), NumberStyles.HexNumber, null, out int id))
                {
                    if (id > -1 && id < 0x683)
                    {
                        return id;
                    }
                }
            }

            if (int.TryParse(inputText, out int intId))
            {
                if (intId > -1 && intId < 0x683)
                {
                    return intId;
                }
            }

            return -1;
        }
    }
}

using Server.Gumps;
using Server.HuePickers;

namespace Server.Services.UOBlackBox.Tools
{
    public class ColorPicker : HuePicker
    {
        private readonly BoxSession Session;

        private readonly IGumpHue Gump;

        public ColorPicker(BoxSession session, int itemID, IGumpHue gump) : base(itemID)
        {
            Session = session;

            Gump = gump;
        }

        public override void OnResponse(int hue)
        {
            if (Gump is BaseGump bg)
            {
                if (Gump.Hue == hue)
                {
                    Gump.Hue = 0;
                }
                else
                {
                    Gump.Hue = hue;

                    if (Gump is ArtPopView)
                    {
                        Session.HueSelected = hue;
                    }
                }

                bg.Refresh(true, false);
            }
        }
    }
}

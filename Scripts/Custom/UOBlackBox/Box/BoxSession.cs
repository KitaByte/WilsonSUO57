using System.Collections.Generic;

using Server.Gumps;
using Server.Mobiles;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox
{
    public class BoxSession
    {
        public PlayerMobile User { get; set; }

        public BlackBox Box { get; set; }

        public int ArtSelected { get; set; }

        public int HueSelected { get; set; }

        public List<GumpLayer> Layers { get; set; }

        public BoxSession(Mobile user, BlackBox box)
        {
            User = user as PlayerMobile;

            Box = box;

            Layers = new List<GumpLayer>();
        }

        public void StartBox()
        {
            User.LightLevel = 30;

            Box.IsOpen = true;

            UndoManager.SetupHandler(User.Name);

            BaseGump.SendGump(new UnDoTool(this));

            User.SendMessage(2721, "Black Box => ON");
        }

        public void UpdateBox(string cmd = "Empty!")
        {
            if (BoxCore.ShowInfo)
                User.SendMessage(Box.Hue - 10, $"Black Box => Processing [{cmd}]");

            if (cmd != "Close")
            {
                //Box.UpdateHue(1922 + Utility.Random(9));
            }
            else
            {
                if (!User.HasGump(typeof(IToolInfo)))
                    EndBox();
            }
        }

        public void EndBox()
        {
            Box.Hue = 1175;

            Box.Movable = true;

            Box.IsOpen = false;

            UndoManager.RemoveHandler(User.Name);

            CloseAllBoxGumps(User);

            User.SendMessage(1175, "Black Box => OFF");

            Box.Session = null;
        }

        private static void CloseAllBoxGumps(Mobile user)
        {
            while (user.HasGump(typeof(IToolInfo)))
            {
                user.CloseGump(typeof(IToolInfo));
            }

            while (user.HasGump(typeof(ArtViewerMin)))
            {
                user.CloseGump(typeof(ArtViewerMin));
            }

            while (user.HasGump(typeof(ToolInfo)))
            {
                user.CloseGump(typeof(ToolInfo));
            }
        }
    }
}

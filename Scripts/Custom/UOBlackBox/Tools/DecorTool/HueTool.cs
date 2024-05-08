using System.Text;

using Server.Gumps;

namespace Server.Services.UOBlackBox.Tools
{
    public class HueTool : BaseGump, IGumpHue, IToolInfo
    {
        public BoxSession Session { get; set; }

        public int Hue { get; set; }

        private int SetHue = 0;

        private bool IsPicker = false;

        private ColorSlots ColorSlot { get; set; }

        private enum ColorSlots
        {
            Hue1,
            Hue2,
            Hue3
        }

        private int Hue1 = 0;

        private int Hue2 = 0;

        private int Hue3 = 0;

        public HueTool(BoxSession session) : base(session.User, 0, 0, null)
        {
            User.SendMessage(52, $"Opening Hue Tool");

            Session = session;
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Hue Tool : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Blue Gem - Hue Picker");
            information.AppendLine("Gold Button - Hue Target");
            information.AppendLine("Hue Input - Manually Set Hue");
            information.AppendLine("Reset Input - Resets Hues");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "UO Black Box : Hue Tool";

            var width = 250;
            var height = 150;

            if (!IsPicker)
            {
                switch (ColorSlot)
                {
                    case ColorSlots.Hue1: Hue1 = Hue; break;
                    case ColorSlots.Hue2: Hue2 = Hue; break;
                    case ColorSlots.Hue3: Hue3 = Hue; break;
                }
            }
            else
            {
                IsPicker = false;
            }

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // Hue 1
            AddButton(X + 65, Y + 50, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 1, GumpButtonType.Reply, 0);

            AddItem(X + 53, Y + 60, GumpCore.HueItem, Hue1);

            AddButton(X + 65, Y + 90, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 4, GumpButtonType.Reply, 0);

            // Hue 2
            AddButton(X + 120, Y + 50, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 2, GumpButtonType.Reply, 0);

            AddItem(X + 108, Y + 60, GumpCore.HueItem, Hue2);

            AddButton(X + 120, Y + 90, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 5, GumpButtonType.Reply, 0);

            // Hue 3
            AddButton(X + 174, Y + 50, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 3, GumpButtonType.Reply, 0);

            AddItem(X + 162, Y + 60, GumpCore.HueItem, Hue3);

            AddButton(X + 174, Y + 90, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 6, GumpButtonType.Reply, 0);

            // Set Hue
            AddLabel(X + 65, Y + 115, GumpCore.GoldText, "Set Hue :");

            AddTextEntry(X + 130, Y + 115, 50, 25, GumpCore.WhtText, 0, SetHue.ToString());

            AddButton(X + 174, Y + 119, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 7, GumpButtonType.Reply, 0);

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                if (info.GetTextEntry(0)?.Text.ToLower() == "reset")
                {
                    Hue = 0;
                    Hue1 = 0;
                    Hue2 = 0;
                    Hue3 = 0;
                    SetHue = 0;

                    Refresh(true, false);
                }
                else
                {
                    if (info.ButtonID < 4)
                    {
                        IsPicker = true;

                        User.SendMessage(52, $"Select Hue!");
                    }
                    else
                    {
                        User.SendMessage(52, $"Set Hue");
                    }

                    if (info.ButtonID == 7)
                    {
                        if (int.TryParse(info.GetTextEntry(0)?.Text, out int newhue))
                        {
                            if (newhue > 0 && newhue < 3000)
                                SetHue = newhue;
                            else
                                SetHue = 0;
                        }
                        else
                        {
                            SetHue = 0;
                        }
                    }

                    if (info.ButtonID > 0)
                    {
                        switch (info.ButtonID)
                        {
                            case 1: User.SendHuePicker(new ColorPicker(Session, GumpCore.HueItem, this)); ColorSlot = ColorSlots.Hue1; break;
                            case 2: User.SendHuePicker(new ColorPicker(Session, GumpCore.HueItem, this)); ColorSlot = ColorSlots.Hue2; break;
                            case 3: User.SendHuePicker(new ColorPicker(Session, GumpCore.HueItem, this)); ColorSlot = ColorSlots.Hue3; break;
                            case 4: BoxCore.RunBBCommand(User, $"m Set Hue {Hue1}"); break;
                            case 5: BoxCore.RunBBCommand(User, $"m Set Hue {Hue2}"); break;
                            case 6: BoxCore.RunBBCommand(User, $"m Set Hue {Hue3}"); break;
                            case 7: BoxCore.RunBBCommand(User, $"m Set Hue {SetHue}"); break;
                            default:
                                {
                                    GumpCore.SendInfoGump(Session, this);

                                    break;
                                }
                        }

                        if (info.ButtonID < 8)
                        {
                            Session.UpdateBox("Hue");
                        }

                        Refresh(true, false);
                    }
                }
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }
    }
}

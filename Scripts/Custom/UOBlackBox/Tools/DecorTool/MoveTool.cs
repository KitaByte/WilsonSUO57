using System.Linq;
using System.Text;

using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Services.UOBlackBox.Tools
{
    public class MoveTool : BaseGump, IToolInfo
    {
        public BoxSession Session { get; set; }

        public object MoveHanle { get; set; }

        private bool HookToggle = false;

        private string Range { get; set; }

        public MoveTool(BoxSession session) : base(session.User, 0, 0, null)
        {
            User.SendMessage(52, $"Opening Move Tool");

            Session = session;

            Range = "###";
        }

        public StringBuilder LoadInfo()
        {
            var information = new StringBuilder();

            information.AppendLine("Move Tool : Instructions");
            information.AppendLine("-----------------------------");
            information.AppendLine("");
            information.AppendLine("Outter Arrows - Move * Range");
            information.AppendLine("Inner Arrows - Z * Range");
            information.AppendLine("Gold Button - Move Art");
            information.AppendLine("Range Input - Set Range");
            information.AppendLine("Gold Button - Toggle Art Handle");
            information.AppendLine("");
            information.AppendLine("-----------------------------");
            information.AppendLine("UO Black Box © 2023 by Kita");
            information.AppendLine("-----------------------------");

            return information;
        }

        public override void AddGumpLayout()
        {
            var title = "UO Black Box : Move Tool";

            var width = 250;
            var height = 250;

            AddBackground(X, Y, width, height, GumpCore.MainBG);

            // Info
            GumpCore.SetGumpInfo(this);

            // Title
            AddLabel(X + GumpCore.GetCentered(width, title, true), Y + 20, GumpCore.GoldText, title);

            // Direction x 8
            AddButton(X + 145, Y + 70, GumpCore.ArrowNorth, GumpCore.ArrowNorth, 1, GumpButtonType.Reply, 0); // North
            AddButton(X + 165, Y + 110, GumpCore.ArrowRight, GumpCore.ArrowRight, 2, GumpButtonType.Reply, 0); // Right
            AddButton(X + 145, Y + 150, GumpCore.ArrowEast, GumpCore.ArrowEast, 3, GumpButtonType.Reply, 0); // East
            AddButton(X + (width / 2) - 25, Y + 170, GumpCore.ArrowDown, GumpCore.ArrowDown, 4, GumpButtonType.Reply, 0); // Down

            AddButton(X + 55, Y + 150, GumpCore.ArrowSouth, GumpCore.ArrowSouth, 5, GumpButtonType.Reply, 0); // South
            AddButton(X + 35, Y + 110, GumpCore.ArrowLeft, GumpCore.ArrowLeft, 6, GumpButtonType.Reply, 0); // Left
            AddButton(X + 55, Y + 70, GumpCore.ArrowWest, GumpCore.ArrowWest, 7, GumpButtonType.Reply, 0); // West
            AddButton(X + (width / 2) - 25, Y + 50, GumpCore.ArrowUp, GumpCore.ArrowUp, 8, GumpButtonType.Reply, 0); // Up

            // Raise/Lower
            AddButton(X + (width / 2) - 8, Y + 105, GumpCore.ArrowRaise, GumpCore.ArrowRaise, 9, GumpButtonType.Reply, 0); // Raise
            AddButton(X + (width / 2) - 8, Y + 150, GumpCore.ArrowLower, GumpCore.ArrowLower, 10, GumpButtonType.Reply, 0); // Lower

            // Move
            AddButton(X + (width / 2) - 33, Y + 128, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 11, GumpButtonType.Reply, 0); // Move

            // Force Move
            if (!HookToggle)
            {
                AddButton(X + (width / 2) + 22, Y + 128, GumpCore.RndBtnUp, GumpCore.RndBtnDown, 12, GumpButtonType.Reply, 0);
            }
            else
            {
                AddButton(X + (width / 2) + 22, Y + 128, GumpCore.RndBtnDown, GumpCore.RndBtnUp, 12, GumpButtonType.Reply, 0);
            }

            // Offset Amount
            AddTextEntry(X + 105, Y + 125, 50, 25, GumpCore.WhtText, 0, Range);

            // Close
            GumpCore.SetGumpClose(this, width);
        }

        public override void OnResponse(RelayInfo info)
        {
            if (info.ButtonID > 0)
            {
                if (MoveHanle != null && User.GetObjectsInRange(50).FirstOrDefault(o => o == MoveHanle) == null)
                {
                    MoveHanle = null;
                }

                var mod = 1;

                if (int.TryParse(info.GetTextEntry(0)?.Text, out int offset))
                {
                    if (offset > -125 && offset < 125)
                    {
                        mod = offset;

                        Range = offset.ToString();
                    }
                }

                if (info.ButtonID < 9)
                {
                    if (MoveHanle == null)
                    {
                        HookToggle = false;

                        BoxCore.RunBBCommand(User, GetDirection((Direction)info.ButtonID - 1, mod));
                    }
                    else
                    {
                        ForceDirection((Direction)info.ButtonID - 1, mod);
                    }

                    User.SendMessage(52, $"Moving Art {(Direction)info.ButtonID - 1}");
                }
                else
                {
                    if (MoveHanle == null)
                    {
                        HookToggle = false;

                        switch (info.ButtonID)
                        {
                            case 9:
                                {
                                    BoxCore.RunBBCommand(User, $"m inc z {mod}");

                                    User.SendMessage(52, $"Raise Art {mod}");

                                    break;
                                }
                            case 10:
                                {
                                    BoxCore.RunBBCommand(User, $"m inc z -{mod}");

                                    User.SendMessage(52, $"Lower Art {mod}");

                                    break;
                                }
                            case 11:
                                {
                                    BoxCore.RunBBCommand(User, "move");

                                    User.SendMessage(52, $"Move Art");

                                    break;
                                }
                            case 12:
                                {
                                    User.Target = new TargetArt(this);

                                    HookToggle = true;

                                    User.SendMessage(52, $"Attach Art");

                                    break;
                                }
                            default:
                                {
                                    GumpCore.SendInfoGump(Session, this);

                                    break;
                                }
                        }
                    }
                    else
                    {
                        switch (info.ButtonID)
                        {
                            case 9:
                                {
                                    ForceElevation(true, mod);

                                    User.SendMessage(52, $"Raise Art {mod}");

                                    break;
                                }
                            case 10:
                                {
                                    ForceElevation(false, mod);

                                    User.SendMessage(52, $"Lower Art {mod}");

                                    break;
                                }
                            case 11:
                                {
                                    BoxCore.RunBBCommand(User, "move");

                                    User.SendMessage(52, $"Move Art");

                                    break; ;
                                }
                            case 12:
                                {
                                    MoveHanle = null;

                                    HookToggle = false;

                                    User.SendMessage(52, $"Detach Art");

                                    break;
                                }
                            default:
                                {
                                    GumpCore.SendInfoGump(Session, this);

                                    break;
                                }
                        }
                    }
                }

                if (info.ButtonID < 13)
                {
                    Session.UpdateBox("Move");
                }

                Refresh(true, false);
            }
            else
            {
                Session.UpdateBox("Close");
            }
        }

        private string GetDirection(Direction d, int offset)
        {
            switch (d)
            {
                case Direction.North: return $"m inc y -{offset}";
                case Direction.Right: return $"m inc x {offset} y -{offset}";
                case Direction.East: return $"m inc x {offset}";
                case Direction.Down: return $"m inc x {offset} y {offset}";
                case Direction.South: return $"m inc y {offset}";
                case Direction.Left: return $"m inc x -{offset} y {offset}";
                case Direction.West: return $"m inc x -{offset}";
                case Direction.Up: return $"m inc x -{offset} y -{offset}";
                default: return $"inc z {0}";
            }
        }

        private void ForceDirection(Direction d, int offset)
        {
            if (MoveHanle is Item i)
            {
                switch (d)
                {
                    case Direction.North: i.Y -= offset; break;
                    case Direction.Right: i.X += offset; i.Y -= offset; break;
                    case Direction.East: i.X += offset; break;
                    case Direction.Down: i.X += offset; i.Y += offset; break;
                    case Direction.South: i.Y += offset; break;
                    case Direction.Left: i.X -= offset; i.Y += offset; break;
                    case Direction.West: i.X -= offset; break;
                    case Direction.Up: i.X -= offset; i.Y -= offset; break;
                    default: i.Z += 0; break;
                }
            }

            if (MoveHanle is Static st)
            {
                switch (d)
                {
                    case Direction.North: st.Y -= offset; break;
                    case Direction.Right: st.X += offset; st.Y -= offset; break;
                    case Direction.East: st.X += offset; break;
                    case Direction.Down: st.X += offset; st.Y += offset; break;
                    case Direction.South: st.Y += offset; break;
                    case Direction.Left: st.X -= offset; st.Y += offset; break;
                    case Direction.West: st.X -= offset; break;
                    case Direction.Up: st.X -= offset; st.Y -= offset; break;
                    default: st.Z += 0; break;
                }
            }

            if (MoveHanle is PlayerMobile pm)
            {
                switch (d)
                {
                    case Direction.North: pm.Y -= offset; break;
                    case Direction.Right: pm.X += offset; pm.Y -= offset; break;
                    case Direction.East: pm.X += offset; break;
                    case Direction.Down: pm.X += offset; pm.Y += offset; break;
                    case Direction.South: pm.Y += offset; break;
                    case Direction.Left: pm.X -= offset; pm.Y += offset; break;
                    case Direction.West: pm.X -= offset; break;
                    case Direction.Up: pm.X -= offset; pm.Y -= offset; break;
                    default: pm.Z += 0; break;
                }
            }
        }

        private void ForceElevation(bool IsUp, int offset)
        {
            if (MoveHanle is Item i)
            {
                if (IsUp)
                {
                    i.Z += offset;
                }
                else
                {
                    i.Z -= offset;
                }
            }

            if (MoveHanle is Static st)
            {
                if (IsUp)
                {
                    st.Z += offset;
                }
                else
                {
                    st.Z -= offset;
                }
            }

            if (MoveHanle is PlayerMobile pm)
            {
                if (IsUp)
                {
                    pm.Z += offset;
                }
                else
                {
                    pm.Z -= offset;
                }
            }
        }
    }
}

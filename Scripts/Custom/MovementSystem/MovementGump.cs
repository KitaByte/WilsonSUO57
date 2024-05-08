using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.MovementSystem
{
    public class MovementGump : BaseGump
    {
        public override bool CloseOnMapChange => false;

        public MovementGump(PlayerMobile user) : base(user, 15, 15, null)
        {
        }

        public override void AddGumpLayout()
        {
            int bgWidth = MoveSettings.SwimActive ? MoveSettings.ClimbActive ? 154 : 90 : 90;

            AddBackground(X + 0, Y + 0, bgWidth, 90, 0x53);

            if (MoveSettings.SwimActive)
            {
                int hue = SwimUtility.HasWater(User.Location, User.Map) ? User.Stam < MoveSettings.SwimStamMod ? 42 : 2 : 62;

                int id = 0x6E;

                int x = X + 13;
                int y = Y + 13;

                AddAlphaRegion(x, y, 128, 64);

                AddImage(x + 0, y + 0, id, hue);
                AddImage(x + 0, y + 5, id, hue);
                AddImage(x + 5, y + 0, id, hue);
                AddImage(x + 5, y + 5, id, hue);

                AddButton(x + 3, y + 3, id, id, 1, GumpButtonType.Reply, 0);

                if (MoveSettings.ClimbActive)
                {
                    hue = ClimbUtility.HasRock(User.Location, User.Map) ? User.Stam < MoveSettings.ClimbStamMod ? 42 : 52 : 62;

                    id = 0x6A;

                    x += 64;

                    AddImage(x + 0, y + 0, id, hue);
                    AddImage(x + 0, y + 5, id, hue);
                    AddImage(x + 5, y + 0, id, hue);
                    AddImage(x + 5, y + 5, id, hue);

                    AddButton(x + 3, y + 3, id, id, 2, GumpButtonType.Reply, 0);
                }
            }
            else
            {
                int hue = ClimbUtility.HasRock(User.Location, User.Map) ? User.Stam < MoveSettings.ClimbStamMod ? 42 : 52 : 62;

                int id = 0x6A;

                int x = X + 13;
                int y = Y + 13;

                AddImage(x + 0, y + 0, id, hue);
                AddImage(x + 0, y + 5, id, hue);
                AddImage(x + 5, y + 0, id, hue);
                AddImage(x + 5, y + 5, id, hue);

                AddButton(x + 3, y + 3, id, id, 2, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(RelayInfo info)
        {
            switch (info.ButtonID)
            {
                case 1: // swim
                    {
                        if (User.Mounted)
                        {
                            User.SendMessage(43, "Can't swim while mounted!");

                            break;
                        }
                        else
                        {
                            User.Target = new SwimTarget(User);

                            break;
                        }
                    }
                case 2: // climb
                    {
                        if (User.Mounted)
                        {
                            User.SendMessage(43, "Can't climb while mounted!");

                            break;
                        }
                        else
                        {
                            User.Target = new ClimbTarget(User);

                            break;
                        }
                    }
            }

            Close();
        }
    }
}

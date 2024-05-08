using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Misc
{
    internal class StaffLoginSetup
    {
        public static void Initialize()
        {
            EventSink.Login += EventSink_Login;
        }

        private static void EventSink_Login(LoginEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm && pm.AccessLevel > AccessLevel.Player)
            {
                switch (pm.AccessLevel)
                {
                    case AccessLevel.VIP:
                        break;
                    case AccessLevel.Counselor:
                        break;
                    case AccessLevel.Decorator:
                        break;
                    case AccessLevel.Spawner:
                        break;
                    case AccessLevel.GameMaster:
                        break;
                    case AccessLevel.Seer:
                        break;
                    case AccessLevel.Administrator:
                        AutoSet(pm);
                        break;
                    case AccessLevel.Developer:
                        AutoSet(pm);
                        break;
                    case AccessLevel.CoOwner:
                        AutoSet(pm);
                        break;
                    case AccessLevel.Owner:
                        AutoSet(pm);
                        break;
                }
            }
        }

        private static void AutoSet(PlayerMobile pm)
        {
            pm.RawStr = 125;
            pm.RawDex = 125;
            pm.RawInt = 125;

            foreach (var skill in pm.Skills)
            {
                skill.Base = 120;
            }

            pm.LightLevel = 30;

            pm.SendMessage(62, $"{pm.Name} : Auto Setup - Complete!");

            Clock.GetTime(pm, out int label, out string time);

            pm.SendLocalizedMessage(label, true, $" {time}", string.Empty, 62);

            //pm.AddToBackpack(new StaffCloak());

            //pm.AddToBackpack(new StaffOrb());

            //pm.AddToBackpack(new GMEthereal());

            //pm.AddToBackpack(new GMHidingStone());
        }
    }
}

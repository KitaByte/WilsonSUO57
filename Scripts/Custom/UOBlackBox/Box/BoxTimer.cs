using System;

using Server.Mobiles;
using Server.Services.UOBlackBox.Tools;

namespace Server.Services.UOBlackBox
{
    public class BoxTimer : Timer
    {
        public BoxTimer() : base(TimeSpan.FromSeconds(1))
        {
            Priority = TimerPriority.OneSecond;
        }

        protected override void OnTick()
        {
            bool inUse = false;

            foreach (var players in World.Mobiles.Values)
            {
                if (players is PlayerMobile pm && pm.AccessLevel > BoxCore.StaffAccess)
                {
                    if (pm.HasGump(typeof(TravelTool)))
                    {
                        if (pm.FindGump(typeof(TravelTool)) is TravelTool gump && gump.IsLive)
                        {
                            gump.Refresh(true, false);

                            inUse = true;
                        }
                    }
                }
            }

            if (inUse)
            {
                Start();
            }
        }
    }
}

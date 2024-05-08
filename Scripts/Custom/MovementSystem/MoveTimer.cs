using System;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.MovementSystem
{
    internal class MoveTimer : Timer
    {
        public MoveTimer() : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3))
        {
            Priority = TimerPriority.OneSecond;
        }

        protected override void OnTick()
        {
            if ((MoveSettings.SwimActive || MoveSettings.ClimbActive) && MovementCore.MovePlayers != null && MovementCore.MovePlayers.Count > 0)
            {
                foreach (var player in MovementCore.MovePlayers)
                {
                    if (player != null && player.Alive)
                    {
                        if (player.HasGump(typeof(MovementGump)))
                        {
                            player.CloseGump(typeof(MovementGump));
                        }

                        BaseGump.SendGump(new MovementGump(player as PlayerMobile));
                    }
                }
            }
        }
    }
}

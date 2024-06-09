using System;
using Server.Mobiles;

namespace Server.Custom.DanceSystem
{
    internal class DanceTimer : Timer
    {
        public DanceTimer() : base(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
        {
            Priority = TimerPriority.TwoFiftyMS;
        }

        protected override void OnTick()
        {
            DanceEngine.ValidatePlayerList();

            if (DanceEngine.PlayerDanceList.Count > 0)
            {
                foreach (var player in DanceEngine.PlayerDanceList)
                {
                    if (player is PlayerMobile pm)
                    {
                        if (Utility.RandomDouble() < 0.3)
                        {
                            SetRandomDirection(pm);
                        }

                        pm.Animate((AnimationType)Utility.RandomMinMax(0, 8), 0);

                        TryPlaySFX(pm);
                    }
                }
            }
        }

        private void SetRandomDirection(PlayerMobile pm)
        {
            switch (Utility.RandomMinMax(1, 8))
            {
                case 1:
                    {
                        pm.Direction = Direction.North;

                        break;
                    }

                case 2:
                    {
                        pm.Direction = Direction.Right;

                        break;
                    }

                case 3:
                    {
                        pm.Direction = Direction.East;

                        break;
                    }

                case 4:
                    {
                        pm.Direction = Direction.Down;

                        break;
                    }

                case 5:
                    {
                        pm.Direction = Direction.South;

                        break;
                    }

                case 6:
                    {
                        pm.Direction = Direction.Left;

                        break;
                    }

                case 7:
                    {
                        pm.Direction = Direction.West;

                        break;
                    }

                case 8:
                    {
                        pm.Direction = Direction.Up;

                        break;
                    }
            }
        }

        private static void TryPlaySFX(PlayerMobile pm)
        {
            if (Utility.RandomDouble() < 0.05)
            {
                if (pm.Female)
                {
                    pm.PlaySound(Utility.RandomList(0x30F, 0x31A, 0x337));
                }
                else
                {
                    pm.PlaySound(Utility.RandomList(0x41E, 0x42A, 0x449));
                }
            }

            if (Utility.RandomDouble() < 0.1)
            {
                pm.FixedEffect(0xA7E3, 1, 14, Utility.RandomBrightHue(), 0);
            }
        }
    }
}

using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.MovementSystem
{
    internal class ClimbTarget : Target
    {
        public ClimbTarget(Mobile from) : base(MoveSettings.ClimbTargetRange, true, TargetFlags.None)
        {
            if (ClimbUtility.HasRock(from.Location, from.Map))
            {
                from.SendMessage(53, "Target rocks to start climbing!");
            }
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (from.HasGump(typeof(MovementGump)))
            {
                from.CloseGump(typeof(MovementGump));
            }

            if (targeted is LandTarget lt)
            {
                if (lt.Name.ToLower() == "rock")
                {
                    ClimbUtility.TryClimb(from, lt);

                    MovementCore.AddPlayerLoc(from);

                    from.ClearHands();
                }
                else
                {
                    if (ClimbUtility.HasRock(from.Location, from.Map) && from.Map.CanSpawnMobile(lt.Location))
                    {
                        from.MoveToWorld(lt.Location, from.Map);

                        from.Animate(AnimationType.Attack, 0);

                        from.PlaySound(Utility.RandomList(0x125, 0x126));

                        MovementCore.RemovePlayerLoc(from);

                        from.Mana = from.ManaMax;
                    }
                    else
                    {
                        from.SendMessage(42, "I can't climb there!");
                    }
                }
            }
            else
            {
                from.SendMessage(42, "I can't climb there!");
            }

            BaseGump.SendGump(new MovementGump(from as PlayerMobile));
        }
    }
}

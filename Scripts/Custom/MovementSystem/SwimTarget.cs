using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.MovementSystem
{
    internal class SwimTarget : Target
    {
        public SwimTarget(Mobile from) : base(MoveSettings.SwimTargetRange, true, TargetFlags.None)
        {
            if (SwimUtility.HasWater(from.Location, from.Map))
            {
                from.SendMessage(53, "Target shore to exit water!");
            }
            else
            {
                from.SendMessage(53, "Target water to start swimming!");
            }
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (!SwimUtility.HasWater(from.Location, from.Map) && targeted is StaticTarget st && st.Z < from.Z && st.Name.ToLower() == "water")
            {
                int z = st.Location.Z - MoveSettings.SwimSinkMod;

                var sunkenLoc = new Point3D(st.Location.X, st.Location.Y, z);

                from.MoveToWorld(sunkenLoc, from.Map);

                SwimUtility.SplashAnimation(from, st.Location);

                SwimUtility.StartSwimming(from);
            }
            else
            {
                if (targeted is LandTarget lTarg)
                {
                    ValidateTarget(from, lTarg.Location);
                }
                else if (targeted is StaticTarget sTarg)
                {
                    ValidateTarget(from, sTarg.Location);
                }
            }
        }

        private void ValidateTarget(Mobile from, Point3D location)
        {
            if (from.Map.CanSpawnMobile(location))
            {
                SwimUtility.StopSwimming(from);

                SwimUtility.SplashAnimation(from, location);

                from.MoveToWorld(location, from.Map);

                if (from.HasGump(typeof(MovementGump)))
                {
                    from.CloseGump(typeof(MovementGump));
                }

                BaseGump.SendGump(new MovementGump(from as PlayerMobile));
            }
            else
            {
                from.SendMessage(42, "I can't swim there!");
            }
        }
    }
}

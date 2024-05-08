using Server.Gumps;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.UOBuilder
{
    internal class UOBuilderTarget : Target
    {
        private readonly UOBuilderPermit Permit;

        private readonly bool AddStatic;

        private readonly bool GetStatic;

        public UOBuilderTarget(UOBuilderPermit permit, bool addStatic, bool getStatic = false) : base(40, true, TargetFlags.None)
        {
            Permit = permit;

            AddStatic = addStatic;

            GetStatic = getStatic;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted != null)
            {
                if (GetStatic)
                {
                    int id = 0;

                    if (targeted is Item item)
                    {
                        id = item.ItemID;
                    }
                    else if (targeted is StaticTarget statix)
                    {
                        id = statix.ItemID;
                    }
                    else if (targeted is UOBuilderStatic uobs)
                    {
                        id = uobs.ItemID;
                    }

                    if (id > 0)
                    {
                        Permit.LastID = id;
                    }

                    BaseGump.SendGump(new UOBuilderGump(from as PlayerMobile, Permit));
                }
                else
                {
                    if (AddStatic)
                    {
                        Point3D loc = Point3D.Zero;

                        if (targeted is Item item)
                        {
                            loc = item.Location;
                        }
                        else if (targeted is StaticTarget statix)
                        {
                            loc = statix.Location;
                        }
                        else if (targeted is LandTarget land)
                        {
                            loc = land.Location;
                        }

                        if (loc != Point3D.Zero && Permit.InPermitArea(loc))
                        {
                            int top = from.Map.GetAverageZ(loc.X, loc.Y);

                            UOBuilderStatic uob_Static = new UOBuilderStatic(Permit.LastID);

                            uob_Static.DropToWorld(new Point3D(loc.X, loc.Y, top), from.Map);

                            uob_Static.AddPermit(Permit);

                            from.Target = new UOBuilderTarget(Permit, true);
                        }
                    }
                    else
                    {
                        if (targeted is UOBuilderStatic uobs)
                        {
                            if (uobs.Permit.BlessedFor == from)
                            {
                                uobs.RemovePermit();

                                from.Target = new UOBuilderTarget(Permit, false);
                            }
                        }
                    }
                }
            }
        }
    }
}

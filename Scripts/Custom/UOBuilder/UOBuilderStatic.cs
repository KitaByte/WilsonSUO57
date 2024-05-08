using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.UOBuilder
{
    internal class UOBuilderStatic : Static
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public UOBuilderPermit Permit { get; private set; }

        [CommandProperty(AccessLevel.Player)]
        public int LOC_X
        {
            get
            {
                return X;
            }
            set
            {
                if (Permit != null && Permit.Center != Point3D.Zero)
                {
                    if (value < Permit.Center.X + Permit.PermitArea && value > Permit.Center.X - Permit.PermitArea)
                    {
                        if (value + X > 0 && value + X < Map.Width)
                        {
                            X = value;
                        }
                    }
                }
            }
        }

        [CommandProperty(AccessLevel.Player)]
        public int LOC_Y
        {
            get
            {
                return Y;
            }
            set
            {
                if (Permit != null && Permit.Center != Point3D.Zero)
                {
                    if (value < Permit.Center.Y + Permit.PermitArea && value > Permit.Center.Y - Permit.PermitArea)
                    {
                        if (value + Y > 0 && value + Y < Map.Height)
                        {
                            Y = value;
                        }
                    }
                }
            }
        }

        [CommandProperty(AccessLevel.Player)]
        public int LOC_Z
        {
            get
            {
                return Z;
            }
            set
            {
                if (value < 100 && value > Map.GetAverageZ(X, Y))
                {
                    Z = value;
                }
            }
        }

        [CommandProperty(AccessLevel.Player)]
        public int HUE
        {
            get
            {
                return Hue;
            }
            set
            {
                if (value > 0 && value < 4000)
                {
                    Hue = value;
                }
            }
        }

        [Constructable]
        public UOBuilderStatic() : this(2)
        {
        }

        public UOBuilderStatic(int itemID) : base(itemID)
        {
            Name = "UOBuilder - Static";
        }

        public void AddPermit(UOBuilderPermit permit, bool storeStatic = true)
        {
            Permit = permit;

            BlessedFor = permit.BlessedFor;

            if (storeStatic)
            {
                UOBuilderCore.AddStatic(permit, this);
            }
        }

        internal void RemovePermit()
        {
            UOBuilderCore.RemoveStatic(Permit, this);

            Delete();
        }

        internal void AddStaff(Mobile from)
        {
            if (from.AccessLevel > AccessLevel.Player)
            {
                BlessedFor = from;
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == Permit.BlessedFor)
            {
                from.SendGump(new PropertiesGump(from, this));
            }
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            var mobs = GetMobilesInRange(10);

            if (mobs != null)
            {
                bool isPlayer = false;

                foreach (var mob in mobs)
                {
                    if (mob is PlayerMobile pm && pm.AccessLevel == AccessLevel.Player && pm != BlessedFor)
                    {
                        isPlayer = true;
                    }
                }

                Visible = isPlayer;
            }

            mobs.Free();
        }

        internal void UpdateStats(UOBuilderEntity entity)
        {
            var map = Map.Parse(entity.E_Map);

            if (map != null)
            {
                ItemID = entity.E_ID;

                Hue = entity.E_HUE;

                MoveToWorld(new Point3D(entity.E_X, entity.E_Y, entity.E_Z), map);
            }
        }

        internal void ConvertStatic()
        {
            Static plainStatic = new Static(ItemID)
            {
                Hue = Hue
            };

            plainStatic.MoveToWorld(Location, Map);
        }

        public UOBuilderStatic(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}

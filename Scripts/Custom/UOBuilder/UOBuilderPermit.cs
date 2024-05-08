using Server.Gumps;
using Server.Mobiles;

namespace Server.Custom.UOBuilder
{
    public class UOBuilderPermit : Item
    {
        [CommandProperty(AccessLevel.GameMaster)]
        public int PermitArea { get; private set; } = 100;

        [CommandProperty(AccessLevel.GameMaster)]
        public int BuildAmount { get; private set; } = 1000;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Center { get; set; } = Point3D.Zero;

        internal bool InPermitArea(Point3D point)
        {
            if (ValidateMapEdge(point))
            {
                if (Center == Point3D.Zero)
                {
                    return true;
                }

                Point3D start = new Point3D(Center.X - PermitArea, Center.Y - PermitArea, Map.GetAverageZ(Center.X - PermitArea, Center.Y - PermitArea));

                Point3D end = new Point3D(Center.X + PermitArea, Center.Y + PermitArea, 100);

                Rectangle3D permitBox = new Rectangle3D(start, end);

                return permitBox.Contains(point);
            }
            else
            {
                return false;
            }
        }

        private bool ValidateMapEdge(Point3D point)
        {
            if (BlessedFor != null && point.X > PermitArea && point.Y > PermitArea)
            {
                if (point.X < BlessedFor.Map.Width - PermitArea && point.Y < BlessedFor.Map.Height - PermitArea)
                {
                    return true;
                }
            }

            return false;
        }

        public int LastID { get; set; } = 2;

        [Constructable]
        public UOBuilderPermit() : base(0x14F0)
        {
            Name = "Building Permit";

            LootType = LootType.Blessed;

            Weight = 1.0;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (BlessedFor == null)
            {
                BlessedFor = from;
            }
            else
            {
                if (from.AccessLevel == AccessLevel.Player)
                {
                    from.SendMessage(42, "This does not belong to you!");

                    Delete();
                }
            }

            if (RootParent != BlessedFor)
            {
                from.SendMessage(53, "This must be in your pack in order to use!");

                return;
            }

            if (BlessedFor == from && UOBuilderCore.CanBuild(this))
            {
                if (Center == Point3D.Zero)
                {
                    UOBuilderCore.PlaceBuild(this);
                }

                BaseGump.SendGump(new UOBuilderGump(from as PlayerMobile, this));
            }
        }

        public UOBuilderPermit(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version

            writer.Write(PermitArea);

            writer.Write(BuildAmount);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();

            if (version >= 0)
            {
                PermitArea = reader.ReadInt();

                BuildAmount = reader.ReadInt();
            }
        }
    }
}

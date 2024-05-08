using Server.Commands;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.Misc
{
    public class FCSTele00 : AddonComponent
    {
        [Constructable]
        public FCSTele00() : base(41108)
        {
            Name = "Master Bedroom";
        }

        public FCSTele00(Serial serial) : base(serial)
        {
        }

        public override bool HandlesOnMovement => true;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.Player && m.Location == Location)
            {
                m.Frozen = true;

                int dist = 20;

                var loc = m.Location;

                var newX = loc.X + dist;

                m.MoveToWorld(new Point3D(newX, loc.Y, loc.Z), Map);

                m.SendMessage($"You have been moved +{dist} tiles : from {loc} to {m.Location}");

                m.Frozen = false;
            }

            base.OnMovement(m, oldLocation);
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

    public class FCSTele01 : AddonComponent
    {
        [Constructable]
        public FCSTele01() : base(41108)
        {
            Name = "Master Bedroom";
        }

        public FCSTele01(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                player.MoveToWorld(new Point3D(player.X + 20, player.Y, player.Z), player.Map);

                player.SendMessage("You have been moved +20 units in the X direction!");
            }
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

    #region FCSTele00Alpha and FCSTele00Beta
    public class FCSTele00Alpha : AddonComponent
    {
        // Static reference to the target teleporter
        public static FCSTele00Beta Destination;

        // constructor
        [Constructable]
        public FCSTele00Alpha() : base(41108)
        {
            Name = "Master Bedroom";
        }

        // Serialization constructor
        public FCSTele00Alpha(Serial serial) : base(serial)
        {
        }

        // Method that is called when the item is double-clicked
        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile player && Destination != null)
            {
                player.Location = Destination.Location;
                player.SendMessage("You have been teleported to another location!");
            }
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    public class FCSTele00Beta : AddonComponent
    {
        // constructor
        [Constructable]
        public FCSTele00Beta() : base(41108)
        {
            Name = "Master Bedroom";
            // Set the destination to this instance
            FCSTele00Alpha.Destination = this;
        }

        // Serialization constructor
        public FCSTele00Beta(Serial serial) : base(serial)
        {
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
    #endregion

    #region TeleporterCommand and FCSTele00AlphaMove and FCSTele00BetaDest
    public class TeleporterCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("SetTeleporter", AccessLevel.GameMaster, new CommandEventHandler(SetTeleporter_OnCommand));
        }

        [Usage("SetTeleporter")]
        [Description("Sets the destination of the teleporter.")]
        private static void SetTeleporter_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.SendMessage("Please select the location for FCSTele00BetaDest.");
            from.Target = new TeleporterTargetBeta();
        }

        private class TeleporterTargetBeta : Target
        {
            public TeleporterTargetBeta() : base(-1, true, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point3D)
                {
                    FCSTele00BetaDest destination = new FCSTele00BetaDest();
                    destination.MoveToWorld(new Point3D(point3D), from.Map);
                    FCSTele00AlphaMove.Destination = destination;
                    from.SendMessage("FCSTele00BetaDest set! Please select the location for FCSTele00AlphaMove.");
                    from.Target = new TeleporterTargetAlpha();
                }
            }
        }

        private class TeleporterTargetAlpha : Target
        {
            public TeleporterTargetAlpha() : base(-1, true, TargetFlags.None) { }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point3D)
                {
                    FCSTele00AlphaMove alphaMove = new FCSTele00AlphaMove();
                    alphaMove.MoveToWorld(new Point3D(point3D), from.Map);
                    from.SendMessage("FCSTele00AlphaMove set!");
                }
            }
        }
    }

    public class FCSTele00AlphaMove : AddonComponent
    {
        // Static reference to the target teleporter
        public static FCSTele00BetaDest Destination;

        // Constructor
        [Constructable]
        public FCSTele00AlphaMove() : base(41108)
        {
            Name = "Master Bedroom";
        }

        // Serialization constructor
        public FCSTele00AlphaMove(Serial serial) : base(serial) { }

        // Method called when a mobile is moved over the item
        public override bool OnMoveOver(Mobile from)
        {
            if (from is PlayerMobile player && Destination != null)
            {
                player.Location = Destination.Location;
                player.SendMessage("Du wurdest an einen anderen Ort teleportiert!");
            }
            return base.OnMoveOver(from);
        }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }

    public class FCSTele00BetaDest : AddonComponent
    {
        // Constructor
        [Constructable]
        public FCSTele00BetaDest() : base(41108)
        {
            Name = "Master Bedroom";
        }

        // Serialization constructor
        public FCSTele00BetaDest(Serial serial) : base(serial) { }

        // Serialization method
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        // Deserialization method
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
    #endregion
}

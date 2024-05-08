using System;
using System.Linq;
using Server.Gumps;
using Server.Items;
using Server.Spells;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.Custom.Misc
{
    public class MovingPlatform : Item
    {
        private bool isMoving = false;

        private bool isMovingHome = false;

        private bool soundActive = false;

        private Timer moveTimer;

        private Timer passengerTimer;

        private Timer soundTimer;

        private Direction oppositeDir = Direction.Up;

        private readonly List<Mobile> Passengers = new List<Mobile>();

        private BaseAddon addonHandle;

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseAddon AddonHandle
        {
            get
            {
                return addonHandle;
            }
            set
            {
                addonHandle = value;

                if (value != null)
                {
                    Visible = false;

                    UpdateAddonMultiLocation();
                }
                else
                {
                    Visible = true;
                }
            }
        }

        private int addonOffSetX;

        [CommandProperty(AccessLevel.GameMaster)]
        public int AddonOffSetX
        {
            get
            {
                return addonOffSetX;
            }
            set
            {
                addonOffSetX = value;

                if (addonHandle != null)
                {
                    UpdateAddonMultiLocation();
                }
            }
        }

        private int addonOffSetY;

        [CommandProperty(AccessLevel.GameMaster)]
        public int AddonOffSetY
        {
            get
            {
                return addonOffSetY;
            }
            set
            {
                addonOffSetY = value;

                if (addonHandle != null)
                {
                    UpdateAddonMultiLocation();
                }
            }
        }

        private int addoOffSetZ;

        [CommandProperty(AccessLevel.GameMaster)]
        public int AddonOffSetZ
        {
            get
            {
                return addoOffSetZ;
            }
            set
            {
                addoOffSetZ = value;

                if (addonHandle != null)
                {
                    UpdateAddonMultiLocation();
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsActive { get; set; } = false;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MoveSound { get; set; } = 0xED;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsAutoReturn { get; set; } = false;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool IsElevator { get; set; } = false;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool MountsAllowed { get; set; } = false;

        [CommandProperty(AccessLevel.GameMaster)]
        public Direction InitDirection { get; set; } = Direction.Up;

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D HomeLocation { get; set; } = Point3D.Zero;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxDistance { get; set; } = 10;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxPassengers { get; set; } = 1;

        private int moveSpeed = 1000;

        [CommandProperty(AccessLevel.GameMaster)]
        public int MoveSpeed
        {
            get { return moveSpeed; }
            set
            {
                if (value >= 100)
                {
                    moveSpeed = value;
                }
                else
                {
                    moveSpeed = 100;
                }
            }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PlatformItemID
        {
            get { return ItemID; }
            set { ItemID = value; }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PlatformHue
        {
            get { return Hue; }
            set { Hue = value; }
        }

        public static void Initialize()
        {
            EventSink.ServerStarted += EventSink_ServerStarted;

            EventSink.Logout += EventSink_Logout;
        }

        private static void EventSink_ServerStarted()
        {
            if (World.Items.Values.Any(p => p is MovingPlatform))
            {
                var movePlatformList = World.Items.Values.Where(p => p is MovingPlatform).ToList();

                for (int i = 0; i < movePlatformList.Count; i++)
                {
                    if (movePlatformList[i] is MovingPlatform mp)
                    {
                        mp.ResetPosition();
                    }
                }
            }
        }

        private static void EventSink_Logout(LogoutEventArgs e)
        {
            if (World.Items.Values.Any(p => p is MovingPlatform))
            {
                var movePlatformList = World.Items.Values.Where(p => p is MovingPlatform).ToList();

                for (int i = 0; i < movePlatformList.Count; i++)
                {
                    if (movePlatformList[i] is MovingPlatform mp)
                    {
                        if (mp.Passengers.Contains(e.Mobile))
                        {
                            e.Mobile.Location = mp.HomeLocation;
                        }
                    }
                }
            }
        }

        private void ResetPosition()
        {
            moveTimer?.Stop();

            soundTimer?.Stop();

            passengerTimer?.Stop();

            Location = HomeLocation;

            if (Passengers.Count > 0)
            {
                UpdatePassengers(isMovingHome);
            }

            isMoving = false;

            isMovingHome = false;

            if (addonHandle != null)
            {
                addonHandle.Location = Location;
            }
        }

        [Constructable]
        public MovingPlatform() : base(0x07BD)
        {
            Name = "Platform";

            Weight = 1.0;
        }

        public override bool OnDragLift(Mobile from)
        {
            if (from.AccessLevel < AccessLevel.GameMaster || isMoving || isMovingHome)
            {
                return false;
            }

            return base.OnDragLift(from);
        }

        public override void OnLocationChange(Point3D oldLocation)
        {
            if (!isMoving && !isMovingHome)
            {
                if (addonHandle != null)
                {
                    UpdateAddonMultiLocation();
                }

                HomeLocation = Location;
            }

            base.OnLocationChange(oldLocation);
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile pm && !isMoving)
            {
                if (IsActive && InRange(pm, 1))
                {
                    if (!MountsAllowed && pm.Mounted)
                    {
                        pm.SendMessage(53, "No room for mounts here!");

                        return;
                    }

                    if (pm.Map != Map.Felucca && (pm.Criminal || pm.Murderer))
                    {
                        pm.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.

                        return;
                    }

                    if (SpellHelper.CheckCombat(pm))
                    {
                        pm.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??

                        return;
                    }

                    if (pm.Spell != null)
                    {
                        pm.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.

                        return;
                    }

                    if (pm.Holding != null)
                    {
                        pm.SendMessage(53, "You cannot use while dragging an object.");

                        return;
                    }

                    oppositeDir = GetOppositeDirection(InitDirection);

                    if (!Passengers.Contains(pm))
                    {
                        AddPassenger(pm);

                        if (Passengers.Count == 1)
                        {
                            if (MaxPassengers != 1)
                            {
                                passengerTimer = Timer.DelayCall(TimeSpan.FromMilliseconds(1500), () =>
                                {
                                    isMoving = true;

                                    MovePlatform();
                                });
                            }
                            else
                            {
                                isMoving = true;

                                MovePlatform();
                            }
                        }
                    }
                }
                else
                {
                    if (pm.AccessLevel >= AccessLevel.GameMaster)
                    {
                        from.SendGump(new PropertiesGump(from, this));

                        return;
                    }

                    if (IsActive)
                    {
                        from.SendMessage(53, "You need to be closer!");
                    }
                    else
                    {
                        from.SendMessage(53, "Platform is not activated!");
                    }
                }
            }
            else
            {
                from.SendMessage(53, "Wait for platform to finish moving!");
            }
        }

        public override bool HandlesOnSpeech => addonHandle != null;

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (e.Speech.ToLower() == "start" || e.Speech.ToLower() == "move")
            {
                OnDoubleClick(e.Mobile);
            }

            base.OnSpeech(e);
        }

        private void AddPassenger(PlayerMobile pm)
        {
            pm.Location = Location;

            pm.Frozen = true;

            pm.Blessed = true;

            Passengers.Add(pm);
        }

        private bool stopMoving;

        private void MovePlatform()
        {
            if (isMoving)
            {
                stopMoving = false;

                if (isMovingHome)
                {
                    if (IsElevator)
                    {
                        if (Z == HomeLocation.Z)
                        {
                            UpdatePassengers(false);

                            stopMoving = true;
                        }
                    }
                    else
                    {
                        if (GetDistanceToSqrt(HomeLocation) == 0)
                        {
                            UpdatePassengers(false);

                            stopMoving = true;
                        }
                    }
                }
                else
                {
                    if (IsElevator)
                    {
                        if (Z >= HomeLocation.Z + MaxDistance)
                        {
                            UpdatePassengers(true);

                            if (!IsAutoReturn)
                            {
                                stopMoving = true;
                            }
                        }
                    }
                    else
                    {
                        if (GetDistanceToSqrt(HomeLocation) >= MaxDistance)
                        {
                            UpdatePassengers(true);

                            if (!IsAutoReturn)
                            {
                                stopMoving = true;
                            }
                        }
                    }
                }

                if (stopMoving)
                {
                    isMoving = false;

                    return;
                }
                else
                {
                    UpdateMove();

                    if (!soundActive)
                    {
                        soundActive = true;

                        Effects.PlaySound(Location, Map, MoveSound);

                        soundTimer = Timer.DelayCall(TimeSpan.FromMilliseconds(1000), () =>
                        {
                            soundActive = false;
                        });
                    }

                    moveTimer = Timer.DelayCall(TimeSpan.FromMilliseconds(moveSpeed), () =>
                    {
                        MovePlatform();
                    });
                }
            }
        }

        private void UpdateMove()
        {
            if (InRange(HomeLocation, MaxDistance + 1) && Z < HomeLocation.Z + MaxDistance + 1)
            {
                if (IsElevator)
                {
                    if (isMovingHome)
                    {
                        Z--;
                    }
                    else
                    {
                        Z++;
                    }
                }
                else
                {
                    X += GetOffset(isMovingHome ? oppositeDir : InitDirection, out int y);

                    Y += y;
                }

                if (addonHandle != null)
                {
                    UpdateAddonMultiLocation();
                }

                for (int i = 0; i < Passengers.Count; i++)
                {
                    Passengers[i].Location = Location;
                }
            }
            else
            {
                ResetPosition();
            }
        }

        private void UpdateAddonMultiLocation()
        {
            addonHandle.X = (X + addonOffSetX);

            addonHandle.Y = (Y + addonOffSetY);

            addonHandle.Z = (Z + addoOffSetZ);
        }

        private void UpdatePassengers(bool returning)
        {
            isMovingHome = returning;

            for (int i = 0; i < Passengers.Count; i++)
            {
                Passengers[i].Frozen = false;

                Passengers[i].Blessed = false;

                Passengers[i].Location = Location;
            }

            Passengers.Clear();
        }

        public override void OnDelete()
        {
            addonHandle?.Delete();

            base.OnDelete();
        }

        public MovingPlatform(Serial serial) : base(serial)
        {
        }

        private Direction GetOppositeDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    {
                        return Direction.South;
                    }
                case Direction.Right:
                    {
                        return Direction.Left;
                    }
                case Direction.East:
                    {
                        return Direction.West;
                    }
                case Direction.Down:
                    {
                        return Direction.Mask;
                    }
                case Direction.South:
                    {
                        return Direction.North;
                    }
                case Direction.Left:
                    {
                        return Direction.Right;
                    }
                case Direction.West:
                    {
                        return Direction.East;
                    }
                case Direction.Mask:
                    {
                        return Direction.Down;
                    }
                default:
                    {
                        return Direction.Up;
                    }
            }
        }

        private int GetOffset(Direction direction, out int offset_Y)
        {
            switch (direction)
            {
                case Direction.North:
                    {
                        offset_Y = -1;

                        return 0;
                    }
                case Direction.Right:
                    {
                        offset_Y = -1;

                        return 1;
                    }
                case Direction.East:
                    {
                        offset_Y = 0;

                        return 1;
                    }
                case Direction.Down:
                    {
                        offset_Y = 1;

                        return 1;
                    }
                case Direction.South:
                    {
                        offset_Y = 1;

                        return 0;
                    }
                case Direction.Left:
                    {
                        offset_Y = 1;

                        return -1;
                    }
                case Direction.West:
                    {
                        offset_Y = 0;

                        return -1;
                    }
                case Direction.Mask:
                    {
                        offset_Y = -1;

                        return -1;
                    }
                default:
                    {
                        offset_Y = 0;

                        return 0;
                    }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(isMoving);

            writer.Write(isMovingHome);

            writer.Write(oppositeDir.ToString());

            writer.Write(IsActive);

            writer.Write(MoveSound);

            writer.Write(IsAutoReturn);

            writer.Write(IsElevator);

            writer.Write(MountsAllowed);

            writer.Write(InitDirection.ToString());

            writer.Write(HomeLocation);

            writer.Write(MaxDistance);

            writer.Write(MaxPassengers);

            writer.Write(moveSpeed);

            if (addonHandle != null)
            {
                writer.Write(addonHandle.Serial);
            }
            else
            {
                writer.Write(0);
            }

            writer.Write(addonOffSetX);

            writer.Write(addonOffSetY);

            writer.Write(addoOffSetZ);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            isMoving = reader.ReadBool();

            isMovingHome = reader.ReadBool();

            oppositeDir = (Direction)Enum.Parse(typeof(Direction), reader.ReadString());

            IsActive = reader.ReadBool();

            MoveSound = reader.ReadInt();

            IsAutoReturn = reader.ReadBool();

            IsElevator = reader.ReadBool();

            MountsAllowed = reader.ReadBool();

            InitDirection = (Direction)Enum.Parse(typeof(Direction), reader.ReadString());

            HomeLocation = reader.ReadPoint3D();

            MaxDistance = reader.ReadInt();

            MaxPassengers = reader.ReadInt();

            moveSpeed = reader.ReadInt();

            var serial = reader.ReadInt();

            if (serial > 0)
            {
                if (World.Items.ContainsKey(serial))
                {
                    if (World.Items[serial] is BaseAddon bs)
                    {
                        addonHandle = bs;
                    }
                }
            }

            addonOffSetX = reader.ReadInt();

            addonOffSetY = reader.ReadInt();

            addoOffSetZ = reader.ReadInt();

            if (addonHandle != null)
            {
                UpdateAddonMultiLocation();
            }
        }
    }
}

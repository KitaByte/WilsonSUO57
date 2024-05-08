using System;

namespace Server.Items
{
    public class ConfettiCannon : Item
    {
        public override int LabelNumber { get { return 1124875; } } // Confetti Cannon

        private bool dropConfetti = true;

        private bool inUse = false;

        private Point3D end = Point3D.Zero;

        private int maxHeight = 0;

        private int height = 0;

        private int hue = 0;

        [Constructable]
        public ConfettiCannon() : base(0x9F93)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from.InRange(Location, 2))
            {
                if (!inUse)
                {
                    Effects.PlaySound(Location, Map, 0x11C);

                    inUse = true;

                    maxHeight = Utility.RandomMinMax(25, 65);

                    height = 10;

                    hue = Utility.RandomBrightHue();

                    Hue = hue;

                    Timer.DelayCall(TimeSpan.FromMilliseconds(150), () =>
                    {
                        Effects.PlaySound(Location, Map, 0xFC);

                        StartSeq();
                    });
                }
            }
            else
            {
                from.SendLocalizedMessage(1076766); // That is too far away.
            }
        }

        private void StartSeq()
        {
            end = new Point3D(Location.X, Location.Y, Location.Z + height);

            Timer.DelayCall(TimeSpan.FromMilliseconds(50), () =>
            {
                if (height < maxHeight)
                {
                    Effects.SendLocationEffect(end, Map, 0x9F88, 1, hue, 0);

                    height++;

                    StartSeq();
                }
                else
                {
                    end = new Point3D(Location.X, Location.Y, Location.Z + (maxHeight - 15));

                    Effects.PlaySound(end, Map, Utility.RandomList(0x3E, 0x3F));

                    Effects.SendLocationEffect(end, Map, 0x9F89, 18, hue, 0);

                    Timer.DelayCall(TimeSpan.FromMilliseconds(100), () =>
                    {
                        DropConfetti();
                    });
                }
            });
        }

        private void DropConfetti()
        {
            if (dropConfetti)
            {
                Point3D loc;

                int x;
                int y;

                for (int i = 0; i < Utility.RandomMinMax(1, 3); i++)
                {
                    Bandage confetti = new Bandage()
                    {
                        Name = "Confetti",
                        Hue = hue
                    };

                    x = Utility.RandomMinMax(-2, 2);

                    y = Utility.RandomMinMax(-2, 2);

                    if (x == 0 && y == 0)
                    {
                        if (Utility.RandomDouble() < 0.5)
                        {
                            x = 1;
                        }
                        else
                        {
                            y = 1;
                        }
                    }

                    loc = new Point3D(Location.X + x, Location.Y + y, Map.GetAverageZ(Location.X + x, Location.Y + y));

                    confetti.MoveToWorld(loc, Map);
                }
            }

            inUse = false;

            Hue = 0;
        }

        public ConfettiCannon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            reader.ReadInt();
        }
    }
}

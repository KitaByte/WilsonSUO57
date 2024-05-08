using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Misc
{
    public class RandomJoint : Item
    {
        private int puffs;

        private int bacMod;

        [Constructable]
        public RandomJoint() : base(Utility.RandomList(0x0F42, 0x0F3E, 0x0F3F, 0x0F8A, 0x1BD4, 0x1BDD, 0x1BE0, 0x1BFB, 0x1BFE, 0x1BFF))
        {
            Hue = Utility.RandomList(2498, 2500, 1153);

            switch (Hue)
            {
                case 2500: Name = "Pinner"; puffs = Utility.RandomMinMax(5, 10); break;
                case 2498: Name = "Spliff"; puffs = Utility.RandomMinMax(10, 20); break;
                case 1153: Name = "Blunt"; puffs = Utility.RandomMinMax(20, 25); break;
            }

            bacMod = 100 / puffs;
        }

        public RandomJoint(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (IsChildOf(typeof(Backpack)))
            {
                from.SendMessage("This must be in your backpack, in order to puff!");

                return;
            }

            if (from.Mounted)
            {
                from.SendMessage("Puffing while riding is against the law!");

                return;
            }

            if (from.Body.IsHuman)
            {
                if (puffs <= 0)
                {
                    if (Utility.RandomDouble() < 0.25)
                    {
                        from.Damage(1);

                        from.SendMessage(42, "The roach burns your fingers!");
                    }
                    else
                    {
                        from.SendMessage(52, "You drop the roach to the ground!");
                    }

                    Delete();
                }
                else
                {
                    puffs--;
                }

                if (from is PlayerMobile pm)
                {
                    pm.RevealingAction();

                    if (Utility.RandomDouble() < 0.1)
                    {
                        if (pm.Female)
                        {
                            pm.PlaySound(Utility.RandomList(0x310, 0x311, 0x312));
                        }
                        else
                        {
                            pm.PlaySound(Utility.RandomList(0x41F, 0x420, 0x421)); // 420 built into UO for cough, could it be? No ... LOL
                        }
                    }
                    else
                    {
                        pm.PlaySound(Utility.RandomList(0x11F, 0x120, 0x2B));
                    }

                    pm.Animate(AnimationType.Eat, 0);

                    pm.FixedEffect(Utility.RandomList(0x3728, 0x9DAC), Utility.RandomMinMax(1, 10), Utility.RandomMinMax(5, 30), Utility.RandomBrightHue(), 0);

                    pm.BAC += bacMod;

                    if (pm.BAC >= 60)
                    {
                        BaseBeverage.CheckHeaveTimer(pm);
                    }
                    else
                    {
                        if (Utility.RandomDouble() < 0.1)
                        {
                            pm.SendMessage(Utility.RandomBrightHue(), Utility.RandomList("Your feeling high!", "Your getting stoned!", "You see a pink elephant!"));
                        }
                    }
                }
            }
            else
            {
                from.SendMessage("Silly, only humans can puff!");
            }

            base.OnDoubleClick(from);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);

            writer.Write(puffs);

            writer.Write(bacMod);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            puffs = reader.ReadInt();

            if (version > 0)
            {
                bacMod = reader.ReadInt();
            }
        }
    }
}

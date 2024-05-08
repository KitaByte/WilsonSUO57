using System;
using System.Linq;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Custom.Misc
{
    public class FeatherfetcherGloves : LeatherGloves
    {
        private enum BirdType
        {
            Bird,
            Eagle
        }

        private BirdType _Bird;

        private bool isEquiped = false;

        private Timer _MoveTimer;

        private Mobile _CurrentBird;

        [Constructable]
        public FeatherfetcherGloves()
        {
            Name = "Featherfetcher Gloves";

            Hue = Utility.RandomBirdHue();

            if (Utility.RandomDouble() < 0.1)
            {
                _Bird = BirdType.Eagle;

                Name += " of Speed";
            }
            else
            {
                _Bird = BirdType.Bird;
            }
        }

        public FeatherfetcherGloves(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (isEquiped && from.Alive && _MoveTimer == null)
            {
                from.SendMessage(53, "Target corpse to send feathered friend to loot!");

                from.Target = new FFTarget(this);
            }
            else
            {
                if (!isEquiped)
                {
                    from.SendMessage(43, "Gloves must be equipped!");
                }
                else if (!from.Alive)
                {
                    from.SendMessage(43, "You must be alive!");
                }
                else if (_MoveTimer != null)
                {
                    from.SendMessage(43, "Your feathered friend is currently busy fetching loot!");
                }
            }

            base.OnDoubleClick(from);
        }

        public override bool OnEquip(Mobile from)
        {
            isEquiped = true;

            return base.OnEquip(from);
        }

        public override void OnRemoved(object parent)
        {
            isEquiped = false;

            base.OnRemoved(parent);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)_Bird);

            writer.Write(isEquiped);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            _Bird = (BirdType)reader.ReadInt();

            isEquiped = reader.ReadBool();
        }

        private class FFTarget : Target
        {
            private readonly FeatherfetcherGloves _Gloves;

            public FFTarget(FeatherfetcherGloves gloves) : base(20, false, TargetFlags.None)
            {
                _Gloves = gloves;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Corpse c && (c.Killer == from || c.Killer is BaseCreature bc && bc.ControlMaster == from))
                {
                    if (_Gloves._Bird == BirdType.Bird)
                    {
                        _Gloves._CurrentBird = new Bird();
                    }
                    else
                    {
                        _Gloves._CurrentBird = new Eagle();
                    }

                    _Gloves._CurrentBird.MoveToWorld(from.Location, from.Map);

                    MoveBird(from, _Gloves, c);
                }
                else
                {
                    from.SendMessage(43, "That is not a corpse you killed!");
                }
            }

            private static void MoveBird(Mobile from, FeatherfetcherGloves gloves, Corpse c)
            {
                gloves._CurrentBird.Move(gloves._CurrentBird.GetDirectionTo(c));

                if (gloves._CurrentBird.Location != c.Location)
                {
                    int time = gloves._CurrentBird.GetType() == typeof(Eagle) ? 250 : 500;

                    gloves._MoveTimer = Timer.DelayCall(TimeSpan.FromMilliseconds(time), () =>
                    {
                        MoveBird(from, gloves, c);
                    });
                }
                else
                {
                    if (c.Items.Count > 0)
                    {
                        int count = c.Items.Count;

                        for (int i = 0; i < count; i++)
                        {
                            if (c.Items.First() is Item item)
                            {
                                if (item.Amount > 1)
                                {
                                    from.SendMessage(62, $"Your feathered friend found {item.Amount} {item.GetType().Name}");
                                }
                                else
                                {
                                    from.SendMessage(62, $"Your feathered friend found a {item.GetType().Name}");
                                }

                                from.AddToBackpack(item);
                            }
                        }

                        from.SendMessage(53, "Your feathered friend magically sent the loot to your backpack!");
                    }
                    else
                    {
                        from.SendMessage(43, "Your feathered friend found nothing!");
                    }

                    gloves._CurrentBird.Delete();

                    gloves._MoveTimer = null;

                    gloves._CurrentBird = null;
                }
            }
        }
    }
}

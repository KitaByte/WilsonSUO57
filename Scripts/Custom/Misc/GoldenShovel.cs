using System;
using System.Linq;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Custom
{
    internal class GoldenShovel : Item
    {
        public static void Initialize()
        {
            EventSink.CreatureDeath += EventSink_CreatureDeath;
        }

        private static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
        {
            if (e.Creature is GoldenElemental && Utility.RandomDouble() < 0.01)
            {
                e.Corpse.AddItem(new GoldenShovel());
            }
        }

        [CommandProperty(AccessLevel.Administrator)]
        public int Used { get; private set; }

        private DateTime lastTarget = DateTime.MinValue;

        private List<Snake> snakes;

        private void InitSnakes()
        {
            snakes = new List<Snake>();
        }

        private DirtPatch dirt;

        [Constructable]
        public GoldenShovel() : base(0xF39)
        {
            Name = "Golden Shovel";

            Hue = 2734;

            Weight = 5.0;

            Used = 1;
        }

        public override void AddNameProperty(ObjectPropertyList list)
        {
            list.Add($"{Name}");

            list.Add($"Chance {Used * 0.1}%"); 
        }

        public override void OnSingleClick(Mobile from)
        {
            LabelTo(from, $"{Name}");

            LabelTo(from, $"Chance {Used * 0.1}%"); 
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.Alive)
            {
                from.SendMessage(43, "Must be alive!");

                return;
            }

            if (RootParent != from)
            {
                from.SendMessage(43, "Must be in backpack!");

                return;
            }

            if (from.Mounted)
            {
                from.SendMessage(43, "Must not be mounted!");

                return;
            }

            if (!from.HasFreeHand())
            {
                from.SendMessage(43, "Must have free hand!");

                return;
            }

            if (from.Hidden)
            {
                from.SendMessage(43, "Must not be hidden!");

                return;
            }

            double skillMod = Used * 0.1 / 2;

            if (skillMod > 50)
            {
                skillMod = 50;
            }

            if (from.Skills.Mining.Value < skillMod)
            {
                from.SendMessage(43, "Your mining skill is too low!");

                return;
            }

            if (lastTarget < DateTime.Now - TimeSpan.FromSeconds(Utility.RandomMinMax(10, 25)))
            {
                from.Target = new ArtifactTarget(this);
            }
            else
            {
                from.SendMessage(33, $"{from.Name}, you are exhaust from digging!");
            }

            CleanUpSnakes();
        }

        private void CleanUpSnakes()
        {
            if (snakes != null && snakes.Count > 0)
            {
                for (int i = 0; i < snakes.Count; i++)
                {
                    if (snakes[i].Combatant == null)
                    {
                        snakes[i].Delete();
                    }
                }

                var tempList = snakes.Where(s => !s.Deleted)?.ToList();

                snakes.Clear();

                if (tempList != null && tempList.Count > 0)
                {
                    snakes.AddRange(tempList);
                }
            }
        }

        private Item RandomGoldenStatue()
        {
            MonsterStatuette statue = new MonsterStatuette((MonsterStatuetteType)Utility.Random(Enum.GetValues(typeof(MonsterStatuetteType)).Length - 1));

            statue.Name = $"Golden {statue.Name}";

            statue.Hue = 2734;

            return statue;
        }

        public GoldenShovel(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(Used);

            CleanUpSnakes();
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Used = reader.ReadInt();
        }

        private class ArtifactTarget : Target
        {
            private readonly GoldenShovel _Shovel;

            public ArtifactTarget(GoldenShovel sovel) : base (3, true, TargetFlags.None)
            {
                _Shovel = sovel;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (from.Alive && !from.Mounted && targeted is LandTarget lt)
                {
                    if (from.Body.IsHuman)
                    {
                        from.Direction = from.GetDirectionTo(lt.Location);

                        if (Core.SA)
                        {
                            from.Animate(AnimationType.Attack, 3);
                        }
                        else
                        {
                            from.Animate(11, 5, 1, true, false, 0);
                        }

                        from.PlaySound(Utility.RandomList(0x125, 0x126));

                        if (_Shovel.dirt == null || _Shovel.dirt.Deleted)
                        {
                            _Shovel.dirt = new DirtPatch();

                            _Shovel.dirt.MoveToWorld(lt.Location, from.Map);

                            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                            {
                                _Shovel.dirt.Delete();
                            });
                        }
                    }

                    double chance = _Shovel.Used * 0.001 + 0.01;

                    if (chance > 1.0)
                    {
                        chance = 0.999;
                    }

                    if (Utility.RandomDouble() < chance)
                    {
                        _Shovel.Used++;

                        _Shovel.lastTarget = DateTime.Now;

                        int reward = _Shovel.Used > 100 ? Utility.RandomMinMax(1, 100) : Utility.RandomMinMax(1, _Shovel.Used);

                        if (reward == 100 && Utility.RandomDouble() < 0.01)
                        {
                            from.SendMessage(63, $"{from.Name}, you found a golden statue!");

                            from.AddToBackpack(_Shovel.RandomGoldenStatue());
                        }
                        else
                        {
                            if (_Shovel.Used > 199)
                            {
                                if (_Shovel.Used < 1000)
                                {
                                    reward *= (int)(chance / 0.1);
                                }
                                else
                                {
                                    reward *= 10;
                                }
                            }

                            from.SendMessage(63, $"{from.Name}, you found {reward} gold coins!");

                            from.AddToBackpack(new Gold(reward));
                        }

                        Timer.DelayCall(TimeSpan.FromMilliseconds(250), () =>
                        {
                            from.PlaySound(Utility.RandomList(0x5A6, 0x5A7));
                        });
                    }
                    else
                    {
                        if (Utility.RandomDouble() < 0.3)
                        {
                            Timer.DelayCall(TimeSpan.FromMilliseconds(250), () =>
                            {
                                if (from.Female)
                                {
                                    from.PlaySound(Utility.RandomList(0x324, 0x325, 0x326, 0x327, 0x328, 0x329, 0x32A));
                                }
                                else
                                {
                                    from.PlaySound(Utility.RandomList(0x434, 0x435, 0x436, 0x437, 0x438, 0x439, 0x43A, 0x43B, 0x43C));
                                }
                            });
                        }

                        if (Utility.RandomDouble() < chance)
                        {
                            int num = Utility.RandomMinMax(2, 5 + (int)(chance / 0.1));

                            if (_Shovel.snakes == null)
                            {
                                _Shovel.InitSnakes();
                            }

                            for (int i = 0; i < num; i++)
                            {
                                Snake snake = new Snake()
                                {
                                    Name = "Golden Snake",

                                    IsParagon = true,

                                    Combatant = from
                                };

                                snake.MoveToWorld(lt.Location, from.Map);

                                _Shovel.snakes.Add(snake);
                            }

                            from.SendMessage(53, $"{from.Name}, you found a nest of golden snakes!");
                        }
                        else
                        {
                            from.SendMessage(53, $"{from.Name}, you found nothng!");
                        }
                    }
                }
                else
                {
                    from.SendMessage(53, $"{from.Name}, you can only target the ground!");
                }
            }
        }
    }
}

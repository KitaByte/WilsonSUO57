using System;
using System.Linq;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Collections.Generic;
using Server.Custom.InfusionSystem.Infusions;

namespace Server.Custom.InfusionSystem
{
    public class BaseInfusion : Item
    {
        public InfusionType Infusion { get; private set; } = InfusionType.Base;

        [Constructable]
        public BaseInfusion() : this((InfusionType)Utility.RandomMinMax(1, Enum.GetValues(typeof(InfusionType)).Length))
        {
        }

        [Constructable]
        public BaseInfusion(InfusionType infusion) : base(InfusionInfo.GetID())
        {
            Infusion = infusion;

            SetStats();
        }

        private void SetStats()
        {
            Name = $"{InfusionInfo.GetInfo(Infusion).name} Infused Crystals";

            Hue = InfusionInfo.GetInfo(Infusion).hue;

            Stackable = false;

            Weight = 0.1;
        }

        public BaseInfusion(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!Movable)
            {
                if (Utility.RandomDouble() < 0.1)
                {
                    from.PlaySound(0xFE);

                    from.SendMessage(53, "The sound of Sosaria can still be heard in the shattered crystals!");
                }

                return;
            }

            if (IsChildOf(from.Backpack))
            {
                from.Target = new InternalTarget(this);
            }
            else
            {
                if (from.InRange(Location, 3))
                {
                    from.Backpack?.AddItem(this);

                    from.SendMessage("The crystals magically teleport into your backpack!");
                }
                else
                {
                    from.SendMessage("You see sparkling crystals emanating energy!");
                }
            }

            base.OnDoubleClick(from);
        }

        public static void AddInfusion(PlayerMobile pm, Item item, InfusionType infusion)
        {
            if (pm == null || item == null)
            {
                return;
            }

            if (item.Name == null)
            {
                item.Name = InfusionCore.AddSpaces(item.GetType().Name);
            }

            if (item.Name.EndsWith("Infused]"))
            {
                pm.SendMessage(53, $"That {item.Name} is already infused with {InfusionInfo.GetInfo(infusion).name}!");

                return;
            }

            pm.SendMessage(53, $"The {item.Name} has been infused with {InfusionInfo.GetInfo(infusion).name}!");

            item.Name = $"{item.Name} [{InfusionInfo.GetInfo(infusion).name} Infused]";

            item.Weight = item.Hue * 0.0001 + (int)item.Weight;

            item.Hue = InfusionInfo.GetInfo(infusion).hue;

            pm.PlaySound(Utility.RandomList(0x3E, 0x3F));

            pm.Animate(AnimationType.Attack, 0);
        }

        public static bool ActivateInfusion(PlayerMobile pm, Item item, InfusionType infusion, Mobile target = null)
        {
            List<Item> items = pm.Backpack?.Items;

            if (items != null && items.Count > 0)
            {
                List<Item> infusions = new List<Item>();

                List<Item> bags = new List<Item>();

                try
                {
                    if (items.Any(i => i is BaseInfusion))
                    {
                        infusions.AddRange(items.Where(i => i is BaseInfusion)?.ToList());
                    }

                    if (items.Any(i => i is BaseContainer))
                    {
                        bags.AddRange(items.Where(i => i is BaseContainer)?.ToList());
                    }

                    if (bags.Count > 0)
                    {
                        foreach (var bag in bags)
                        {
                            if (bag.Items.Count > 0)
                            {
                                if (bag.Items.Any(i => i is BaseInfusion))
                                {
                                    infusions.AddRange(bag.Items.Where(i => i is BaseInfusion)?.ToList());
                                }
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }

                if (infusions.Count > 0)
                {
                    List<Item> infusedCrystals;

                    switch (infusion)
                    {
                        case InfusionType.Cold:
                            {
                                infusedCrystals = infusions.FindAll(inf => inf is ColdInfusion).ToList();

                                if (infusedCrystals.Count > 0)
                                {
                                    ApplyDamage(target, infusedCrystals, InfusionType.Cold);

                                    target.FixedEffect(0x3789, 2, 20, InfusionInfo.GetInfo(infusion).hue, 0);

                                    Effects.PlaySound(target.Location, target.Map, Utility.RandomList(0x5C6, 0x5C7, 0x5BE));
                                }

                                CheckInfusion(pm, item);

                                break;
                            }
                        case InfusionType.Fire:
                            {
                                infusedCrystals = infusions.FindAll(inf => inf is FireInfusion).ToList();

                                if (infusedCrystals.Count > 0)
                                {
                                    ApplyDamage(target, infusedCrystals, InfusionType.Fire);

                                    target.FixedEffect(0x3709, 1, 30, InfusionInfo.GetInfo(infusion).hue, 0);

                                    Effects.PlaySound(target.Location, target.Map, Utility.RandomList(0x225, 0x226, 0x227));
                                }

                                CheckInfusion(pm, item);

                                break;
                            }
                        case InfusionType.Wind:
                            {
                                infusedCrystals = infusions.FindAll(inf => inf is WindInfusion).ToList();

                                if (infusedCrystals.Count > 0)
                                {
                                    ApplyDamage(target, infusedCrystals, InfusionType.Wind);

                                    target.FixedEffect(0x37CC, 2, 25, InfusionInfo.GetInfo(infusion).hue, 0);

                                    Effects.PlaySound(target.Location, target.Map, Utility.RandomList(0x14, 0x15, 0x16));
                                }

                                CheckInfusion(pm, item);

                                break;
                            }
                        case InfusionType.Earth:
                            {
                                infusedCrystals = infusions.FindAll(inf => inf is EarthInfusion).ToList();

                                if (infusedCrystals.Count > 0)
                                {
                                    ApplyDamage(target, infusedCrystals, InfusionType.Earth);

                                    target.Frozen = true;

                                    target.SendMessage(53, $"You are stuck!");

                                    target.FixedEffect(0x322C, 4, 9, InfusionInfo.GetInfo(infusion).hue, 0);

                                    Effects.PlaySound(target.Location, target.Map, Utility.RandomList(0x20, 0x21));

                                    Timer.DelayCall(TimeSpan.FromSeconds(Utility.Random(3)), () =>
                                    {
                                        target.SendMessage(53, $"You are freed!");

                                        target.Frozen = false;
                                    });
                                }

                                CheckInfusion(pm, item);

                                break;
                            }
                        case InfusionType.Magic:
                            {
                                infusedCrystals = infusions.FindAll(inf => inf is MagicInfusion).ToList();

                                if (infusedCrystals.Count > 0)
                                {
                                    ApplyDamage(target, infusedCrystals, InfusionType.Magic);

                                    target.FixedEffect(0x375A, 1, 16, InfusionInfo.GetInfo(infusion).hue, 0);

                                    Effects.PlaySound(target.Location, target.Map, Utility.RandomList(0x1F8, 0x1F9));
                                }

                                CheckInfusion(pm, item);

                                break;
                            }
                        case InfusionType.Poison:
                            {
                                infusedCrystals = infusions.FindAll(inf => inf is PoisonInfusion).ToList();

                                if (infusedCrystals.Count > 0)
                                {
                                    ApplyDamage(target, infusedCrystals, InfusionType.Poison);

                                    if (Utility.RandomDouble() < 0.01)
                                    {
                                        target.ApplyPoison(pm, Utility.RandomList(Poison.Lethal, Poison.Deadly));
                                    }
                                    else
                                    {
                                        target.ApplyPoison(pm, Utility.RandomList(Poison.Lesser, Poison.Regular));
                                    }

                                    target.FixedEffect(0x3735, 8, 5, InfusionInfo.GetInfo(infusion).hue, 0);

                                    Effects.PlaySound(target.Location, target.Map, Utility.RandomList(0x474, 0x3E9));
                                }

                                CheckInfusion(pm, item);

                                break;
                            }
                        case InfusionType.Rot:
                            {
                                infusedCrystals = infusions.FindAll(inf => inf is RotInfusion).ToList();

                                if (infusedCrystals.Count > 0)
                                {
                                    ApplyDamage(target, infusedCrystals, InfusionType.Rot);

                                    target.ApplyPoison(pm, Poison.Parasitic);

                                    target.FixedEffect(0x3728, 1, 13, InfusionInfo.GetInfo(infusion).hue, 0);

                                    Effects.PlaySound(target.Location, target.Map, Utility.RandomList(0x1C8, 0x1C9));
                                }

                                CheckInfusion(pm, item);

                                break;
                            }
                        case InfusionType.Death:
                            {
                                infusedCrystals = infusions.FindAll(inf => inf is DeathInfusion).ToList();

                                if (infusedCrystals.Count > 0)
                                {
                                    ApplyDamage(target, infusedCrystals, InfusionType.Death);

                                    target.ApplyPoison(pm, Poison.DarkGlow);

                                    target.FixedEffect(0x37C4, 1, 8, InfusionInfo.GetInfo(infusion).hue, 0);

                                    Effects.PlaySound(target.Location, target.Map, Utility.RandomList(0x451, 0x452));

                                    if (Utility.RandomDouble() < 0.01)
                                    {
                                        pm.SendMessage(53, $"Your {item.GetType().Name} broke under the stress of the infusion!");

                                        item.Delete();
                                    }
                                }

                                if (!item.Deleted)
                                {
                                    CheckInfusion(pm, item);
                                }

                                break;
                            }
                    }
                }
            }

            return false;
        }

        private static void CheckInfusion(PlayerMobile pm, Item item)
        {
            if (Utility.RandomDouble() < 0.01)
            {
                pm.SendMessage(53, $"Your {item.GetType().Name} infusion wore off!");

                RemoveInfusion(pm, item);
            }
        }

        private static void ApplyDamage(Mobile target, List<Item> infusedCrystals, InfusionType infusion)
        {
            if (target.Alive)
            {
                var max = infusedCrystals.Count > 10 ? 10 : infusedCrystals.Count;

                var damageMod = Utility.RandomMinMax(1, max);

                for (int i = 0; i < damageMod; i++)
                {
                    if (infusedCrystals.Count > 0)
                    {
                        infusedCrystals.Last().Delete();

                        infusedCrystals.Remove(infusedCrystals.Last());
                    }
                }

                target.SendMessage(InfusionInfo.GetInfo(infusion).hue, $"Your are infused by {InfusionInfo.GetInfo(infusion).name}");

                target.Damage(damageMod);
            }
        }

        public static void RemoveInfusion(PlayerMobile pm, Item item)
        {
            if (pm == null || item == null || item?.Name == null)
            {
                return;
            }

            if (item.Name != null && !item.Name.EndsWith("Infused]"))
            {
                pm.SendMessage(53, $"That {item.Name} is not infused!");

                return;
            }

            pm.SendMessage(53, $"{item.Name.Split('[').First().Trim()}'s infusion has been removed!");

            item.Name = null;

            if (item.Weight > 0)
            {
                item.Hue = (int)((item.Weight - (int)(item.Weight)) / 0.0001);
            }
            else
            {
                item.Hue = 0;
            }

            item.Weight = (int)(item.Weight);

            pm.PlaySound(0x4F);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)Infusion);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            Infusion = (InfusionType)reader.ReadInt();
        }

        private class InternalTarget : Target
        {
            private readonly BaseInfusion m_Infusion;

            public InternalTarget(BaseInfusion infusion) : base(3, false, TargetFlags.None)
            {
                m_Infusion = infusion;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_Infusion == null || m_Infusion.Deleted)
                    return;

                if (from is PlayerMobile pm && targeted is Item item)
                {
                    if (pm.Skills.Tinkering.Value * 0.01 > Utility.RandomDouble() && pm.Skills.Alchemy.Value * 0.01 > Utility.RandomDouble())
                    {
                        if (item is BaseWeapon || item is BaseArmor || item is BaseClothing || item is BaseJewel || item is BaseHarvestTool || item is BaseTool)
                        {
                            if (item.GetType().GetConstructors().Any(c => c.GetParameters().Length == 0))
                            {
                                AddInfusion(pm, item, m_Infusion.Infusion);

                                m_Infusion.Delete();
                            }
                            else
                            {
                                pm.SendMessage(53, "Can't infuse that!");
                            }
                        }
                        else
                        {
                            pm.SendMessage(53, "Can't infuse that!");
                        }
                    }
                    else
                    {
                        if (Utility.RandomDouble() > pm.Skills.Tinkering.Value * 0.01)
                        {
                            pm.SendMessage(53, $"You failed to infuse the {item.GetType().Name} & broke the crystal the process!");

                            m_Infusion.Delete();
                        }
                        else
                        {
                            pm.SendMessage(53, $"You failed to infuse the {item.GetType().Name}!");
                        }
                    }
                }
            }
        }
    }
}

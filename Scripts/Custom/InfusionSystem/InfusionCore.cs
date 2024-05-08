using System;
using System.Text;
using Server.Mobiles;
using Server.Custom.InfusionSystem.Items;
using Server.Custom.InfusionSystem.Infusions;

namespace Server.Custom.InfusionSystem
{
    internal static class InfusionCore
    {
        public static void Initialize()
        {
            EventSink.AggressiveAction += EventSink_AggressiveAction;

            EventSink.ResourceHarvestSuccess += EventSink_ResourceHarvestSuccess;

            EventSink.CraftSuccess += EventSink_CraftSuccess;

            EventSink.CreatureDeath += EventSink_CreatureDeath;
        }

        private static void EventSink_AggressiveAction(AggressiveActionEventArgs e)
        {
            if (e.Aggressor is PlayerMobile pm)
            {
                for (int i = 0x00; i < Enum.GetValues(typeof(Layer)).Length; i++)
                {
                    Item item = e.Aggressor.FindItemOnLayer((Layer)i);

                    if (item != null && HasInfusionHue(item))
                    {
                        OnInfusionAction(pm, item, 0.5, item.Hue, e.Aggressed, true);
                    }
                }
            }
        }

        private static bool HasInfusionHue(Item item)
        {
            return InfusionInfo.GetInfusion(item.Hue) != InfusionType.Base;
        }

        private static void EventSink_ResourceHarvestSuccess(ResourceHarvestSuccessEventArgs e)
        {
            if (e.Harvester is PlayerMobile pm && e.Tool != null)
            {
                OnInfusionAction(pm, e.Tool, 0.01, e.Tool.Hue);
            }
        }

        private static void EventSink_CraftSuccess(CraftSuccessEventArgs e)
        {
            if (e.Crafter is PlayerMobile pm && e.Tool != null)
            {
                OnInfusionAction(pm, e.Tool, 0.01, e.Tool.Hue);
            }
        }

        private static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
        {
            if (e.Creature.CanSwim && Utility.RandomDouble() < 0.01)
            {
                e.Corpse.AddItem(new InfusionSponge());
            }
        }

        private static void OnInfusionAction(PlayerMobile pm, Item item, double chance, int hue, Mobile target = null, bool isUsing = false)
        {
            if (Utility.RandomDouble() < chance)
            {
                switch (InfusionInfo.GetInfusion(hue))
                {
                    case InfusionType.Base:
                        {
                            if (Utility.RandomDouble() < 0.05)
                            {
                                if (isUsing)
                                {
                                    BaseInfusion.ActivateInfusion(pm, item, InfusionType.Base, target);

                                    break;
                                }

                                pm.Backpack?.AddItem(new BaseInfusion());
                            }

                            break;
                        }

                    case InfusionType.Cold:
                        {
                            if (Utility.RandomDouble() < InfusionInfo.GetChance(InfusionType.Cold))
                            {
                                if (isUsing)
                                {
                                    BaseInfusion.ActivateInfusion(pm, item, InfusionType.Cold, target);

                                    break;
                                }

                                pm.Backpack?.AddItem(new ColdInfusion());
                            }

                            break;
                        }

                    case InfusionType.Fire:
                        {
                            if (Utility.RandomDouble() < InfusionInfo.GetChance(InfusionType.Fire))
                            {
                                if (isUsing)
                                {
                                    BaseInfusion.ActivateInfusion(pm, item, InfusionType.Fire, target);

                                    break;
                                }

                                pm.Backpack?.AddItem(new FireInfusion());
                            }

                            break;
                        }

                    case InfusionType.Wind:
                        {
                            if (Utility.RandomDouble() < InfusionInfo.GetChance(InfusionType.Wind))
                            {
                                if (isUsing)
                                {
                                    BaseInfusion.ActivateInfusion(pm, item, InfusionType.Wind, target);

                                    break;
                                }

                                pm.Backpack?.AddItem(new WindInfusion());
                            }

                            break;
                        }

                    case InfusionType.Earth:
                        {
                            if (Utility.RandomDouble() < InfusionInfo.GetChance(InfusionType.Earth))
                            {
                                if (isUsing)
                                {
                                    BaseInfusion.ActivateInfusion(pm, item, InfusionType.Earth, target);

                                    break;
                                }

                                pm.Backpack?.AddItem(new EarthInfusion());
                            }

                            break;
                        }

                    case InfusionType.Magic:
                        {
                            if (Utility.RandomDouble() < InfusionInfo.GetChance(InfusionType.Magic))
                            {
                                if (isUsing)
                                {
                                    BaseInfusion.ActivateInfusion(pm, item, InfusionType.Magic, target);

                                    break;
                                }

                                pm.Backpack?.AddItem(new MagicInfusion());
                            }

                            break;
                        }

                    case InfusionType.Poison:
                        {
                            if (Utility.RandomDouble() < InfusionInfo.GetChance(InfusionType.Poison))
                            {
                                if (isUsing)
                                {
                                    BaseInfusion.ActivateInfusion(pm, item, InfusionType.Poison, target);

                                    break;
                                }

                                pm.Backpack?.AddItem(new PoisonInfusion());
                            }

                            break;
                        }

                    case InfusionType.Rot:
                        {
                            if (Utility.RandomDouble() < InfusionInfo.GetChance(InfusionType.Rot))
                            {
                                if (isUsing)
                                {
                                    BaseInfusion.ActivateInfusion(pm, item, InfusionType.Rot, target);

                                    break;
                                }

                                pm.Backpack?.AddItem(new RotInfusion());
                            }

                            break;
                        }

                    case InfusionType.Death:
                        {
                            if (Utility.RandomDouble() < InfusionInfo.GetChance(InfusionType.Death))
                            {
                                if (isUsing)
                                {
                                    BaseInfusion.ActivateInfusion(pm, item, InfusionType.Death, target);

                                    break;
                                }

                                pm.Backpack?.AddItem(new DeathInfusion());
                            }

                            break;
                        }
                }
            }
        }

        public static string AddSpaces(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder(input.Length * 2);

            result.Append(input[0]);

            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                {
                    result.Append(' ');
                }
                result.Append(input[i]);
            }

            return result.ToString();
        }
    }
}

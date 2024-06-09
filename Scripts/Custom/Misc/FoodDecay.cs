using System;
using System.Linq;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Misc
{
    internal static class FoodDecay
    {
        // Settings
        private static readonly TimeSpan ts_DecayRate = TimeSpan.FromDays(7);

        private static readonly bool b_CanBeLucky = true;

        private static readonly int i_LuckBonus = 20;

        public static void Configure()
        {
            EventSink.WorldSave += e => Persistence.Serialize(@"Saves\\FoodDecay\\LastDecay.bin", Save);

            EventSink.WorldLoad += () => Persistence.Deserialize(@"Saves\\FoodDecay\\LastDecay.bin", Load);
        }

        public static void Initialize()
        {
            EventSink.AfterWorldSave += EventSink_AfterWorldSave;

            if (b_CanBeLucky)
            {
                EventSink.OnConsume += EventSink_OnConsume;

                EventSink.HungerChanged += EventSink_HungerChanged;
            }
        }

        private static DateTime dt_LastDecay = DateTime.MinValue;

        private static void EventSink_AfterWorldSave(AfterWorldSaveEventArgs e)
        {
            if (dt_LastDecay + ts_DecayRate < DateTime.Now)
            {
                var foodEffected = World.Items.Values.Where(f => f is Food food && food.PlayerConstructed).ToList();

                if (foodEffected != null && foodEffected.Count > 0)
                {
                    for (int i = 0; i < foodEffected.Count; i++)
                    {
                        if (foodEffected[i] is Food f)
                        {
                            if (f.FillFactor > 0)
                            {
                                if (f.Quality != ItemQuality.Exceptional)
                                {
                                    if (f.Quality != ItemQuality.Normal)
                                    {
                                        f.FillFactor--;
                                    }
                                    else
                                    {
                                        f.Quality = ItemQuality.Low;
                                    }
                                }
                                else
                                {
                                    f.Quality = ItemQuality.Normal;
                                }
                            }
                            else
                            {
                                if (f.Poison == null)
                                {
                                    if (Utility.RandomDouble() < 0.1)
                                    {
                                        f.Poison = Poison.Parasitic;

                                        f.Hue = Utility.RandomOrangeHue();
                                    }
                                }
                                else
                                {
                                    f.Poison = GetPoisonLevel(f.Poison);

                                    f.Hue = Utility.RandomGreenHue();
                                }
                            }
                        }
                    }
                }

                dt_LastDecay = DateTime.Now;
            }
        }

        private static Poison GetPoisonLevel(Poison poison)
        {
            if (poison == Poison.Parasitic)
            {
                return Poison.Lesser;
            }

            if (poison == Poison.Lesser)
            {
                return Poison.Regular;
            }

            if (poison == Poison.Regular)
            {
                return Poison.Greater;
            }

            if (poison == Poison.Greater)
            {
                return Poison.Deadly;
            }

            return Poison.Lethal;
        }

        private static void EventSink_OnConsume(OnConsumeEventArgs e)
        {
            if (e.Consumed is Food food && food.PlayerConstructed)
            {
                if (food.Quality == ItemQuality.Exceptional && e.Consumer.Hunger > 19)
                {
                    if (e.Consumer is PlayerMobile pm && pm.RealLuck < Math.Abs(1000 - i_LuckBonus))
                    {
                        TryApplyLuckBonus(pm);
                    }
                }
            }
        }

        private static void TryApplyLuckBonus(PlayerMobile pm)
        {
            if (!(pm.Items.Find(o => o is FoodLuckItem) is FoodLuckItem luckItem))
            {
                luckItem = new FoodLuckItem();

                if (pm.EquipItem(luckItem))
                {
                    pm.SendAsciiMessage(Utility.RandomYellowHue(), "Fortune smiles upon thee!");

                    pm.PlaySound(0x5AA);
                }
                else
                {
                    luckItem.Delete();
                }
            }
        }

        private static void EventSink_HungerChanged(HungerChangedEventArgs e)
        {
            if (e.Mobile is PlayerMobile pm && pm.Hunger < 20 && pm.FindItemOnLayer(Layer.Invalid) != null)
            {
                if (pm.Items.Find(o => o is FoodLuckItem) is FoodLuckItem luckItem)
                {
                    pm.SendAsciiMessage(Utility.RandomRedHue(), "Your fortune wares off!");

                    pm.PlaySound(0x5AA);

                    luckItem.Delete();
                }
            }
        }

        private static void Save(GenericWriter writer)
        {
            writer.Write(0);

            writer.Write(dt_LastDecay);
        }

        private static void Load(GenericReader reader)
        {
            reader.ReadInt();

            dt_LastDecay = reader.ReadDateTime();
        }

        private class FoodLuckItem : BaseJewel
        {
            public FoodLuckItem() : base(0x1, Layer.Invalid)
            {
                Visible = false;

                Movable = false;

                Attributes.Luck = i_LuckBonus;

                LootType = LootType.Blessed;
            }

            public override bool CanEquip(Mobile from)
            {
                if (from is PlayerMobile && from.Alive)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public FoodLuckItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);

                Delete();
            }
        }
    }
}

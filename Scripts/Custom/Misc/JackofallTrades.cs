using System;
using Server.Items;
using System.Collections;

namespace Server.Custom.Misc
{
    public class JackofallTrades : Mobile
    {
        private const int CostPerRes = 5000;

        private double ResSkillMod => GetResSkillMod();

        private double GetResSkillMod()
        {
            if (Title.Contains("Master"))
            {
                if (Title.Contains("Grand"))
                {
                    return 0.01;
                }

                return 0.1;
            }

            return 0.25;
        }

        private readonly ArrayList CraftTitles = new ArrayList() { "the", "the Master", "the Grand Master" };

        [Constructable]
        public JackofallTrades() : base()
        {
            Title = $"{CraftTitles[Utility.Random(2)]} Crafter";

            InitStats(31, 41, 51);

            Hue = Utility.RandomSkinHue();

            SpeechHue = Utility.RandomDyedHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;

                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;

                Name = NameList.RandomName("male");
            }

            SetDress();

            SetHair();

            YellowHealthbar = false;

            Container pack = new Backpack
            {
                Movable = false
            };

            AddItem(pack);
        }

        private void SetHair()
        {
            Utility.AssignRandomHair(this);

            HairHue = Utility.RandomHairHue();

            if (!Female)
            {
                Utility.AssignRandomFacialHair(this);

                FacialHairHue = HairHue;
            }
        }

        private void SetDress()
        {
            if (Female)
            {
                AddItem(new WideBrimHat(Utility.RandomBirdHue()));

                AddItem(new PlainDress(Utility.RandomBirdHue()));

                AddItem(new Sandals(Utility.RandomBirdHue()));
            }
            else
            {
                AddItem(new Bandana(Utility.RandomMetalHue()));

                AddItem(new Shirt(Utility.RandomMetalHue()));

                AddItem(new ShortPants(Utility.RandomMetalHue()));

                AddItem(new Sandals(Utility.RandomMetalHue()));
            }

            if (Title.Contains("Master"))
            {
                if (Title.Contains("Grand"))
                {
                    AddItem(new BodySash(2734));
                }
                else
                {
                    AddItem(new BodySash(Utility.RandomBrightHue()));
                }
            }
        }

        private DateTime speechDelay = DateTime.MinValue;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (DateTime.Now > speechDelay && Utility.RandomDouble() < 0.1)
            {
                speechDelay = DateTime.Now + TimeSpan.FromMinutes(3);

                if (Title.Contains("Master"))
                {
                    if (Title.Contains("Master"))
                    {
                        Say("I am a Grand Master, the Best there is!");
                    }
                    else
                    {
                        Say("I am a Master Crafter!");
                    }
                }
                else
                {
                    Say("I am a Crafter!");
                }

                Say("Looking for work, can upgrade your gear!");
                Say("Drop gear on me for estimate!");
            }

            base.OnMovement(m, oldLocation);
        }

        private CraftResource _res;

        private Item loadedItem;

        private int costMod;

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            Direction = GetDirectionTo(from);

            if (!dropped.IsArtifact)
            {
                if (dropped is BaseArmor ba)
                {
                    _res = TryIncRes(ba.Resource, from, ba, ba.GetType().Name);

                    if (!ba.Deleted)
                    {
                        ba.Resource = _res;
                    }

                    return base.OnDragDrop(from, dropped);
                }

                if (dropped is BaseWeapon wep)
                {
                    _res = TryIncRes(wep.Resource, from, wep, wep.GetType().Name);

                    if (!wep.Deleted)
                    {
                        wep.Resource = _res;
                    }

                    return base.OnDragDrop(from, dropped);
                }

                if (dropped is BaseShield shield)
                {
                    _res = TryIncRes(shield.Resource, from, shield, shield.GetType().Name);

                    if (!shield.Deleted)
                    {
                        shield.Resource = _res;
                    }

                    return base.OnDragDrop(from, dropped);
                }

                if (dropped is BaseJewel jewel)
                {
                    _res = TryIncRes(jewel.Resource, from, jewel, jewel.GetType().Name);

                    if (!jewel.Deleted)
                    {
                        jewel.Resource = _res;
                    }

                    return base.OnDragDrop(from, dropped);
                }
            }

            Say("I can't work on that!");

            return base.OnDragDrop(from, dropped);
        }

        private bool GetGold(Mobile from)
        {
            var gold = from.Backpack.FindItemByType(typeof(Gold));

            if (gold != null && gold.Amount > costMod * CostPerRes)
            {
                gold.Amount -= (costMod * CostPerRes);

                return true;
            }
            else
            {
                return false;
            }
        }

        private CraftResource TryIncRes(CraftResource res, Mobile from, Item item, string name)
        {
            if (loadedItem == null || loadedItem != item)
            {
                if (res < GetMaxRes(res))
                {
                    costMod = Math.Abs(GetMinRes(res) - res) + 1;

                    loadedItem = item;

                    Say($"The {name} will cost {costMod * CostPerRes} gold to upgrade!");
                }
                else
                {
                    from.SendMessage(53, $"That {name} is already maxed out!");

                    loadedItem = null;
                }
            }
            else
            {
                if (GetGold(from))
                {
                    if (Utility.RandomDouble() < ResSkillMod)
                    {
                        item.Delete();

                        Say($"Damn, Sorry, I broke your {name}!");

                        Gold gold = new Gold(costMod * CostPerRes + 1000);

                        from.AddToBackpack(gold);

                        from.PlaySound(gold.GetDropSound());

                        Say($"I refunded the gold for your {name}, plus a bit extra for the trouble!");
                    }
                    else
                    {
                        var tempRes = res;

                        tempRes++;

                        if (GetMaxRes(res) >= tempRes) // Sanity
                        {
                            from.SendMessage(53, $"Increased to {tempRes}!");

                            from.PlaySound(0x2A);

                            loadedItem = null;

                            return tempRes;
                        }
                        else
                        {
                            from.SendMessage(53, $"The {name} is maxed out!");
                        }
                    }
                }
                else
                {
                    Say($"You don't have enough gold to upgrade the {name}!");
                }

                loadedItem = null;
            }

            return res;
        }

        private CraftResource GetMinRes(CraftResource res)
        {
            switch (res)
            {
                case CraftResource.None: return CraftResource.None;
                case CraftResource.Iron: return CraftResource.Iron;
                case CraftResource.DullCopper: return CraftResource.Iron;
                case CraftResource.ShadowIron: return CraftResource.Iron;
                case CraftResource.Copper: return CraftResource.Iron;
                case CraftResource.Bronze: return CraftResource.Iron;
                case CraftResource.Gold: return CraftResource.Iron;
                case CraftResource.Agapite: return CraftResource.Iron;
                case CraftResource.Verite: return CraftResource.Iron;
                case CraftResource.Valorite: return CraftResource.Iron;
                case CraftResource.RegularLeather: return CraftResource.RegularLeather;
                case CraftResource.SpinedLeather: return CraftResource.RegularLeather;
                case CraftResource.HornedLeather: return CraftResource.RegularLeather;
                case CraftResource.BarbedLeather: return CraftResource.RegularLeather;
                case CraftResource.RedScales: return CraftResource.RedScales;
                case CraftResource.YellowScales: return CraftResource.RedScales;
                case CraftResource.BlackScales: return CraftResource.RedScales;
                case CraftResource.GreenScales: return CraftResource.RedScales;
                case CraftResource.WhiteScales: return CraftResource.RedScales;
                case CraftResource.BlueScales: return CraftResource.RedScales;
                case CraftResource.RegularWood: return CraftResource.RegularWood;
                case CraftResource.OakWood: return CraftResource.RegularWood;
                case CraftResource.AshWood: return CraftResource.RegularWood;
                case CraftResource.YewWood: return CraftResource.RegularWood;
                case CraftResource.Heartwood: return CraftResource.RegularWood;
                case CraftResource.Bloodwood: return CraftResource.RegularWood;
                case CraftResource.Frostwood: return CraftResource.RegularWood;
            }

            return res;
        }

        private CraftResource GetMaxRes(CraftResource res)
        {
            switch (res)
            {
                case CraftResource.None: return CraftResource.None;
                case CraftResource.Iron: return CraftResource.Valorite;
                case CraftResource.DullCopper: return CraftResource.Valorite;
                case CraftResource.ShadowIron: return CraftResource.Valorite;
                case CraftResource.Copper: return CraftResource.Valorite;
                case CraftResource.Bronze: return CraftResource.Valorite;
                case CraftResource.Gold: return CraftResource.Valorite;
                case CraftResource.Agapite: return CraftResource.Valorite;
                case CraftResource.Verite: return CraftResource.Valorite;
                case CraftResource.Valorite: return CraftResource.Valorite;
                case CraftResource.RegularLeather: return CraftResource.BarbedLeather;
                case CraftResource.SpinedLeather: return CraftResource.BarbedLeather;
                case CraftResource.HornedLeather: return CraftResource.BarbedLeather;
                case CraftResource.BarbedLeather: return CraftResource.BarbedLeather;
                case CraftResource.RedScales: return CraftResource.BlueScales;
                case CraftResource.YellowScales: return CraftResource.BlueScales;
                case CraftResource.BlackScales: return CraftResource.BlueScales;
                case CraftResource.GreenScales: return CraftResource.BlueScales;
                case CraftResource.WhiteScales: return CraftResource.BlueScales;
                case CraftResource.BlueScales: return CraftResource.BlueScales;
                case CraftResource.RegularWood: return CraftResource.Frostwood;
                case CraftResource.OakWood: return CraftResource.Frostwood;
                case CraftResource.AshWood: return CraftResource.Frostwood;
                case CraftResource.YewWood: return CraftResource.Frostwood;
                case CraftResource.Heartwood: return CraftResource.Frostwood;
                case CraftResource.Bloodwood: return CraftResource.Frostwood;
                case CraftResource.Frostwood: return CraftResource.Frostwood;
            }

            return res;
        }

        public JackofallTrades(Serial serial) : base(serial)
        {
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
}

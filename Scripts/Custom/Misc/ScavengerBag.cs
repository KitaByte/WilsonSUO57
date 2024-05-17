using System.Collections.Generic;
using Server.Mobiles;
using System.Linq;
using System.Text;

namespace Server.Items
{
    public class ScavengerBag : Bag
    {
        private List<(string Name, int Count)> CurrentList;

        private bool HasList => CurrentList != null;

        private readonly int BaseRewardAmount = 25;

        private int TimesRewarded = 0;

        public override bool DisplaysContent => false;

        public override bool DisplayWeight => false;

        public override bool DisplayLootType => false;

        [CommandProperty(AccessLevel.GameMaster)]
        public bool ResetList
        {
            get { return false; }
            set
            {
                ResetBagList();
            }
        }

        private void ResetBagList()
        {
            CreateList();
        }

        [Constructable]
        public ScavengerBag()
        {
            Name = "Scavenger Bag - Level 0";

            Hue = 2500;

            LootType = LootType.Blessed;

            CreateList();
        }

        public override void Open(Mobile from)
        {
            if (!HasList)
            {
                CreateList();
            }

            SendSavengerList(from);
        }

        private void SendSavengerList(Mobile from)
        {
            from.SendMessage(43, $"Level {TimesRewarded} Scavenger List");

            foreach (var item in CurrentList)
            {
                if (item.Count > 1)
                {
                    from.SendMessage(53, $"{item.Count} X {SeparateWords(item.Name)}'s");
                }
                else
                {
                    from.SendMessage(53, $"{item.Count} X {SeparateWords(item.Name)}");
                }
            }
        }

        private void CreateList()
        {
            if (HasList)
            {
                CurrentList.Clear();
            }
            else
            {
                CurrentList = new List<(string, int)>();
            }

            var itemList = World.Items.Values.ToList();

            if (itemList.Count > 100)
            {
                Item item;

                int rndItem;

                int needed;

                for (int i = 0; i < itemList.Count; i++)
                {
                    rndItem = Utility.RandomMinMax(0, itemList.Count - 1);

                    item = itemList[rndItem];

                    if (IsUnique(item.GetType().Name) && IsGoodBase(item))
                    {
                        needed = Utility.RandomMinMax(1, 2 + TimesRewarded) * TimesRewarded == 0 ? 1 : TimesRewarded;

                        CurrentList.Add((item.GetType().Name, needed));
                    }

                    if (CurrentList.Count > 9)
                    {
                        break;
                    }
                }
            }
        }

        private bool IsUnique(string name)
        {
            foreach (var item in CurrentList)
            {
                if (item.Name == name)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsGoodBase(Item item)
        {
            if (item is BaseWeapon || item is BaseArmor || item is BaseJewel || item is BaseClothing)
            {
                if (item is IVvVItem)
                {
                    return false;
                }

                return true;
            }


            if (item is BaseReagent || item is Food || item is BasePotion)
            {
                return true;
            }

            return false;
        }

        public override bool TryDropItem(Mobile from, Item dropped, bool sendFullMessage)
        {
            if (RootParent == null)
            {
                from.SendMessage(53, "Must be in your backpack!");

                return false;
            }

            if (HasList && from is PlayerMobile pm)
            {
                if (ValidateScavengerItem(dropped, pm))
                {
                    if (CurrentList.Count == 0)
                    {
                        RewardPlayer(pm);

                        CreateList();
                    }
                    else
                    {
                        pm.PlaySound(0x5A6);
                    }
                }
                else
                {
                    pm.PlaySound(0x5A5);
                }
            }

            return false;
        }

        private bool ValidateScavengerItem(Item item, PlayerMobile pm)
        {
            for (int i = 0; i < CurrentList.Count; i++)
            {
                if (CurrentList[i].Name == item.GetType().Name)
                {
                    if (CurrentList[i].Count > 1)
                    {
                        CurrentList[i] = (CurrentList[i].Name, CurrentList[i].Count - 1);

                        pm.SendMessage(43, $"{CurrentList[i].Name} : {CurrentList[i].Count} Left to find!");
                    }
                    else
                    {
                        pm.SendMessage(62, $"{CurrentList[i].Name} : Completed!");

                        CurrentList.RemoveAt(i);
                    }

                    if (item.Amount > 1)
                    {
                        item.Amount--;

                        pm.AddToBackpack(item);
                    }
                    else
                    {
                        item.Delete();
                    }

                    return true;
                }
            }

            pm.AddToBackpack(item);

            pm.SendMessage(33, "That item is not in the scavenger list!");

            return false;
        }

        private void RewardPlayer(PlayerMobile pm)
        {
            TimesRewarded++;

            Name = $"Scavenger Bag - Level {TimesRewarded}";

            if (TimesRewarded > 99)
            {
                Hue = 2734;
            }

            int reward = BaseRewardAmount * TimesRewarded;

            pm.AddToBackpack(new Gold(reward));

            pm.SendMessage(53, $"You were rewarded {reward} gold for completing the scavenger list!");

            pm.PlaySound(0x5A7);

            Effects.SendBoltEffect(pm, false, Utility.RandomBrightHue());
        }

        public string SeparateWords(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var result = new StringBuilder();

            foreach (char c in input)
            {
                if (char.IsUpper(c) && result.Length > 0)
                {
                    result.Append(' ');
                }

                result.Append(c);
            }

            return result.ToString();
        }

        public ScavengerBag(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1);

            writer.Write(TimesRewarded);

            if (!HasList)
            {
                CreateList();
            }

            writer.Write(CurrentList.Count);

            foreach (var item in CurrentList)
            {
                writer.Write(item.Name);

                writer.Write(item.Count);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version > 0)
            {
                TimesRewarded = reader.ReadInt();

                int count = reader.ReadInt();

                CurrentList = new List<(string Name, int Count)>();

                for (int i = 0; i < count; i++)
                {
                    CurrentList.Add((reader.ReadString(), reader.ReadInt()));
                }
            }
        }
    }
}

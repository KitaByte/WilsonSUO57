using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;

namespace Server.Custom.Misc
{
    internal class PlayerKiller : BaseCreature
    {
        public override bool ClickTitle { get { return false; } }
        public override bool CanStealth { get { return true; } }
        public override bool AlwaysMurderer { get { return true; } }

        private DateTime m_NextWeaponChange;

        [Constructable]
        public PlayerKiller() : base(AIType.AI_Ninja, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();

            Hue = Utility.RandomSkinHue();

            Name = "an elite assasin";

            Body = (Female = Utility.RandomBool()) ? 0x191 : 0x190;

            Utility.AssignRandomHair(this);

            SetHits(251, 350);

            SetStr(126, 225);
            SetDex(81, 95);
            SetInt(151, 165);

            SetDamage(12, 20);

            SetDamageType(ResistanceType.Physical, 65);
            SetDamageType(ResistanceType.Fire, 15);
            SetDamageType(ResistanceType.Cold, 15);
            SetDamageType(ResistanceType.Poison, 15);
            SetDamageType(ResistanceType.Energy, 5);

            SetResistance(ResistanceType.Physical, 35, 65);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 25, 45);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 35, 55);

            SetSkill(SkillName.Anatomy, 105.0, 120.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 115.0, 130.0);
            SetSkill(SkillName.Wrestling, 95.0, 120.0);
            SetSkill(SkillName.Fencing, 95.0, 120.0);
            SetSkill(SkillName.Macing, 95.0, 120.0);
            SetSkill(SkillName.Swords, 95.0, 120.0);

            SetSkill(SkillName.Ninjitsu, 95.0, 120.0);
            SetSkill(SkillName.Hiding, 100.0);
            SetSkill(SkillName.Stealth, 120.0);

            Fame = 8500;
            Karma = -8500;

            LeatherNinjaBelt belt = new LeatherNinjaBelt
            {
                UsesRemaining = 20,
                Poison = Poison.Greater,
                PoisonCharges = 20,
                Movable = false
            };

            AddItem(belt);

            int amount = Skills[SkillName.Ninjitsu].Value >= 100 ? 2 : 1;

            for (int i = 0; i < amount; i++)
            {
                Fukiya f = new Fukiya
                {
                    UsesRemaining = 10,
                    Poison = amount == 1 ? Poison.Regular : Poison.Greater,
                    PoisonCharges = 10,
                    Movable = false
                };
                PackItem(f);
            }

            AddItem(new NinjaTabi());
            AddItem(new LeatherNinjaJacket());
            AddItem(new LeatherNinjaHood());
            AddItem(new LeatherNinjaPants());
            AddItem(new LeatherNinjaMitts());

            if (Utility.RandomDouble() < 0.33)
                PackItem(new SmokeBomb());

            if (Utility.RandomBool())
                PackItem(new Tessen());
            else
                PackItem(new Wakizashi());

            if (Utility.RandomBool())
                PackItem(new Nunchaku());
            else
                PackItem(new Daisho());

            if (Utility.RandomBool())
                PackItem(new Sai());
            else
                PackItem(new Tekagi());

            if (Utility.RandomBool())
                PackItem(new Kama());
            else
                PackItem(new Katana());

            ChangeWeapon();
        }

        public PlayerKiller(Serial serial) : base(serial)
        {
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
        }

        public override bool BardImmune { get { return true; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, 2);
        }

        private void ChangeWeapon()
        {
            if (Backpack == null)
                return;

            Item item = FindItemOnLayer(Layer.OneHanded) ?? FindItemOnLayer(Layer.TwoHanded);

            List<BaseWeapon> weapons = new List<BaseWeapon>();

            foreach (Item i in Backpack.Items)
            {
                if (i is BaseWeapon && i != item)
                    weapons.Add((BaseWeapon)i);
            }

            if (weapons.Count > 0)
            {
                if (item != null)
                    Backpack.DropItem(item);

                AddItem(weapons[Utility.Random(weapons.Count)]);

                m_NextWeaponChange = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 60));
            }

            ColUtility.Free(weapons);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && m_NextWeaponChange < DateTime.UtcNow)
                ChangeWeapon();
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
    }
}

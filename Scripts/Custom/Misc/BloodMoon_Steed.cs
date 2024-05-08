using Server.Items;
using Server.Spells;

namespace Server.Mobiles

{
    [CorpseName("a BloodMoon Steed corpse")]
    public class BloodMoonSteed : BaseMount
    {
        public override bool UseSmartAI => true;

        [Constructable]
        public BloodMoonSteed()  : this("BloodMoon Steed")
        {
        }

        [Constructable]
        public BloodMoonSteed(string name) : base(name, 0x74, 0x3EA7, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "BloodMoon Steed";

            BaseSoundID = Core.AOS ? 0xA8 : 0x16A;

            SetStr(725, 795);
            SetDex(100, 130);
            SetInt(300, 400);

            SetHits(480, 500);

            SetDamage(30, 35);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Fire, 25);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 80.1, 100.0);
            SetSkill(SkillName.Magery, 80.1, 100.0);
            SetSkill(SkillName.Meditation, 52.5, 75.0);
            SetSkill(SkillName.MagicResist, 100.0, 115.0);
            SetSkill(SkillName.Tactics, 97.6, 100.0);
            SetSkill(SkillName.Wrestling, 97.6, 100.0);
	        SetSkill(SkillName.Anatomy, 40.1, 50.0);

            Fame = 14000;
            Karma = -14000;

            VirtualArmor = 60;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 98.1;

            bool hueMod = Utility.RandomDouble() < 0.05; // 1955

            switch (Utility.Random(4))
            {
                case 0:
                    {
                        BodyValue = 116;
                        Hue = hueMod ? 1955 : 2734;
                        ItemID = 16039;
                        break;
                    }
                case 1:
                    {
                        BodyValue = 177;
                        Hue = hueMod ? 1955 : 2735;
                        ItemID = 16053;
                        break;
                    }
                case 2:
                    {
                        BodyValue = 178;
                        Hue = hueMod ? 1955 : 2736;
                        ItemID = 16041;
                        break;
                    }
                case 3:
                    {
                        BodyValue = 179;
                        Hue = hueMod ? 1955 : 2737;
                        ItemID = 16055;
                        break;
                    }
            }

            SetSpecialAbility(SpecialAbility.DragonBreath);

            PackReg(100, 200);
        }

        public BloodMoonSteed(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            if (Combatant != null)
            {
                if (Utility.RandomDouble() < 0.05)
                {
                    Spell spell = null;

                    switch (Utility.Random(10))
                    {
                        case 1:
                            {
                                spell = new Spells.Third.FireballSpell(this, new FireballScroll());

                                break;
                            }

                        case 2:
                            {
                                spell = new Spells.Seventh.FlameStrikeSpell(this, new FlamestrikeScroll());

                                break;
                            }

                        default:
                            break;
                    }

                    spell.Cast();
                }
            }
            else
            {
                base.OnThink();
            }
        }

        public override int Meat
        {
            get
            {
                return 5;
            }
        }

        public override int Hides
        {
            get
            {
                return 10;
            }
        }

        public override HideType HideType
        {
            get
            {
                return HideType.Barbed;
            }
        }

        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat;
            }
        }

        public override bool CanAngerOnTame
        {
            get
            {
                return true;
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Potions);
        }

        public override int GetAngerSound()
        {
            if (!Controlled)
                return 0x16A;

            return base.GetAngerSound();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}

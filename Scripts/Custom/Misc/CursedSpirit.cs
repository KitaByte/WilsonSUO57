using System.Collections.Generic;
using Server.Mobiles;
using System.Linq;
using System;

namespace Server.Custom.Misc
{
    [CorpseName("a cursed corpse")]
    internal class CursedSpirit : BaseCreature
    {
        private List<Mobile> RandomMobs;

        private Mobile boss = null;

        private MobLevel mobLevel = MobLevel.Normal;

        private bool isBoss = false;

        public enum MobLevel
        {
            Easy,
            Normal,
            Hard
        }

        [Constructable]
        public CursedSpirit() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            RandomMobs = new List<Mobile>();

            mobLevel = (MobLevel)Utility.RandomMinMax(0, 2);

            SetRandomList();

            if (RandomMobs.Count > 0)
            {
                SetStats(SetRandomInfo());
            }
        }

        public CursedSpirit(List<Mobile> randomMobs, Mobile _boss, MobLevel level, bool isboss) : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            RandomMobs = randomMobs;

            boss = _boss;

            mobLevel = level;

            isBoss = isboss;

            SetStats(SetRandomInfo());
        }

        private void SetRandomList()
        {
            List<Mobile> mobs = World.Mobiles.Values.Where(m => IsValidMob(m)).ToList();

            if (mobs.Count > 100)
            {
                Mobile topMob = null;

                for (int i = 0; i < 100; i++)
                {
                    Mobile mob = mobs[Utility.Random(mobs.Count - 1)];

                    RandomMobs.Add(mob);

                    if (topMob == null || topMob.Karma > mob.Karma)
                    {
                        topMob = mob;
                    }
                }

                boss = RandomMobs.Find(m => m.Name == topMob.Name);

                RandomMobs.Remove(boss);
            }
        }

        private bool IsValidMob(Mobile m)
        {
            if (m is BaseCreature bc)
            {
                if (bc.CanSwim) return false;

                switch (mobLevel)
                {
                    case MobLevel.Easy: if (bc.Karma > -100) return false; break;
                    case MobLevel.Normal:if (bc.Karma > -1000) return false; break;
                    case MobLevel.Hard: if (bc.Karma > -10000) return false; break;
                }

                if (bc.Name.Split(' ').Length > 1)
                {
                    if (bc.Name.StartsWith("a ") || bc.Name.StartsWith("an "))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private Mobile SetRandomInfo()
        {
            Mobile info = null;

            if (RandomMobs.Count > 0)
            {
                 info = RandomMobs[Utility.Random(RandomMobs.Count - 1)];

                try
                {
                    Name = $"[CURSED] {info.Name}";

                    Body = info.Body;

                    BaseSoundID = info.BaseSoundID;

                    Hue = Utility.RandomMetalHue();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Bad RandomMob = {info?.Name} : {ex.Message}");

                    info = this;
                }
            }

            return info;
        }

        public CursedSpirit(Serial serial) : base(serial)
        {
        }

        public override bool OnBeforeDeath()
        {
            if (!isBoss)
            {
                CursedSpirit mob = new CursedSpirit(RandomMobs, boss, mobLevel, isBoss);

                mob.UpdateStats(Deaths);

                mob.Combatant = Combatant;

                mob.MoveToWorld(Location, Map);

                if (isBoss)
                {
                    mob.Say("Meet your DOOOOOM!");
                }
                else
                {
                    mob.Say("My spirit lives on!");
                }

                Effects.SendLocationEffect(Location, Map, 0x37CC, 30, Hue, 0);

                Effects.PlaySound(Location, Map, 0x567);
            }

            return base.OnBeforeDeath();
        }

        public override void OnThink()
        {
            if (Deaths >= 0)
            {
                if (Combatant == null && GetClientsInRange(20).ToList().Count == 0)
                {
                    Delete();
                }
            }

            base.OnThink();
        }

        private void UpdateStats(int deaths)
        {
            Deaths = deaths + 1;

            double chance = 0.01 * Deaths;

            Mobile current;

            if (Deaths > 10 && Utility.RandomDouble() < chance)
            {
                Name = $"[BOSS] {boss.Name}";

                Body = boss.Body;

                BaseSoundID = boss.BaseSoundID;

                Hue = Utility.RandomBrightHue();

                IsParagon = true;

                Deaths += 10;

                isBoss = true;

                current = boss;
            }
            else
            {
                current = SetRandomInfo();

                if (current == null)
                {
                    current = this;
                }
            }

            NameHue = Hue;

            CorpseNameOverride = $"{Name} corpse";

            SetStats(current);
        }

        private void SetStats(Mobile m)
        {
            SetStr(m.RawStr + Deaths);
            SetDex(m.RawDex + Deaths);
            SetInt(m.RawInt + Deaths);

            SetHits(m.HitsMax * ((Deaths + 1) / 2));

            SetDamage(10 + Deaths);

            SetDamageType(ResistanceType.Physical, m.PhysicalResistance + ((Deaths + 1) / 2));
            SetDamageType(ResistanceType.Fire, m.FireResistance + ((Deaths + 1) / 2));
            SetDamageType(ResistanceType.Cold, m.ColdResistance + ((Deaths + 1) / 2));
            SetDamageType(ResistanceType.Poison, m.PoisonResistance + ((Deaths + 1) / 2));
            SetDamageType(ResistanceType.Energy, m.EnergyResistance + ((Deaths + 1) / 2));

            SetResistance(ResistanceType.Physical, m.PhysicalResistance + ((Deaths + 1) / 2));
            SetResistance(ResistanceType.Fire, m.FireResistance + ((Deaths + 1) / 2));
            SetResistance(ResistanceType.Cold, m.ColdResistance + ((Deaths + 1) / 2));
            SetResistance(ResistanceType.Poison, m.PoisonResistance + ((Deaths + 1) / 2));
            SetResistance(ResistanceType.Energy, m.EnergyResistance + ((Deaths + 1) / 2));

            SetSkill(SkillName.EvalInt, m.Skills.EvalInt.Value + ((Deaths + 1) / 2));
            SetSkill(SkillName.Magery, m.Skills.Magery.Value + ((Deaths + 1) / 2));
            SetSkill(SkillName.MagicResist, m.Skills.MagicResist.Value + ((Deaths + 1) / 2));
            SetSkill(SkillName.Tactics, m.Skills.Tactics.Value + ((Deaths + 1) / 2));
            SetSkill(SkillName.Wrestling, m.Skills.Wrestling.Value + ((Deaths + 1) / 2));

            Fame = m.Fame + (100 * Deaths);

            Karma = m.Karma - (100 * Deaths);

            VirtualArmor = m.VirtualArmor + ((Deaths + 1) / 10);

            if (isBoss)
            {
                AddLoot(LootPack.UltraRich, (Deaths / 5));
            }
            else
            {
                if (Deaths > 0)
                {
                    AddLoot(LootPack.FilthyRich, Deaths);
                }
            }

            AddLoot(LootPack.Gems, (Deaths + 1));

            PackGold(50 * (Deaths + 1));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            writer.Write((int)mobLevel);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            RandomMobs = new List<Mobile>();

            if (version >= 0)
            {
                mobLevel = (MobLevel)reader.ReadInt();

                SetRandomList();

                SetStats(SetRandomInfo());
            }
        }
    }
}

using Server.Mobiles;

namespace Server.Custom.WispPack
{
    internal static class WispUtility
    {
        internal static int GetResistanceDamage(int resistance, string title)
        {
            int damageMod = 100 - resistance;

            return (damageMod > 10 ? damageMod / 10 : 1) * GetTitleMod(title);
        }

        internal static void SetWispStats(BaseCreature wisp, ResistanceType resistance)
        {
            int mod = GetTitleMod(wisp);

            wisp.Body = 165;
            wisp.BaseSoundID = 466;
            wisp.VirtualArmor = 15 + (5 * mod);
            wisp.Fame = 1500 * mod;

            wisp.SetStr(15, 25 + (25 * mod));
            wisp.SetDex(15, 25 + (25 * mod));
            wisp.SetInt(15, 25 + (25 * mod));

            wisp.SetHits(25, 25 + (25 * mod));

            wisp.SetDamage(5, 10 + (5 * mod));

            SetDamageType(wisp, resistance);

            SetResistances(wisp, resistance);

            wisp.SetSkill(SkillName.EvalInt, 20.0 + (20 * mod));
            wisp.SetSkill(SkillName.Magery, 20.0 + (20 * mod));
            wisp.SetSkill(SkillName.Meditation, 20.0 + (20 * mod));
            wisp.SetSkill(SkillName.MagicResist, 20.0 + (20 * mod));
            wisp.SetSkill(SkillName.Tactics, 20.0 + (20 * mod));
            wisp.SetSkill(SkillName.Wrestling, 20.0 + (20 * mod));
        }

        private static int GetTitleMod(BaseCreature wisp)
        {
            if (Utility.RandomDouble() < 0.01)
            {
                wisp.Title = "[Elder]";
            }
            else
            {
                wisp.Title = Utility.RandomList("[Adult]", "[Child]");
            }

            return GetTitleMod(wisp.Title);
        }

        private static int GetTitleMod(string title)
        {
            int mod = 1;

            switch (title)
            {
                case "[Edlder]":
                    {
                        mod = 3;

                        break;
                    }

                case "[Adult]":
                    {
                        mod = 2;

                        break;
                    }
            }

            return mod;
        }

        private static void SetDamageType(BaseCreature wisp, ResistanceType resistance)
        {
            if (resistance != ResistanceType.Physical)
            {
                wisp.SetDamageType(ResistanceType.Physical, 20);
                wisp.SetDamageType(resistance, 80);
            }
            else
            {
                wisp.SetDamageType(ResistanceType.Physical, 80);
                wisp.SetDamageType(ResistanceType.Cold, 5);
                wisp.SetDamageType(ResistanceType.Fire, 5);
                wisp.SetDamageType(ResistanceType.Poison, 5);
                wisp.SetDamageType(ResistanceType.Energy, 5);
            }
        }

        private static void SetResistances(BaseCreature wisp, ResistanceType resistance)
        {
            wisp.SetResistance(ResistanceType.Physical, 50);

            wisp.SetResistance(ResistanceType.Cold, 100);
            wisp.SetResistance(ResistanceType.Fire, 100);
            wisp.SetResistance(ResistanceType.Poison, 100);
            wisp.SetResistance(ResistanceType.Energy, 100);

            switch (resistance)
            {
                case ResistanceType.Fire:
                    {
                        wisp.SetResistance(ResistanceType.Cold, 5);

                        break;
                    }

                case ResistanceType.Cold:
                    {
                        wisp.SetResistance(ResistanceType.Fire, 5);

                        break;
                    }

                case ResistanceType.Poison:
                    {
                        wisp.SetResistance(ResistanceType.Energy, 5);

                        break;
                    }

                case ResistanceType.Energy:
                    {
                        wisp.SetResistance(ResistanceType.Poison, 5);

                        break;
                    }

                case ResistanceType.Physical:
                    {
                        wisp.SetResistance(ResistanceType.Physical, 80);

                        wisp.SetResistance(ResistanceType.Cold, 20);
                        wisp.SetResistance(ResistanceType.Fire, 20);
                        wisp.SetResistance(ResistanceType.Energy, 20);
                        wisp.SetResistance(ResistanceType.Poison, 20);

                        break;
                    }
            }
        }

        internal static int GetRange(string title)
        {
            return 3 * GetTitleMod(title);
        }
    }
}

using Server.Spells;
using Server.Spells.First;
using Server.Spells.Second;
using Server.Spells.Third;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Sixth;
using Server.Spells.Seventh;
using Server.Spells.Eighth;
using Server.Spells.Ninjitsu;

namespace Server.Custom.UOHotBar
{
    internal static class HotBarArt
    {
        internal static int GetSpellArt(Spell spell)
        {
            return CheckAll(spell.GetType().Name);
        }

        internal static int GetMoveArt(SpecialMove move)
        {
            return CheckAll(move.GetType().Name);
        }

        private static int CheckAll(string name)
        {
            switch (name)
            {
                // Magery First

                case nameof(ClumsySpell):
                    {
                        return 2240;
                    }

                case nameof(CreateFoodSpell):
                    {
                        return 2241;
                    }

                case nameof(FeeblemindSpell):
                    {
                        return 2242;
                    }

                case nameof(HealSpell):
                    {
                        return 2243;
                    }

                case nameof(MagicArrowSpell):
                    {
                        return 2244;
                    }

                case nameof(NightSightSpell):
                    {
                        return 2245;
                    }

                case nameof(ReactiveArmorSpell):
                    {
                        return 2246;
                    }

                case nameof(WeakenSpell):
                    {
                        return 2247;
                    }

                // Magery Second

                case nameof(AgilitySpell):
                    {
                        return 2248;
                    }

                case nameof(CunningSpell):
                    {
                        return 2249;
                    }

                case nameof(CureSpell):
                    {
                        return 2250;
                    }

                case nameof(HarmSpell):
                    {
                        return 2251;
                    }

                case nameof(MagicTrapSpell):
                    {
                        return 2252;
                    }

                case nameof(ProtectionSpell):
                    {
                        return 2253;
                    }

                case nameof(RemoveTrapSpell):
                    {
                        return 2254;
                    }

                case nameof(StrengthSpell):
                    {
                        return 2255;
                    }

                // Magery Third

                case nameof(BlessSpell):
                    {
                        return 2256;
                    }

                case nameof(FireballSpell):
                    {
                        return 2257;
                    }

                case nameof(MagicLockSpell):
                    {
                        return 2258;
                    }

                case nameof(PoisonSpell):
                    {
                        return 2259;
                    }

                case nameof(TelekinesisSpell):
                    {
                        return 2260;
                    }

                case nameof(TeleportSpell):
                    {
                        return 2261;
                    }

                case nameof(UnlockSpell):
                    {
                        return 2262;
                    }

                case nameof(WallOfStoneSpell):
                    {
                        return 2263;
                    }

                // Magery Forth

                case nameof(ArchCureSpell):
                    {
                        return 2264;
                    }

                case nameof(ArchProtectionSpell):
                    {
                        return 2265;
                    }

                case nameof(CurseSpell):
                    {
                        return 2266;
                    }

                case nameof(FireFieldSpell):
                    {
                        return 2267;
                    }

                case nameof(GreaterHealSpell):
                    {
                        return 2268;
                    }

                case nameof(LightningSpell):
                    {
                        return 2269;
                    }

                case nameof(ManaDrainSpell):
                    {
                        return 2270;
                    }

                case nameof(RecallSpell):
                    {
                        return 2271;
                    }

                // Magery Fifth

                case nameof(BladeSpiritsSpell):
                    {
                        return 2272;
                    }

                case nameof(DispelFieldSpell):
                    {
                        return 2273;
                    }

                case nameof(IncognitoSpell):
                    {
                        return 2274;
                    }

                case nameof(MagicReflectSpell):
                    {
                        return 2275;
                    }

                case nameof(MindBlastSpell):
                    {
                        return 2276;
                    }

                case nameof(ParalyzeSpell):
                    {
                        return 2277;
                    }

                case nameof(PoisonFieldSpell):
                    {
                        return 2278;
                    }

                case nameof(SummonCreatureSpell):
                    {
                        return 2279;
                    }

                // Magery Sixth

                case nameof(DispelSpell):
                    {
                        return 2280;
                    }

                case nameof(EnergyBoltSpell):
                    {
                        return 2281;
                    }

                case nameof(ExplosionSpell):
                    {
                        return 2282;
                    }

                case nameof(InvisibilitySpell):
                    {
                        return 2283;
                    }

                case nameof(MarkSpell):
                    {
                        return 2284;
                    }

                case nameof(MassCurseSpell):
                    {
                        return 2285;
                    }

                case nameof(ParalyzeFieldSpell):
                    {
                        return 2286;
                    }

                case nameof(RevealSpell):
                    {
                        return 2287;
                    }

                // Magery Seventh

                case nameof(ChainLightningSpell):
                    {
                        return 2288;
                    }

                case nameof(EnergyFieldSpell):
                    {
                        return 2289;
                    }

                case nameof(FlameStrikeSpell):
                    {
                        return 2290;
                    }

                case nameof(GateTravelSpell):
                    {
                        return 2291;
                    }

                case nameof(ManaVampireSpell):
                    {
                        return 2292;
                    }

                case nameof(MassDispelSpell):
                    {
                        return 2293;
                    }

                case nameof(MeteorSwarmSpell):
                    {
                        return 2294;
                    }

                case nameof(PolymorphSpell):
                    {
                        return 2295;
                    }

                // Magery Eighth

                case nameof(EarthquakeSpell):
                    {
                        return 2296;
                    }

                case nameof(EnergyVortexSpell):
                    {
                        return 2297;
                    }

                case nameof(ResurrectionSpell):
                    {
                        return 2298;
                    }

                case nameof(AirElementalSpell):
                    {
                        return 2299;
                    }

                case nameof(SummonDaemonSpell):
                    {
                        return 2300;
                    }

                case nameof(EarthElementalSpell):
                    {
                        return 2301;
                    }

                case nameof(FireElementalSpell):
                    {
                        return 2302;
                    }

                case nameof(WaterElementalSpell):
                    {
                        return 2303;
                    }

                // Ninja

                case nameof(FocusAttack):
                    {
                        return 21280;
                    }

                case nameof(DeathStrike):
                    {
                        return 21281;
                    }

                case nameof(AnimalForm):
                    {
                        return 21282;
                    }

                case nameof(KiAttack):
                    {
                        return 21283;
                    }

                case nameof(SurpriseAttack):
                    {
                        return 21284;
                    }

                case nameof(Backstab):
                    {
                        return 21285;
                    }

                case nameof(Shadowjump):
                    {
                        return 21286;
                    }

                case nameof(MirrorImage):
                    {
                        return 21287;
                    }
            }

            return 2239;
        }
    }
}
